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

            // Використання шаблону Фасад (Facade) - DeviceSimulator надає простий інтерфейс 
            // для взаємодії зі складною підсистемою симуляції пристроїв
            var simulator = new DeviceSimulator();
            simulator.Run();
        }
    }
}