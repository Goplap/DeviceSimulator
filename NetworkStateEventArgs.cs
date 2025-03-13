// EventArgs/NetworkStateEventArgs.cs - Аргументи події зміни стану мережі
using System;

namespace DeviceSimulation.EventArgs
{
    public class NetworkStateEventArgs : System.EventArgs
    {
        public bool IsConnected { get; }

        public NetworkStateEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }
    }
}