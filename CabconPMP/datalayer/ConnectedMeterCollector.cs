using DataLayer;
using SmartCalibration.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationInterface;
using Utilities;

namespace SmartCalibration.DataLayer
{
    public class ConnectedMeterCollector
    {
        private readonly List<string> _candidatePorts;

        public ConnectedMeterCollector()
            : this(new[] { "COM5", "COM6", "COM7" })
        {
        }

        public ConnectedMeterCollector(IEnumerable<string> candidatePorts)
        {
            _candidatePorts = new List<string>();

            if (candidatePorts == null)
            {
                return;
            }

            foreach (string portName in candidatePorts)
            {
                if (string.IsNullOrWhiteSpace(portName))
                {
                    continue;
                }

                string trimmedPortName = portName.Trim();

                if (_candidatePorts.Any(existingPort =>
                    string.Equals(existingPort, trimmedPortName, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                _candidatePorts.Add(trimmedPortName);
            }
        }

        public List<Meter> CollectConnectedMeters()
        {
            List<Meter> connectedMeters = new List<Meter>();

            GlobalConstants.MeterPortMap.Clear();

            foreach (string portName in _candidatePorts)
            {
                Meter meter = TryConnectMeter(portName);
                if (meter == null || meter.IsConnected == false)
                {
                    continue;
                }

                meter.ID = connectedMeters.Count + 1;
                meter.mpos = ParsePortPosition(portName, connectedMeters.Count);
                connectedMeters.Add(meter);
                GlobalConstants.MeterPortMap[meter.mpos] = portName;
            }

            return connectedMeters;
        }

        private Meter TryConnectMeter(string portName)
        {
            LayerInterface layerInterface = new LayerInterface();

            try
            {
                if (!layerInterface.ConnectToMeter(portName))
                {
                    return new Meter
                    {
                        PortName = portName,
                        IsConnected = false,
                        mstatus = false
                    };
                }

                string rtcValue = ReadMeterRtc(layerInterface);
                string pcbaId = ReadMeterPcbaId(layerInterface);
                layerInterface.AssociationDisconnect();

                return new Meter
                {
                    PortName = portName,
                    IsConnected = true,
                    mstatus = true,
                    RTCValue = rtcValue,
                    PCBAId = pcbaId
                };
            }
            catch
            {
                try
                {
                    layerInterface.AssociationDisconnect();
                }
                catch
                {
                }

                return new Meter
                {
                    PortName = portName,
                    IsConnected = false,
                    mstatus = false
                };
            }
        }

        private static string ReadMeterRtc(LayerInterface layerInterface)
        {
            try
            {
                byte[] meterRtcObis = DLMSDataStracture.MeterRTCDataStracture.MeterRTCOBIS;
                byte classCode = DLMSDataStracture.MeterRTCDataStracture.MeterRTCClassID;
                byte attributeId = DLMSDataStracture.MeterRTCDataStracture.MeterRTCValueAttribute;

                int readResponse = layerInterface.ReadDataCommand(meterRtcObis, classCode, attributeId);
                if (readResponse != (int)LayerInterface.ProgrammingCode.Success)
                {
                    return string.Empty;
                }

                string[] rtcData = DLMSDataStracture.DLMSDataFormator(
                    GlobalObjects.objSerialComm.ReceiveBuffer,
                    18,
                    false);

                if (rtcData != null && rtcData.Length > 0 && !string.IsNullOrWhiteSpace(rtcData[0]))
                {
                    return rtcData[0];
                }

                return BitConverter.ToString(GlobalObjects.objSerialComm.ReceiveBuffer).Replace("-", string.Empty);
            }
            catch
            {
                return string.Empty;
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

        public string GetMeterPcbaId(string portName)
        {
            LayerInterface layerInterface = new LayerInterface();

            try
            {
                if (!layerInterface.ConnectToMeter(portName))
                {
                    return string.Empty;
                }

                return ReadMeterPcbaId(layerInterface);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                try
                {
                    layerInterface.AssociationDisconnect();
                }
                catch
                {
                }
            }
        }

        private static int ParsePortPosition(string portName, int fallbackPosition)
        {
            if (string.IsNullOrWhiteSpace(portName))
            {
                return fallbackPosition;
            }

            string normalizedPort = portName.Trim().ToUpperInvariant();

            if (!normalizedPort.StartsWith("COM"))
            {
                return fallbackPosition;
            }

            string numericPort = normalizedPort.Substring(3);
            int portNumber;
            if (int.TryParse(numericPort, out portNumber))
            {
                return portNumber;
            }

            return fallbackPosition;
        }
    }
}
