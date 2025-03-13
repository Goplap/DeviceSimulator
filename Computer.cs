// Devices/Computer.cs - Конкретна реалізація комп'ютера
using System;
using System.Diagnostics;
using DeviceSimulation.Components;
using DeviceSimulation.EventArgs;

namespace DeviceSimulation.Devices
{
    /// <summary>
    /// Клас Computer представляє стаціонарний комп'ютер
    /// Реалізує конкретну стратегію для пристрою типу "Комп'ютер" (шаблон Strategy)
    /// </summary>
    public class Computer : BaseDevice
    {
        public bool HasDedicatedGPU { get; }
        public bool HasUPS { get; private set; }
        public int UPSTimeMinutes { get; private set; }
        public bool IsMonitorConnected { get; private set; }

        public Computer()
            : base(DeviceType.Computer, 0,
                  new Processor("AMD Ryzen 7", 8, 3.8),
                  new Memory("DDR4", 16))
        {
            HasDedicatedGPU = true;
            HasUPS = false;
            UPSTimeMinutes = 0;
            HasPowerSupport = true;
            IsMonitorConnected = true;

            // Встановлення основного ПЗ
            InstallSoftware("Операційна система");
            InstallSoftware("Браузер");
            InstallSoftware("Месенджер");
            InstallSoftware("Ютуб");
        }

        public override void DisplaySpecificOptions()
        {
            Console.WriteLine("C. Підключити/відключити ДБЖ");
            Console.WriteLine("G. Запустити гру (потребує потужного GPU)");
        }

        public override bool HandleSpecificOption(char choice)
        {
            switch (choice)
            {
                case 'c':
                case 'C':
                    ToggleUPS();
                    return true;
                case 'g':
                case 'G':
                    PlayGame();
                    return true;
                default:
                    return false;
            }
        }

        private void ToggleUPS()
        {
            HasUPS = !HasUPS;
            UPSTimeMinutes = HasUPS ? 30 : 0;

            // Генеруємо подію зміни стану пристрою
            OnDeviceStateChanged(new DeviceStateEventArgs(
                $"ДБЖ {(HasUPS ? "підключено" : "відключено")}"));

            Console.WriteLine(HasUPS ?
                $"ДБЖ підключено. Час автономної роботи: {UPSTimeMinutes} хв." :
                "ДБЖ відключено.");
        }

        /// <summary>
        /// Запуск гри на комп'ютері
        /// </summary>
        private void PlayGame()
        {
            if (!HasDedicatedGPU)
            {
                Console.WriteLine("Неможливо запустити гру без потужної відеокарти.");
                return;
            }

            if (CanPerformAction(false, new[] { "Гра" }, null))
            {
                // Генеруємо подію зміни стану пристрою
                OnDeviceStateChanged(new DeviceStateEventArgs("Гра запущена"));

                SimulateProcessorLoad(LoadIntensity.High);
                Console.WriteLine("Гра запущена. Насолоджуйтесь максимальною якістю графіки!");
            }
            else
            {
                Console.WriteLine("Будь ласка, встановіть гру перед запуском.");
            }
        }
        public override void ToggleHeadphonesConnection()
        {
            HeadphonesConnected = !HeadphonesConnected;

            // Генеруємо подію зміни стану пристрою
            OnDeviceStateChanged(new DeviceStateEventArgs(
                $"Гарнітура {(HeadphonesConnected ? "підключена" : "відключена")}"));
        }

        /// <summary>
        /// Відображення специфічної інформації про комп'ютер (шаблон Template Method)
        /// </summary>
        protected override void DisplaySpecificInfo()
        {
            Console.WriteLine($"Наявність відеокарти: {(HasDedicatedGPU ? "Так" : "Ні")}");
            Console.WriteLine($"Джерело безперебійного живлення: {(HasUPS ? $"Підключено ({UPSTimeMinutes} хв)" : "Відсутнє")}");
        }

        /// <summary>
        /// Перевизначення розрахунку залишкового часу роботи від батареї для комп'ютера
        /// </summary>
        public override double CalculateBatteryLife()
        {
            return HasUPS ? UPSTimeMinutes / 60.0 : 0;
        }
        protected override void DisplayPeripheralConnections()
        {
            Console.WriteLine($"Монітор: {(IsMonitorConnected ? "Підключений" : "Не підключений")}");
        }

        protected override void DisplayProcessorInfo()
        {
            Console.WriteLine($"Процесор: {DeviceProcessor.Model}, {DeviceProcessor.Cores} ядер, {DeviceProcessor.ClockSpeedGHz} ГГц");
        }

        protected override void DisplayMemoryInfo()
        {
            Console.WriteLine($"Оперативна пам'ять: {DeviceMemory.Type}, {DeviceMemory.CapacityGB} ГБ");
        }

        protected override void DisplayInstalledSoftware()
        {
            Console.WriteLine("Встановлене ПЗ: " + (InstalledSoftware.Any() ? string.Join(", ", InstalledSoftware) : "Немає"));
        }
    }
}