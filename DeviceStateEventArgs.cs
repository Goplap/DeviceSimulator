// EventArgs/DeviceStateEventArgs.cs - Аргументи події зміни стану пристрою
using System;

namespace DeviceSimulation.EventArgs
{
    public class DeviceStateEventArgs : System.EventArgs
    {
        public string Message { get; }

        public DeviceStateEventArgs(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}