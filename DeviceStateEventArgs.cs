// EventArgs/DeviceStateEventArgs.cs - Аргументи події зміни стану пристрою
using System;

namespace DeviceSimulation.EventArgs
{
    /// <summary>
    /// Клас аргументів події зміни стану пристрою (шаблон Observer)
    /// </summary>
    public class DeviceStateEventArgs : System.EventArgs
    {
        public string Message { get; }

        public DeviceStateEventArgs(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}