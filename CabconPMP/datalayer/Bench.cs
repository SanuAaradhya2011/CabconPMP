using System;
using System.Threading;
using static CabconPMP.PortRetryExecutionRunner;

namespace CabconPMP.datalayer
{
    public sealed class BenchSimulator
    {
        private readonly object _syncRoot =
            new object();

        private CalibrationMode _currentMode;

        public CalibrationMode CurrentMode
        {
            get
            {
                lock (_syncRoot)
                {
                    return _currentMode;
                }
            }
        }

        public void SetPowerFactor(
            CalibrationMode mode)
        {
            lock (_syncRoot)
            {
                Console.WriteLine(
                    $"[BENCH] Changing Power Factor Mode to {mode}");

                // Simulate bench hardware delay
                Thread.Sleep(2000);

                _currentMode = mode;

                Console.WriteLine(
                    $"[BENCH] Power Factor Mode changed to {mode}");
            }
        }

        public bool IsStable()
        {
            lock (_syncRoot)
            {
                return true;
            }
        }
    }
    public enum PowerFactorMode
    {
        UPF,
        Lag,
        Lead
    }
}
