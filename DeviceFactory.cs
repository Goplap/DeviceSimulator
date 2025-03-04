// Factories/DeviceFactory.cs - Фабрика для створення пристроїв
using System;
using DeviceSimulation.Devices;

namespace DeviceSimulation
{
    /// <summary>
    /// Фабрика для створення пристроїв (шаблон Factory Method)
    /// </summary>
    public class DeviceFactory : IDeviceFactory
    {
        /// <summary>
        /// Створює пристрій вказаного типу
        /// </summary>
        public IDevice CreateDevice(DeviceType type)
        {
            // Шаблон Factory Method - створюємо конкретний пристрій залежно від типу
            return type switch
            {
                DeviceType.Computer => new Computer(),
                DeviceType.Laptop => new Laptop(),
                DeviceType.Smartphone => new Smartphone(),
                _ => throw new ArgumentException("Невідомий тип пристрою")
            };
        }
    }
}