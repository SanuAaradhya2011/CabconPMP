using ApplicationInterface;
using CabconPMP.datalayer;
using COMMONENTITY;
using DLMSLIB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Utilities;
using static CabconPMP.PortRetryExecutionRunner;


namespace CabconPMP
{
    public sealed class ProcedureMethodBinding
    {
        public ProcedureMethodBinding()
        {
            Arguments = new List<string>();
        }
        public int Order { get; set; }
        public string MethodName { get; set; }
        public string RawLine { get; set; }
        public List<string> Arguments { get; private set; }
    }

    public sealed class RetryRequestedEventArgs : EventArgs
    {
        public RetryRequestedEventArgs(Guid retryId, string portName, string methodName, string error)
        {
            RetryId = retryId;
            PortName = portName;
            MethodName = methodName;
            Error = error;
        }

        public Guid RetryId { get; }
        public string PortName { get; }
        public string MethodName { get; }
        public string Error { get; }
    }

    public class RetryPortExecutionResult
    {
        public RetryPortExecutionResult()
        {
            Activities = new List<string>();
            MethodResults = new List<RetryMethodExecutionResult>();
        }

        public int ThreadIndex { get; set; }
        public int ManagedThreadId { get; set; }
        public string PortName { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public bool PortConnected { get; set; }
        public bool CompletedSuccessfully { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Activities { get; private set; }
        public List<RetryMethodExecutionResult> MethodResults { get; private set; }
        public CalibrationMode Mode { get; set; }

        public TimeSpan Duration
        {
            get
            {
                if (CompletedOn <= StartedOn)
                {
                    return TimeSpan.Zero;
                }

                return CompletedOn - StartedOn;
            }
        }

    }

    public class RetryMethodExecutionResult
    {
        public RetryMethodExecutionResult()
        {
            Activities = new List<string>();
            InputArguments = new List<string>();
        }

        public string MethodName { get; set; }
        public bool Succeeded { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string ReturnValue { get; set; }
        public object RawReturnValue { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public List<string> InputArguments { get; private set; }
        public List<string> Activities { get; private set; }

        public TimeSpan Duration
        {
            get
            {
                if (CompletedOn <= StartedOn)
                {
                    return TimeSpan.Zero;
                }

                return CompletedOn - StartedOn;
            }
        }
    }

    public class PortRetryExecutionRunner
    {
        private readonly object _syncRoot = new object();
        private readonly object _summaryLock = new object();
        // coordination for retry requests
        private readonly ConcurrentDictionary<Guid, ManualResetEventSlim> _retryEvents = new ConcurrentDictionary<Guid, ManualResetEventSlim>();
        private readonly ConcurrentDictionary<Guid, bool> _retryDecisions = new ConcurrentDictionary<Guid, bool>();

        /// <summary>
        /// Raised when the runner requires a user decision to retry a failed method.
        /// UI should show Retry/Cancel and call SignalRetry(retryId, true/false).
        /// </summary>
        public event EventHandler<RetryRequestedEventArgs> RetryRequested;

        private readonly object _benchSyncRoot = new object();
        BenchSimulator _bench = new BenchSimulator();

        /// <summary>
        /// Called by UI to signal retry (true) or cancel (false) for a pending retry request.
        /// </summary>
        public void SignalRetry(Guid retryId, bool retry)
        {
            _retryDecisions[retryId] = retry;
            if (_retryEvents.TryRemove(retryId, out ManualResetEventSlim ev))
            {
                try { ev.Set(); }
                finally { ev.Dispose(); }
            }
        }
        public event LayerInterface.UpdateHandler StatusUpdated;

        /// <summary>
        /// Connect to all configured ports concurrently and return a summary of connection results.
        /// This performs only the ConnectToMeter step for each port and does not execute procedure methods.
        /// </summary>
        public List<RetryPortExecutionResult> ConnectToAllPorts()
        {
            List<string> availablePorts = GetConfiguredPorts();
            List<RetryPortExecutionResult> results = new List<RetryPortExecutionResult>();
            List<Thread> runningThreads = new List<Thread>();

            if (availablePorts.Count == 0)
            {
                return results;
            }

            for (int index = 0; index < availablePorts.Count; index++)
            {
                string portName = availablePorts[index];
                int threadIndex = index;

                Thread workerThread = new Thread(delegate ()
                {
                    RetryPortExecutionResult portResult = new RetryPortExecutionResult();
                    portResult.ThreadIndex = threadIndex;
                    portResult.PortName = portName;
                    portResult.ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
                    portResult.StartedOn = DateTime.Now;
                    portResult.Status = "Running";
                    portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Thread {0} started for {1}.", threadIndex, portName));

                    LayerInterface layerInterface = new LayerInterface();
                    layerInterface.UpdatedLed += ForwardStatusMessage;

                    // Ensure thread-local instances for helper objects
                    GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
                    GlobalObjects.objHDLCLIB = new HDLCLIB();
                    GlobalObjects.objCOSEMLIB = new COSEMLIB();
                    GlobalObjects.objGlobalFunctions = new GlobalFunctions();

                    try
                    {
                        portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Calling ConnectToMeter for {0}.", portName));
                        if (!layerInterface.ConnectToMeter(portName))
                        {
                            string pcbaId = ReadMeterPcbaId(layerInterface);

                            portResult.Status = "Connection Failed";
                            portResult.ErrorMessage = string.Format(CultureInfo.InvariantCulture, "Unable to connect to meter on {0}.", portName);
                            portResult.Activities.Add(portResult.ErrorMessage);
                            portResult.PortConnected = false;
                            // ensure no stale registry entry
                            ConnectionRegistry.Remove(portName);
                        }
                        else
                        {
                            portResult.PortConnected = true;
                            portResult.Status = "Connected";
                            portResult.Activities.Add("ConnectToMeter succeeded.");
                            portResult.CompletedSuccessfully = true;

                            string pcbaId = ReadMeterPcbaId(layerInterface);

                            // persist connection context so Execute (calibration) can reuse it later
                            var info = new ConnectionInfo
                            {
                                PortName = portName,
                                LayerInterface = layerInterface,
                                SerialComm = GlobalObjects.objSerialComm,
                                HDLCLIB = GlobalObjects.objHDLCLIB,
                                COSEMLIB = GlobalObjects.objCOSEMLIB,
                                GlobalFunctions = GlobalObjects.objGlobalFunctions
                            };

                            ConnectionRegistry.AddOrUpdate(portName, info);
                        }
                    }
                    catch (Exception ex)
                    {
                        portResult.Status = "Failed";
                        portResult.ErrorMessage = ex.Message;
                        portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Unhandled exception: {0}", ex.Message));
                    }
                    finally
                    {


                        portResult.CompletedOn = DateTime.Now;

                        lock (_summaryLock)
                        {
                            results.Add(portResult);
                        }

                        layerInterface.UpdatedLed -= ForwardStatusMessage;
                    }
                });

                workerThread.IsBackground = true;
                workerThread.Name = string.Format(CultureInfo.InvariantCulture, "Connect-{0}", portName);
                runningThreads.Add(workerThread);
                workerThread.Start();
            }

            foreach (Thread workerThread in runningThreads)
            {
                workerThread.Join();
            }

            lock (_syncRoot)
            {
                return results.OrderBy(r => r.ThreadIndex).ToList();
            }
        }

        public string ReadMeterPcbaId(LayerInterface layerInterface)
        {
            try
            {
                byte[] pcbaObis = DLMSDataStracture.PCBAIDDataStracture.PCBAIDOBIS;
                byte classCode = DLMSDataStracture.PCBAIDDataStracture.PCBAIDClassID;
                byte attributeId = DLMSDataStracture.PCBAIDDataStracture.PCBAIDValueAttribute;

                int readResponse = layerInterface.ReadDataCommand(pcbaObis, classCode, attributeId);
                if (readResponse != (int)LayerInterface.ProgrammingCode.Success)
                {
                    return string.Empty;
                }

                string[] pcbaData = DLMSDataStracture.DLMSDataFormator(
                    GlobalObjects.objSerialComm.ReceiveBuffer,
                    18,
                    false);

                if (pcbaData != null && pcbaData.Length > 0 && !string.IsNullOrWhiteSpace(pcbaData[0]))
                {
                    return pcbaData[0];
                }

                return BitConverter.ToString(GlobalObjects.objSerialComm.ReceiveBuffer).Replace("-", string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }

        public List<string> LoadProcedureMethodNames()
        {
            return LoadProcedureMethodBindings()
                .Select(binding => binding.MethodName)
                .Where(methodName => !string.IsNullOrWhiteSpace(methodName))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>
        /// Execute the specified procedure methods on a single port and return the result.
        /// This is used to perform a retry on one card/port for a single method.
        /// </summary>
        public RetryPortExecutionResult ExecuteSinglePort(string portName, CalibrationMode mode, int threadIndex, string[] procedureMethodNames)
        {
            var procedureMethods = LoadProcedureMethodBindings();
            var selectedProcedureMethods = FilterProcedureMethods(procedureMethods, procedureMethodNames);

            var results = new List<RetryPortExecutionResult>();
            ExecutePort(portName, threadIndex, mode, selectedProcedureMethods, results);

            return results.FirstOrDefault();
        }

        public List<RetryPortExecutionResult> Execute(string[] procedureMethodNames)
        {
            List<string> availablePorts = GetConfiguredPorts();

            //Call to Bech and Get Power

            List<ProcedureMethodBinding> procedureMethods = LoadProcedureMethodBindings();
            List<RetryPortExecutionResult> results = new List<RetryPortExecutionResult>();
            List<Thread> runningThreads = new List<Thread>();

            if (availablePorts.Count == 0)
            {
                return results;
            }

            if (procedureMethods.Count == 0)
            {
                return results;
            }

            List<ProcedureMethodBinding> selectedProcedureMethods = FilterProcedureMethods(procedureMethods, procedureMethodNames);
            if (selectedProcedureMethods.Count == 0)
            {
                return results;
            }

            CalibrationMode[] modes =
                                        {
                                            CalibrationMode.UPF,
                                            CalibrationMode.Lag,
                                            CalibrationMode.Lead
                                        };

            foreach (CalibrationMode mode in modes)
            {
                for (int index = 0; index < availablePorts.Count; index++)
                {

                    ChangeBenchMode(mode, selectedProcedureMethods);

                    string portName = availablePorts[index];
                    int threadIndex = index;

                    Thread workerThread = new Thread(delegate ()
                    {
                        ExecutePort(portName, threadIndex, mode, selectedProcedureMethods, results);
                    });

                    workerThread.IsBackground = true;
                    workerThread.Name = string.Format(CultureInfo.InvariantCulture, "Retry-{0}", portName);
                    runningThreads.Add(workerThread);
                    workerThread.Start();
                }

                foreach (Thread workerThread in runningThreads)
                {
                    workerThread.Join();
                }
            }

            lock (_syncRoot)
            {
                return results.OrderBy(result => result.ThreadIndex).ToList();
            }
        }
        public enum CalibrationMode
        {
            UPF,
            Lag,
            Lead
        }

        private void ChangeBenchMode(CalibrationMode mode, List<ProcedureMethodBinding> selectedProcedureMethods)
        {
            lock (_benchSyncRoot)
            {
                //Logger.Info(
                //    $"Changing bench mode to [{mode}]...");

                switch (mode)
                {
                    case CalibrationMode.UPF:

                        _bench.SetPowerFactor(
                            CalibrationMode.UPF);

                        break;

                    case CalibrationMode.Lag:

                        _bench.SetPowerFactor(
                            CalibrationMode.Lag); 

                        break;

                    case CalibrationMode.Lead:

                        _bench.SetPowerFactor(
                            CalibrationMode.Lead);

                        break;

                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(mode),
                            mode,
                            "Unsupported calibration mode.");
                }

                WaitForBenchStable();

                //Logger.Info(
                //    $"Bench mode [{mode}] ready.");
            }
        }
        private void WaitForBenchStable()
        {
            const int maxRetry = 30;

            for (int retry = 0; retry < maxRetry; retry++)
            {
                if (_bench.IsStable())
                {
                    return;
                }

                Thread.Sleep(1000);
            }

            throw new TimeoutException(
                "Bench failed to reach stable state.");
        }

        private void ExecutePort(string portName, int threadIndex, CalibrationMode mode, List<ProcedureMethodBinding> procedureMethods, List<RetryPortExecutionResult> results)
        {
            RetryPortExecutionResult portResult = new RetryPortExecutionResult();
            portResult.ThreadIndex = threadIndex;
            portResult.PortName = portName;
            portResult.ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
            portResult.StartedOn = DateTime.Now;
            portResult.Status = "Running";
            portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Thread {0} started for {1}.", threadIndex, portName));

            // Attempt to reuse a previously established connection (from Connect action).
            ConnectionInfo existing;
            bool reusedConnection = false;
            LayerInterface layerInterface = null;
            CommonCommandMethods commandMethods = new CommonCommandMethods();

            if (ConnectionRegistry.TryGet(portName, out existing) && existing != null)
            {
                // reuse the connection context (use the existing LayerInterface and globals)
                reusedConnection = true;

                // restore the GlobalObjects references in this worker thread to point to the
                // same runtime instances created during Connect
                GlobalObjects.objSerialComm = existing.SerialComm ?? new SerialCommunication.SerialComm();
                GlobalObjects.objHDLCLIB = existing.HDLCLIB ?? new HDLCLIB();
                GlobalObjects.objCOSEMLIB = existing.COSEMLIB ?? new COSEMLIB();
                GlobalObjects.objGlobalFunctions = existing.GlobalFunctions ?? new GlobalFunctions();

                // reuse the same LayerInterface instance that established the association
                layerInterface = existing.LayerInterface ?? new LayerInterface();
                layerInterface.UpdatedLed += ForwardStatusMessage;
                InjectLayerInterface(commandMethods, layerInterface);
            }
            else
            {
                // No existing connection - remove stale connection entry for this port and skip calibration for this port.
                ConnectionRegistry.Remove(portName);

                // Request UI to clear previously rendered cards for this specific port before logging skip activities.
                try
                {
                    var notifier = new LayerInterface();
                    notifier.UpdatedLed += ForwardStatusMessage;
                    notifier.DisplayStatusMsg(string.Format(CultureInfo.InvariantCulture, "ClearCards|{0}", portName), false);
                    notifier.UpdatedLed -= ForwardStatusMessage;
                }
                catch
                {
                    // ignore notifier failures
                }

                portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Cleared connection entry for {0} before skipping.", portName));
                portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Port {0} not connected via Connect action. Skipping calibration for this port.", portName));
                portResult.Status = string.Format(CultureInfo.InvariantCulture, "Skipped - Not Connected ({0})", portName);
                portResult.CompletedOn = DateTime.Now;
                portResult.Mode = mode;
                lock (_summaryLock)
                {
                    results.Add(portResult);
                }
                return;
            }

            try
            {
                //portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Calling ConnectToMeter for {0}.", portName));
                //if (!layerInterface.ConnectToMeter(portName))
                //{
                //    portResult.Status = "Connection Failed";
                //    portResult.ErrorMessage = string.Format(CultureInfo.InvariantCulture, "Unable to connect to meter on {0}.", portName);
                //    portResult.Activities.Add("ConnectToMeter succeeded.{Thread.CurrentThread.ManagedThreadId} timestamp {DateTime.Now:HH:mm:ss:fffffff}");
                //    return;
                //}

                //portResult.PortConnected = true;
                //portResult.Activities.Add("ConnectToMeter succeeded.");

                string previousResultText = string.Empty;
                object previousRawResult = null;

                bool stopProcessing = false;
                foreach (ProcedureMethodBinding procedureMethod in procedureMethods)
                {
                    // Check Mode and skip methods that are not applicable for the current calibration mode
                    RetryMethodExecutionResult methodResult = ExecuteProcedureMethod(
                        commandMethods,
                        procedureMethod,
                        previousRawResult,
                        previousResultText);

                    portResult.MethodResults.Add(methodResult);
                    portResult.Activities.AddRange(methodResult.Activities);

                    // If invocation failed, loop and wait for UI decision to retry or cancel
                    while (!methodResult.Succeeded)
                    {
                        Guid retryId = Guid.NewGuid();
                        var ev = new ManualResetEventSlim(false);
                        _retryEvents[retryId] = ev;

                        // notify UI
                        RetryRequested?.Invoke(this, new RetryRequestedEventArgs(
                            retryId,
                            portName,
                            procedureMethod.MethodName,
                            methodResult.ErrorMessage ?? string.Empty));

                        // wait for UI to call SignalRetry
                        ev.Wait();

                        bool doRetry = false;
                        _retryDecisions.TryRemove(retryId, out doRetry);

                        // ensure event removed and disposed
                        if (_retryEvents.TryRemove(retryId, out ManualResetEventSlim removed))
                        {
                            try { removed.Dispose(); } catch { }
                        }

                        if (!doRetry)
                        {
                            portResult.Status = "Failed";
                            portResult.CompletedSuccessfully = false;
                            portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "User cancelled retry for method {0}.", procedureMethod.MethodName));
                            stopProcessing = true;
                            break; // exit retry loop
                        }

                        // perform retry
                        methodResult = ExecuteProcedureMethod(
                            commandMethods,
                            procedureMethod,
                            previousRawResult,
                            previousResultText);

                        portResult.MethodResults.Add(methodResult);
                        portResult.Activities.AddRange(methodResult.Activities);

                        // loop again if still failed
                    }

                    if (stopProcessing)
                    {
                        break; // stop processing remaining methods for this port
                    }

                    // If succeeded, update previous result and continue
                    previousRawResult = methodResult.RawReturnValue;
                    previousResultText = methodResult.ReturnValue;
                }

                portResult.CompletedSuccessfully = true;
                portResult.Status = "Completed";
            }
            catch (Exception ex)
            {
                portResult.Status = "Failed";
                portResult.ErrorMessage = ex.Message;
                portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Unhandled exception: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    layerInterface.AssociationDisconnect();
                    portResult.Activities.Add("Association disconnected.");
                }
                catch (Exception disconnectEx)
                {
                    portResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Association disconnect failed: {0}", disconnectEx.Message));
                }

                portResult.CompletedOn = DateTime.Now;

                lock (_summaryLock)
                {
                    results.Add(portResult);
                }

                layerInterface.UpdatedLed -= ForwardStatusMessage;
            }
        }

        private void ForwardStatusMessage(object sender, UpdateEventArgs e)
        {
            StatusUpdated?.Invoke(sender, e);
        }



        private List<string> GetConfiguredPorts()
        {
            //List<string> selectedPorts = ParseConfiguredPorts(SerialPortSettings.Default.SelectedPortsCsv);
            //if (selectedPorts.Count > 0)
            //{
            //    return selectedPorts;
            //}

            return GetAvailablePorts();
        }

        private RetryMethodExecutionResult ExecuteProcedureMethod(
            CommonCommandMethods commandMethods,
            ProcedureMethodBinding procedureMethod,
            object previousRawResult,
            string previousResultText)
        {
            RetryMethodExecutionResult methodResult = new RetryMethodExecutionResult();
            methodResult.MethodName = procedureMethod.MethodName;
            methodResult.StartedOn = DateTime.Now;
            methodResult.Status = "Running";
            methodResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Preparing method {0}.", procedureMethod.MethodName));

            MethodInfo methodInfo = typeof(CommonCommandMethods).GetMethod(
                procedureMethod.MethodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (methodInfo == null)
            {
                methodResult.Status = "Method Not Found";
                methodResult.ErrorMessage = string.Format(CultureInfo.InvariantCulture, "Method {0} was not found on CommonCommandMethods.", procedureMethod.MethodName);
                methodResult.Activities.Add(methodResult.ErrorMessage);
                methodResult.CompletedOn = DateTime.Now;
                return methodResult;
            }

            object[] methodArguments = BuildMethodArguments(
                methodInfo,
                procedureMethod,
                previousRawResult,
                previousResultText,
                methodResult.Activities);

            methodResult.InputArguments.Clear();
            for (int index = 0; index < methodArguments.Length; index++)
            {
                methodResult.InputArguments.Add(Convert.ToString(methodArguments[index], CultureInfo.InvariantCulture));
            }

            try
            {

                object returnValue = methodInfo.Invoke(commandMethods, methodArguments);
                methodResult.RawReturnValue = returnValue;
                methodResult.ReturnValue = Convert.ToString(returnValue, CultureInfo.InvariantCulture);
                methodResult.Succeeded = IsSuccessfulReturnValue(returnValue);
                methodResult.Status = methodResult.Succeeded ? "Completed" : "Completed With Error";
                methodResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Returned value: {0}", string.IsNullOrEmpty(methodResult.ReturnValue) ? "<null>" : methodResult.ReturnValue));
            }
            catch (TargetInvocationException ex)
            {
                Exception rootException = ex.InnerException ?? ex;
                methodResult.Status = "Failed";
                methodResult.ErrorMessage = rootException.Message;
                methodResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Invocation failed: {0}", rootException.Message));
            }
            catch (Exception ex)
            {
                methodResult.Status = "Failed";
                methodResult.ErrorMessage = ex.Message;
                methodResult.Activities.Add(string.Format(CultureInfo.InvariantCulture, "Invocation failed: {0}", ex.Message));
            }
            finally
            {
                methodResult.CompletedOn = DateTime.Now;
            }

            return methodResult;
        }

        private object[] BuildMethodArguments(
            MethodInfo methodInfo,
            ProcedureMethodBinding procedureMethod,
            object previousRawResult,
            string previousResultText,
            List<string> activities)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters == null || parameters.Length == 0)
            {
                activities.Add(string.Format(CultureInfo.InvariantCulture, "No arguments required for {0}.", procedureMethod.MethodName));
                return new object[0];
            }

            object[] resolvedArguments = new object[parameters.Length];
            int bindingIndex = 0;
            string previousResultValue = !string.IsNullOrWhiteSpace(previousResultText)
                ? previousResultText
                : Convert.ToString(previousRawResult, CultureInfo.InvariantCulture);

            for (int index = 0; index < parameters.Length; index++)
            {
                string rawValue = bindingIndex < procedureMethod.Arguments.Count
                    ? procedureMethod.Arguments[bindingIndex]
                    : string.Empty;

                if (IsPreviousResultToken(rawValue))
                {
                    rawValue = previousResultValue;
                }
                else if (string.IsNullOrWhiteSpace(rawValue) && index == 0 && !string.IsNullOrWhiteSpace(previousResultValue))
                {
                    rawValue = previousResultValue;
                }

                resolvedArguments[index] = ConvertArgument(rawValue, parameters[index].ParameterType);
                bindingIndex++;
            }

            activities.Add(string.Format(CultureInfo.InvariantCulture, "Resolved {0} argument(s) for {1} from ProcedureDetails.txt.", parameters.Length, procedureMethod.MethodName));
            return resolvedArguments;
        }

        private object ConvertArgument(string rawValue, Type parameterType)
        {
            if (parameterType == typeof(string))
            {
                return rawValue ?? string.Empty;
            }

            if (parameterType == typeof(int))
            {
                int intValue;
                string sanitized = SanitizeNumericToken(rawValue);
                return int.TryParse(sanitized, NumberStyles.Integer, CultureInfo.InvariantCulture, out intValue) ? intValue : 0;
            }

            if (parameterType == typeof(byte))
            {
                byte byteValue;
                string sanitized = SanitizeNumericToken(rawValue);
                return byte.TryParse(sanitized, NumberStyles.Integer, CultureInfo.InvariantCulture, out byteValue) ? byteValue : (byte)0;
            }

            if (parameterType == typeof(bool))
            {
                bool boolValue;
                return bool.TryParse(rawValue, out boolValue) ? boolValue : false;
            }

            if (parameterType == typeof(decimal))
            {
                decimal decimalValue;
                string sanitized = SanitizeNumericToken(rawValue);
                return decimal.TryParse(sanitized, NumberStyles.Number, CultureInfo.InvariantCulture, out decimalValue) ? decimalValue : 0m;
            }

            if (parameterType == typeof(double))
            {
                double doubleValue;
                string sanitized = SanitizeNumericToken(rawValue);
                return double.TryParse(sanitized, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out doubleValue) ? doubleValue : 0d;
            }

            if (parameterType == typeof(long))
            {
                long longValue;
                string sanitized = SanitizeNumericToken(rawValue);
                return long.TryParse(sanitized, NumberStyles.Integer, CultureInfo.InvariantCulture, out longValue) ? longValue : 0L;
            }

            return rawValue;
        }

        private static string SanitizeNumericToken(string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return string.Empty;
            }

            string trimmed = rawValue.Trim();

            // Extract the leading numeric token (handles values like "230V", "10I", "0.0", "-12.5")
            Match m = Regex.Match(trimmed, "^[+-]?\\d+(?:\\.\\d+)?");
            if (m.Success)
            {
                return m.Value;
            }

            // Fallback: return the trimmed input (will likely fail parse and default used)
            return trimmed;
        }

        private bool IsSuccessfulReturnValue(object returnValue)
        {
            if (returnValue == null)
            {
                return false;
            }

            if (returnValue is bool)
            {
                return (bool)returnValue;
            }

            string returnText = Convert.ToString(returnValue, CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(returnText))
            {
                return false;
            }

            return returnText.IndexOf(StaticVariables.ERRORPreFix, StringComparison.OrdinalIgnoreCase) < 0;
        }

        private List<ProcedureMethodBinding> LoadProcedureMethodBindings()
        {
            string procedureDetailsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProcedureDetails.txt");
            List<ProcedureMethodBinding> methodBindings = new List<ProcedureMethodBinding>();

            if (!File.Exists(procedureDetailsPath))
            {
                return methodBindings;
            }

            string[] lines = File.ReadAllLines(procedureDetailsPath);
            for (int index = 0; index < lines.Length; index++)
            {
                ProcedureMethodBinding binding = ParseProcedureLine(lines[index], index);
                if (binding != null && !string.IsNullOrWhiteSpace(binding.MethodName))
                {
                    methodBindings.Add(binding);
                }
            }

            return methodBindings
                .OrderBy(binding => binding.Order)
                .ToList();
        }

        private List<ProcedureMethodBinding> FilterProcedureMethods(List<ProcedureMethodBinding> loadedMethods, string[] procedureMethodNames)
        {
            if (loadedMethods == null || loadedMethods.Count == 0)
            {
                return new List<ProcedureMethodBinding>();
            }

            if (procedureMethodNames == null || procedureMethodNames.Length == 0)
            {
                return loadedMethods
                    .Where(binding => !string.IsNullOrWhiteSpace(binding.MethodName))
                    .Distinct(new ProcedureMethodBindingComparer())
                    .ToList();
            }

            HashSet<string> requestedMethods = new HashSet<string>(procedureMethodNames.Where(name => !string.IsNullOrWhiteSpace(name)), StringComparer.OrdinalIgnoreCase);
            return loadedMethods
                .Where(binding => requestedMethods.Contains(binding.MethodName))
                .OrderBy(binding => binding.Order)
                .ToList();
        }

        private ProcedureMethodBinding ParseProcedureLine(string rawLine, int fallbackOrder)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                return null;
            }

            string trimmedLine = rawLine.Trim();
            int separatorIndex = trimmedLine.IndexOf('.');
            if (separatorIndex >= 0 && separatorIndex < trimmedLine.Length - 1)
            {
                string orderToken = trimmedLine.Substring(0, separatorIndex).Trim();
                int parsedOrder;
                if (int.TryParse(orderToken, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedOrder))
                {
                    fallbackOrder = parsedOrder;
                }

                trimmedLine = trimmedLine.Substring(separatorIndex + 1).Trim();
            }

            // Support both formats:
            //  - MethodName|arg1|arg2
            //  - MethodName,arg1,arg2
            //  - MethodName(arg1,arg2, ...)
            string methodName = null;
            List<string> arguments = new List<string>();

            // Check for parentheses style: MethodName(arg1,arg2,...)
            int openParen = trimmedLine.IndexOf('(');
            int closeParen = trimmedLine.LastIndexOf(')');
            if (openParen > 0 && closeParen > openParen)
            {
                methodName = trimmedLine.Substring(0, openParen).Trim();
                string inside = trimmedLine.Substring(openParen + 1, closeParen - openParen - 1);
                if (!string.IsNullOrWhiteSpace(inside))
                {
                    var innerTokens = inside.Split(new[] { ',' }, StringSplitOptions.None);
                    foreach (var t in innerTokens)
                    {
                        var a = t.Trim();
                        if (a.Length > 0) arguments.Add(a);
                    }
                }
            }
            else
            {
                // Try pipe-separated first, then comma-separated
                string[] tokens = trimmedLine.Split(new[] { '|' }, StringSplitOptions.None);
                if (tokens.Length == 1)
                {
                    tokens = trimmedLine.Split(new[] { ',' }, StringSplitOptions.None);
                }

                if (tokens.Length == 0)
                {
                    return null;
                }

                methodName = tokens[0].Trim();
                for (int index = 1; index < tokens.Length; index++)
                {
                    string argument = tokens[index].Trim();
                    if (argument.Length > 0)
                    {
                        arguments.Add(argument);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                return null;
            }

            ProcedureMethodBinding binding = new ProcedureMethodBinding();
            binding.Order = fallbackOrder;
            binding.MethodName = methodName;
            binding.RawLine = rawLine;
            foreach (var a in arguments) binding.Arguments.Add(a);

            return binding;
        }

        private List<string> GetAvailablePorts()
        {
            //SerialComm serialComm = new SerialComm();
            string[] ports = new string[] { "COM5" };//serialComm.GetAvailablePorts();
            if (ports == null)
            {
                return new List<string>();
            }

            return ports
                .Where(portName => !string.IsNullOrWhiteSpace(portName))
                .Select(portName => portName.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(portName => ParsePortPosition(portName, int.MaxValue))
                .ThenBy(portName => portName, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private int ParsePortPosition(string portName, int fallbackPosition)
        {
            if (string.IsNullOrWhiteSpace(portName))
            {
                return fallbackPosition;
            }

            string normalizedPortName = portName.Trim().ToUpperInvariant();
            if (!normalizedPortName.StartsWith("COM", StringComparison.Ordinal))
            {
                return fallbackPosition;
            }

            string numericPort = normalizedPortName.Substring(3);
            int portNumber;
            if (int.TryParse(numericPort, NumberStyles.Integer, CultureInfo.InvariantCulture, out portNumber))
            {
                return portNumber;
            }

            return fallbackPosition;
        }

        private bool IsPreviousResultToken(string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return false;
            }

            string normalized = rawValue.Trim();
            return string.Equals(normalized, "{previous}", StringComparison.OrdinalIgnoreCase)
                || string.Equals(normalized, "{previousresult}", StringComparison.OrdinalIgnoreCase)
                || string.Equals(normalized, "{prev}", StringComparison.OrdinalIgnoreCase)
                || string.Equals(normalized, "$PREVIOUS_RESULT", StringComparison.OrdinalIgnoreCase);
        }

        private void InjectLayerInterface(CommonCommandMethods commandMethods, LayerInterface layerInterface)
        {
            FieldInfo layerField = typeof(CommonCommandMethods).GetField("objLI", BindingFlags.Instance | BindingFlags.NonPublic);
            if (layerField == null)
            {
                return;
            }

            layerField.SetValue(commandMethods, layerInterface);
        }

        private sealed class ProcedureMethodBindingComparer : IEqualityComparer<ProcedureMethodBinding>
        {
            public bool Equals(ProcedureMethodBinding x, ProcedureMethodBinding y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return string.Equals(x.MethodName, y.MethodName, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(ProcedureMethodBinding obj)
            {
                if (obj == null || string.IsNullOrWhiteSpace(obj.MethodName))
                {
                    return 0;
                }

                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.MethodName);
            }
        }
    }
}
