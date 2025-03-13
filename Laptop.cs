// Devices/Laptop.cs - Конкретна реалізація ноутбука
using System;
using DeviceSimulation.Components;
using DeviceSimulation.EventArgs;

namespace DeviceSimulation.Devices
{
    public class Laptop : BaseDevice
    {
        public bool IsTouchscreen { get; }
        public string DisplayResolution { get; }
        public bool IsDockingStationConnected { get; private set; }

        public Laptop()
            : base(DeviceType.Laptop, 5500,
                  new Processor("Intel Core i7", 6, 2.8),
                  new Memory("DDR4", 8))
        {
            IsTouchscreen = true;
            DisplayResolution = "1920x1080";
            IsDockingStationConnected = false;
            HasPowerSupport = true;

            // Встановлення основного ПЗ
            InstallSoftware("Операційна система");
            InstallSoftware("Браузер");
            InstallSoftware("Месенджер");
            InstallSoftware("Ютуб");
            InstallSoftware("Офісний пакет");
        }

        public override void DisplaySpecificOptions()
        {
            Console.WriteLine("D. Підключити/відключити док-станцію");
            Console.WriteLine("P. Презентаційний режим");
            Console.WriteLine("E. Режим економії енергії");
        }

        public override bool HandleSpecificOption(char choice)
        {
            switch (choice)
            {
                case 'd':
                case 'D':
                    ToggleDockingStation();
                    return true;
                case 'p':
                case 'P':
                    PresentationMode();
                    return true;
                case 'e':
                case 'E':
                    PowerSavingMode();
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Підключення/відключення док-станції
        /// </summary>
        private void ToggleDockingStation()
        {
            IsDockingStationConnected = !IsDockingStationConnected;

            // Генеруємо подію зміни стану пристрою
            OnDeviceStateChanged(new DeviceStateEventArgs(
                $"Док-станція {(IsDockingStationConnected ? "підключена" : "відключена")}"));

            Console.WriteLine(IsDockingStationConnected ?
                "Док-станція підключена. Доступні додаткові порти та зовнішній монітор." :
                "Док-станція відключена.");
        }

        /// <summary>
        /// Режим презентації
        /// </summary>
        private void PresentationMode()
        {
            if (CanPerformAction(false, new[] { "Офісний пакет" }, null))
            {
                // Генеруємо подію зміни стану пристрою
                OnDeviceStateChanged(new DeviceStateEventArgs("Режим презентації активовано"));

                SimulateProcessorLoad(LoadIntensity.Medium);
                SimulateBatteryUsage(LoadIntensity.Medium);
                Console.WriteLine("Режим презентації активовано. Презентація готова до показу.");
            }
            else
            {
                Console.WriteLine("Для презентації потрібен офісний пакет.");
            }
        }

        /// <summary>
        /// Режим економії енергії
        /// </summary>
        private void PowerSavingMode()
        {
            // Генеруємо подію зміни стану пристрою
            OnDeviceStateChanged(new DeviceStateEventArgs("Режим економії енергії активовано"));

            Console.WriteLine("Режим економії енергії активовано. Продуктивність знижена, але час автономної роботи збільшено.");
        }

        public override void ToggleHeadphonesConnection()
        {
            HeadphonesConnected = !HeadphonesConnected;

            // Генеруємо подію зміни стану пристрою
            OnDeviceStateChanged(new DeviceStateEventArgs(
                $"Гарнітура {(HeadphonesConnected ? "підключена" : "відключена")}"));
        }

        /// <summary>
        /// Відображення специфічної інформації про ноутбук (шаблон Template Method)
        /// </summary>
        protected override void DisplaySpecificInfo()
        {
            Console.WriteLine($"Сенсорний екран: {(IsTouchscreen ? "Так" : "Ні")}");
            Console.WriteLine($"Роздільна здатність: {DisplayResolution}");
            Console.WriteLine($"Док-станція: {(IsDockingStationConnected ? "Підключена" : "Відключена")}");
        }

        protected override void DisplayPeripheralConnections()
        {
            Console.WriteLine($"Гарнітура: {(HeadphonesConnected ? "Підключена" : "Відключена")}");
            Console.WriteLine($"Док-станція: {(IsDockingStationConnected ? "Підключена" : "Відключена")}");
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
