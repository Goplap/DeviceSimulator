// EventArgs/NetworkStateEventArgs.cs - Аргументи події зміни стану мережі
using System;

namespace DeviceSimulation.EventArgs
{
    /// <summary>
    /// Клас аргументів події зміни стану мережі (шаблон Observer)
    /// </summary>
    public class NetworkStateEventArgs : System.EventArgs
    {
        public bool IsConnected { get; }

        public NetworkStateEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }
    }
}