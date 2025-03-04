// Components/Memory.cs - Клас пам'яті
using System;

namespace DeviceSimulation.Components
{
    /// <summary>
    /// Клас, що представляє пам'ять пристрою
    /// </summary>
    public class Memory
    {
        public string Type { get; }
        public int CapacityGB { get; }

        public Memory(string type, int capacityGB)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            CapacityGB = capacityGB > 0 ? capacityGB : throw new ArgumentException("Ємність пам'яті має бути більше 0", nameof(capacityGB));
        }
    }
}