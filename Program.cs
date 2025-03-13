// Program.cs - Головний файл програми, що запускає симулятор пристроїв
using System;
using DeviceSimulation;

namespace lab1
{
    class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IDeviceFactory factory = new DeviceFactory();

            var simulator = new DeviceSimulator();
            simulator.Run();
        }
    }
}