// Devices/Smartphone.cs - Конкретна реалізація смартфона
using System;
using DeviceSimulation.Components;
using DeviceSimulation.EventArgs;

namespace DeviceSimulation.Devices
{
    /// <summary>
    /// Клас Smartphone представляє смартфон
    /// Реалізує конкретну стратегію для пристрою типу "Смартфон" (шаблон Strategy)
    /// </summary>
    public class Smartphone : BaseDevice
    {
        public double ScreenSize { get; }
        public string CameraMP { get; }
        public bool IsBiometricEnabled { get; private set; }

        public Smartphone()
            : base(DeviceType.Smartphone, 3000,
                  new Processor("Snapdragon 888", 8, 2.4),
                  new Memory("LPDDR5", 6))
        {
            ScreenSize = 6.5;
            CameraMP = "48 MP";
            IsBiometricEnabled = true;
            HasPowerSupport = false;

            // Встановлення основного ПЗ
            InstallSoftware("Операційна система");
            InstallSoftware("Браузер");
            InstallSoftware("Месенджер");
            InstallSoftware("Ютуб");
            InstallSoftware("Камера");
        }

        /// <summary>
        /// Реалізація специфічних опцій для смартфона (шаблон Strategy)
        /// </summary>
        public override void DisplaySpecificOptions()
        {
            Console.WriteLine("T. Увімкнути/вимкнути біометричну автентифікацію");
            Console.WriteLine("C. Зробити фотографію");
            Console.WriteLine("M. Увімкнути мобільні дані");
        }

        /// <summary>
        /// Обробка специфічних опцій для смартфона (шаблон Strategy)
        /// </summary>
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

        /// <summary>
        /// Увімкнення/вимкнення біометричної автентифікації
        /// </summary>
        private void ToggleBiometricAuthentication()
        {
            IsBiometricEnabled = !IsBiometricEnabled;

            // Генеруємо подію зміни стану пристрою
            OnDeviceStateChanged(new DeviceStateEventArgs(
                $"Біометричну автентифікацію {(IsBiometricEnabled ? "увімкнено" : "вимкнено")}"));

            Console.WriteLine(IsBiometricEnabled ?
                "Біометричну автентифікацію увімкнено. Підвищений рівень безпеки." :
                "Біометричну автентифікацію вимкнено. Використовується PIN-код.");
        }

        /// <summary>
        /// Фотографування на камеру смартфона
        /// </summary>
        private void TakePhoto()
        {
            if (CanPerformAction(false, new[] { "Камера" }, null))
            {
                // Генеруємо подію зміни стану пристрою
                OnDeviceStateChanged(new DeviceStateEventArgs("Фото зроблено"));

                SimulateProcessorLoad(LoadIntensity.Medium);
                SimulateBatteryUsage(LoadIntensity.Low);
                Console.WriteLine($"Фото зроблено з роздільною здатністю {CameraMP}.");
            }
            else
            {
                Console.WriteLine("Для фотографування потрібен додаток камери.");
            }
        }

        /// <summary>
        /// Увімкнення/вимкнення мобільних даних
        /// </summary>
        private void ToggleMobileData()
        {
            NetworkConnected = !NetworkConnected;

            SimulateBatteryUsage(LoadIntensity.Low);
            Console.WriteLine(NetworkConnected ?
                "Мобільні дані увімкнено. Доступ до інтернету через мобільну мережу." :
                "Мобільні дані вимкнено.");
        }

        /// <summary>
        /// Відображення специфічної інформації про смартфон (шаблон Template Method)
        /// </summary>
        protected override void DisplaySpecificInfo()
        {
            Console.WriteLine($"Розмір екрану: {ScreenSize} дюймів");
            Console.WriteLine($"Камера: {CameraMP}");
            Console.WriteLine($"Біометрична автентифікація: {(IsBiometricEnabled ? "Увімкнена" : "Вимкнена")}");
        }
    }
}