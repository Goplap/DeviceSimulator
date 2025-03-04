// Components/Processor.cs - Клас процесора
using System;

namespace DeviceSimulation.Components
{
    /// <summary>
    /// Клас, що представляє процесор пристрою
    /// </summary>
    public class Processor
    {
        public string Model { get; }
        public int Cores { get; }
        public double ClockSpeedGHz { get; }

        public Processor(string model, int cores, double clockSpeedGHz)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Cores = cores > 0 ? cores : throw new ArgumentException("Кількість ядер має бути більше 0", nameof(cores));
            ClockSpeedGHz = clockSpeedGHz > 0 ? clockSpeedGHz : throw new ArgumentException("Тактова частота має бути більше 0", nameof(clockSpeedGHz));
        }
    }
}