using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DLMSLIB;
using Utilities;
using ApplicationInterface;
using SerialCommunication;

namespace CabconPMP
{
    internal sealed class ConnectionInfo
    {
        public string PortName { get; set; }
        public LayerInterface LayerInterface { get; set; }
        public SerialComm SerialComm { get; set; }
        public HDLCLIB HDLCLIB { get; set; }
        public COSEMLIB COSEMLIB { get; set; }
        public GlobalFunctions GlobalFunctions { get; set; }
    }

    internal static class ConnectionRegistry
    {
        private static readonly ConcurrentDictionary<string, ConnectionInfo> _connections = new ConcurrentDictionary<string, ConnectionInfo>(StringComparer.OrdinalIgnoreCase);

        public static bool TryGet(string portName, out ConnectionInfo info)
        {
            return _connections.TryGetValue(portName, out info);
        }

        public static void AddOrUpdate(string portName, ConnectionInfo info)
        {
            _connections.AddOrUpdate(portName, info, (k, v) => info);
        }

        public static bool Remove(string portName)
        {
            ConnectionInfo tmp;
            return _connections.TryRemove(portName, out tmp);
        }
        public static void Clear()
        {
            _connections.Clear();
        }
        public static void MarkConnected(string portName)
        {
            _connections.AddOrUpdate(portName, new ConnectionInfo { PortName = portName }, (k, v) => { v.PortName = portName; return v; });
        }

        public static bool IsConnected(string portName)
        {
            return _connections.ContainsKey(portName);
        }
    }
}
