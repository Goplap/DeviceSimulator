using System;
using System.Collections.Generic;
using DeviceSimulation.Components;
using DeviceSimulation.EventArgs;

namespace DeviceSimulation.Devices
{
    public class Smartphone : BaseDevice
    {
        public double ScreenSize { get; }
        public string CameraMP { get; }
        public bool IsBiometricEnabled { get; private set; }
        public bool IsBluetoothConnected { get; private set; }

        private readonly List<IObserver> observers = new();

        public Smartphone()
            : base(DeviceType.Smartphone, 3000,
                  new Processor("Snapdragon 888", 8, 2.4),
                  new Memory("LPDDR5", 6))
        {
            ScreenSize = 6.5;
            CameraMP = "48 MP";
            IsBiometricEnabled = true;
            HasPowerSupport = false;
            IsBluetoothConnected = true;

            InstallSoftware("Операційна система");
            InstallSoftware("Браузер");
            InstallSoftware("Месенджер");
            InstallSoftware("Ютуб");
            InstallSoftware("Камера");
        }

        public void Attach(IObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        private void NotifyObservers(string message)
        {
            foreach (var observer in observers)
            {
                observer.Update(message);
            }
        }

        // Implementing the required abstract methods
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

        public override void DisplaySpecificOptions()
        {
            Console.WriteLine("T. Увімкнути/вимкнути біометричну автентифікацію");
            Console.WriteLine("C. Зробити фотографію");
            Console.WriteLine("M. Увімкнути мобільні дані");
        }

        public override bool HandleSpecificOption(char choice)
        {
            switch (choice)
            {
                case 't':
                case 'T':
                    ToggleBiometricAuthentication();
                    return true;
                case 'c':
                case 'C':
                    TakePhoto();
                    return true;
                case 'm':
                case 'M':
                    ToggleMobileData();
                    return true;
                default:
                    return false;
            }
        }

        public void ToggleBiometricAuthentication()
        {
            IsBiometricEnabled = !IsBiometricEnabled;
            string message = $"Біометричну автентифікацію {(IsBiometricEnabled ? "увімкнено" : "вимкнено")}";
            NotifyObservers(message);
            Console.WriteLine(message);
        }

        public void TakePhoto()
        {
            if (CanPerformAction(false, new[] { "Камера" }, null))
            {
                SimulateProcessorLoad(LoadIntensity.Medium);
                SimulateBatteryUsage(LoadIntensity.Low);
                string message = $"Фото зроблено з роздільною здатністю {CameraMP}.";
                NotifyObservers(message);
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("Для фотографування потрібен додаток камери.");
            }
        }

        public void ToggleMobileData()
        {
            NetworkConnected = !NetworkConnected;
            SimulateBatteryUsage(LoadIntensity.Low);
            string message = NetworkConnected ?
                "Мобільні дані увімкнено. Доступ до інтернету через мобільну мережу." :
                "Мобільні дані вимкнено.";
            NotifyObservers(message);
            Console.WriteLine(message);
        }

        public override void ToggleHeadphonesConnection()
        {
            HeadphonesConnected = !HeadphonesConnected;
            string message = $"Гарнітура {(HeadphonesConnected ? "підключена" : "відключена")}";
            NotifyObservers(message);
            // Also notify through the BaseDevice's event system
            OnDeviceStateChanged(new DeviceStateEventArgs(message));
        }

        protected override void DisplaySpecificInfo()
        {
            Console.WriteLine($"Розмір екрану: {ScreenSize} дюймів");
            Console.WriteLine($"Камера: {CameraMP}");
            Console.WriteLine($"Біометрична автентифікація: {(IsBiometricEnabled ? "Увімкнена" : "Вимкнена")}");
        }

        protected override void DisplayPeripheralConnections()
        {
            Console.WriteLine($"Bluetooth: {(IsBluetoothConnected ? "Підключений" : "Не підключений")}");
        }
    }

    public interface IObserver
    {
        void Update(string message);
    }
}