// EventArgs/BatteryLevelEventArgs.cs - Аргументи події низького заряду батареї
using System;

namespace DeviceSimulation.EventArgs
{
    /// <summary>
    /// Клас аргументів події низького заряду батареї (шаблон Observer)
    /// </summary>
    public class BatteryLevelEventArgs : System.EventArgs
    {
        public int BatteryLevel { get; }

        public BatteryLevelEventArgs(int batteryLevel)
        {
            BatteryLevel = batteryLevel;
        }
    }
}