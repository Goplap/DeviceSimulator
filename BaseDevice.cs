// Devices/BaseDevice.cs - Базовий клас для всіх пристроїв
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DeviceSimulation.Components;
using DeviceSimulation.EventArgs;

namespace DeviceSimulation.Devices
{
    /// <summary>
    /// Базовий клас для всіх пристроїв, реалізує загальну функціональність
    /// Використовує шаблон Template Method для загальної структури пристроїв
    /// </summary>
    public abstract class BaseDevice : IDevice
    {
        // Реалізація шаблону Observer - визначення подій
        public event EventHandler<BatteryLevelEventArgs>? BatteryLowEvent;
        public event EventHandler<NetworkStateEventArgs>? NetworkStateChangedEvent;
        public event EventHandler<DeviceStateEventArgs>? DeviceStateChangedEvent;

        // Загальні властивості пристрою
        public DeviceType Type { get; protected set; }
        public int BatteryCapacity { get; protected set; }
        public bool HasPowerSupport { get; protected set; }
        public List<string> InstalledSoftware { get; protected set; }

        // Властивість з подією (шаблон Observer)
        private bool _networkConnected;
        public bool NetworkConnected
        {
            get => _networkConnected;
            set
            {
                if (_networkConnected != value)
                {
                    _networkConnected = value;
                    // Генеруємо подію при зміні стану мережі
                    OnNetworkStateChanged(new NetworkStateEventArgs(_networkConnected));
                }
            }
        }

        // Загальні властивості стану
        private int _batteryLevel;
        public int BatteryLevel
        {
            get => _batteryLevel;
            protected set
            {
                _batteryLevel = value;
                // Перевіряємо умову для генерації події низького заряду
                if (_batteryLevel < 20)
                {
                    OnBatteryLow(new BatteryLevelEventArgs(_batteryLevel));
                }
            }
        }

        public bool HeadphonesConnected { get; set; }
        public Processor DeviceProcessor { get; protected set; }
        public Memory DeviceMemory { get; protected set; }

        /// <summary>
        /// Конструктор базового пристрою
        /// </summary>
        protected BaseDevice(DeviceType type, int batteryCapacity, Processor processor, Memory memory)
        {
            Type = type;
            BatteryCapacity = batteryCapacity;
            DeviceProcessor = processor ?? throw new ArgumentNullException(nameof(processor));
            DeviceMemory = memory ?? throw new ArgumentNullException(nameof(memory));
            InstalledSoftware = new List<string>();
            BatteryLevel = batteryCapacity > 0 ? 100 : 0;
            NetworkConnected = false;
            HeadphonesConnected = false;
        }

        /// <summary>
        /// Метод для підключення/відключення гарнітури
        /// </summary>
        public virtual void ToggleHeadphonesConnection()
        {
            HeadphonesConnected = !HeadphonesConnected;

            // Генеруємо подію зміни стану пристрою
            OnDeviceStateChanged(new DeviceStateEventArgs(
                $"Гарнітура {(HeadphonesConnected ? "підключена" : "відключена")}"));
        }

        /// <summary>
        /// Перевіряє, чи може пристрій виконати певну дію
        /// </summary>
        public virtual bool CanPerformAction(bool requireNetwork, string[] requiredSoftware, DeviceType? requiredDevice)
        {
            bool hasNetwork = !requireNetwork || NetworkConnected;
            bool hasSoftware = requiredSoftware == null || requiredSoftware.All(sw => InstalledSoftware.Contains(sw));
            bool isCompatibleDevice = !requiredDevice.HasValue || Type == requiredDevice.Value;

            return hasNetwork && hasSoftware && isCompatibleDevice;
        }

        /// <summary>
        /// Симулює використання батареї з різною інтенсивністю
        /// </summary>
        public virtual void SimulateBatteryUsage(LoadIntensity intensity)
        {
            if (BatteryCapacity == 0)
            {
                Console.WriteLine("Пристрій працює без батареї.");
                return;
            }

            // Визначаємо швидкість розряду залежно від інтенсивності
            int usageRate = intensity switch
            {
                LoadIntensity.Low => 3,
                LoadIntensity.Medium => 8,
                LoadIntensity.High => 15,
                _ => 5
            };

            // Оновлюємо рівень заряду (властивість з генерацією події)
            BatteryLevel = Math.Max(0, BatteryLevel - usageRate);

            Console.WriteLine($"Рівень заряду батареї: {BatteryLevel}%");
        }

        /// <summary>
        /// Встановлює програмне забезпечення на пристрій
        /// </summary>
        public virtual void InstallSoftware(string software)
        {
            if (!InstalledSoftware.Contains(software))
            {
                InstalledSoftware.Add(software);

                // Генеруємо подію зміни стану пристрою
                OnDeviceStateChanged(new DeviceStateEventArgs($"Додаток '{software}' встановлено"));
            }
            else
            {
                Console.WriteLine($"Додаток '{software}' вже встановлено.");
            }
        }

        /// <summary>
        /// Симулює навантаження на процесор з різною інтенсивністю
        /// </summary>
        public virtual void SimulateProcessorLoad(LoadIntensity intensity)
        {
            string load = intensity switch
            {
                LoadIntensity.Low => "Низьке навантаження на процесор.",
                LoadIntensity.High => "Високе навантаження на процесор.",
                _ => "Середнє навантаження на процесор."
            };
            Console.WriteLine(load);
        }

        /// <summary>
        /// Симулює використання пам'яті з різною інтенсивністю
        /// </summary>
        public virtual void SimulateMemoryUsage(LoadIntensity intensity)
        {
            string usage = intensity switch
            {
                LoadIntensity.Low => "Низьке використання пам'яті.",
                LoadIntensity.High => "Високе використання пам'яті.",
                _ => "Середнє використання пам'яті."
            };
            Console.WriteLine(usage);
        }

        /// <summary>
        /// Відображає інформацію про пристрій (шаблон Template Method)
        /// </summary>
        public virtual void DisplayDeviceInfo()
        {
            Console.WriteLine("\n=== ІНФОРМАЦІЯ ПРО ПРИСТРІЙ ===");
            Console.WriteLine($"Тип пристрою: {Type}");
            Console.WriteLine($"Ємність батареї: {BatteryCapacity} мАг");
            Console.WriteLine($"Рівень заряду: {BatteryLevel}%");
            Console.WriteLine($"Підключення до мережі: {(NetworkConnected ? "Так" : "Ні")}");
            Console.WriteLine($"Гарнітура: {(HeadphonesConnected ? "Підключена" : "Відключена")}");
            Console.WriteLine($"Процесор: {DeviceProcessor.Model}, {DeviceProcessor.Cores} ядер, {DeviceProcessor.ClockSpeedGHz} ГГц");
            Console.WriteLine($"Оперативна пам'ять: {DeviceMemory.Type}, {DeviceMemory.CapacityGB} ГБ");
            Console.WriteLine("Встановлене ПЗ: " + (InstalledSoftware.Any() ? string.Join(", ", InstalledSoftware) : "Немає"));

            // Цей метод буде перевизначено у підкласах для додавання специфічної інформації
            DisplaySpecificInfo();
        }

        /// <summary>
        /// Метод для відображення специфічної інформації про конкретний тип пристрою
        /// Буде перевизначений у кожному конкретному пристрої (шаблон Template Method)
        /// </summary>
        protected virtual void DisplaySpecificInfo() { }

        /// <summary>
        /// Розраховує залишковий час роботи від батареї
        /// </summary>
        public virtual double CalculateBatteryLife()
        {
            if (BatteryCapacity == 0)
                return HasPowerSupport ? 0.5 : 0;

            // Розрахунок залежить від ємності батареї
            if (BatteryCapacity >= 2000 && BatteryCapacity <= 3000)
                return BatteryLevel / 100.0 * (16 * 0.33 + 48 * 0.67);

            if (BatteryCapacity >= 5000 && BatteryCapacity <= 7000)
                return BatteryLevel / 100.0 * (4 * 0.33 + 12 * 0.67);

            return 0.5;
        }

        /// <summary>
        /// Використання інтернету (шаблон Template Method)
        /// </summary>
        public virtual bool UseInternet()
        {
            if (CanPerformAction(true, new[] { "Браузер" }, null))
            {
                // Генеруємо подію зміни стану пристрою
                OnDeviceStateChanged(new DeviceStateEventArgs("Інтернет підключено"));

                SimulateProcessorLoad(LoadIntensity.Low);
                SimulateBatteryUsage(LoadIntensity.Low);
                return true;
            }
            Console.WriteLine("Неможливо користуватися інтернетом.");
            return false;
        }

        /// <summary>
        /// Використання месенджера (шаблон Template Method)
        /// </summary>
        public virtual bool UseMessenger()
        {
            if (CanPerformAction(false, new[] { "Месенджер" }, null))
            {
                // Генеруємо подію зміни стану пристрою
                OnDeviceStateChanged(new DeviceStateEventArgs("Спілкування доступне"));

                SimulateBatteryUsage(LoadIntensity.Low);
                return true;
            }
            Console.WriteLine("Неможливо користуватися месенджером.");
            return false;
        }

        /// <summary>
        /// Відтворення музики (шаблон Template Method)
        /// </summary>
        public virtual bool PlayMusic()
        {
            if (HeadphonesConnected)
            {
                // Генеруємо подію зміни стану пристрою
                OnDeviceStateChanged(new DeviceStateEventArgs("Музика грає у навушниках"));

                SimulateProcessorLoad(LoadIntensity.Medium);
                SimulateBatteryUsage(LoadIntensity.Medium);
                return true;
            }
            Console.WriteLine("Будь ласка, підключіть гарнітуру для прослуховування музики.");
            return false;
        }

        /// <summary>
        /// Перегляд відео (шаблон Template Method)
        /// </summary>
        public virtual bool WatchVideo()
        {
            if (CanPerformAction(true, new[] { "Ютуб" }, null))
            {
                // Генеруємо подію зміни стану пристрою
                OnDeviceStateChanged(new DeviceStateEventArgs("Відео запущено"));

                SimulateProcessorLoad(LoadIntensity.Low);
                SimulateBatteryUsage(LoadIntensity.High);
                return true;
            }
            Console.WriteLine("Неможливо дивитися відео.");
            return false;
        }

        /// <summary>
        /// Метод шаблону Strategy для відображення специфічних опцій пристрою
        /// </summary>
        public abstract void DisplaySpecificOptions();

        /// <summary>
        /// Метод шаблону Strategy для обробки специфічних опцій пристрою
        /// </summary>
        public abstract bool HandleSpecificOption(char choice);

        // Методи для генерації подій (шаблон Observer)

        /// <summary>
        /// Метод для виклику події низького заряду батареї
        /// </summary>
        protected virtual void OnBatteryLow(BatteryLevelEventArgs e)
        {
            BatteryLowEvent?.Invoke(this, e);
        }

        /// <summary>
        /// Метод для виклику події зміни стану мережі
        /// </summary>
        protected virtual void OnNetworkStateChanged(NetworkStateEventArgs e)
        {
            NetworkStateChangedEvent?.Invoke(this, e);
        }

        /// <summary>
        /// Метод для виклику події зміни стану пристрою
        /// </summary>
        protected virtual void OnDeviceStateChanged(DeviceStateEventArgs e)
        {
            DeviceStateChangedEvent?.Invoke(this, e);
        }
    }
}