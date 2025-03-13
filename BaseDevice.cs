// Add implementation for IObservable<DeviceStateEventArgs> in BaseDevice class
using System;
using System.Collections.Generic;
using System.Linq;
using DeviceSimulation.Components;
using DeviceSimulation.EventArgs;

namespace DeviceSimulation.Devices
{
    public abstract class BaseDevice : IDevice
    {
        // Event definitions for Observer pattern
        public event EventHandler<BatteryLevelEventArgs>? BatteryLowEvent;
        public event EventHandler<NetworkStateEventArgs>? NetworkStateChangedEvent;
        public event EventHandler<DeviceStateEventArgs>? DeviceStateChangedEvent;

        // Collections of observers for different event types
        private readonly List<IObserver<DeviceStateEventArgs>> _deviceStateObservers = new();
        private readonly List<IObserver<NetworkStateEventArgs>> _networkStateObservers = new();
        private readonly List<IObserver<BatteryLevelEventArgs>> _batteryLevelObservers = new();

        protected abstract void DisplayPeripheralConnections();
        protected abstract void DisplayProcessorInfo();
        protected abstract void DisplayMemoryInfo();
        protected abstract void DisplayInstalledSoftware();

        // Existing properties
        public DeviceType Type { get; protected set; }
        public int BatteryCapacity { get; protected set; }
        public bool HasPowerSupport { get; protected set; }
        public List<string> InstalledSoftware { get; protected set; }

        private bool _networkConnected;
        public bool NetworkConnected
        {
            get => _networkConnected;
            set
            {
                if (_networkConnected != value)
                {
                    _networkConnected = value;
                    OnNetworkStateChanged(new NetworkStateEventArgs(_networkConnected));
                }
            }
        }

        private int _batteryLevel;
        public int BatteryLevel
        {
            get => _batteryLevel;
            protected set
            {
                _batteryLevel = value;
                if (_batteryLevel < 20)
                {
                    OnBatteryLow(new BatteryLevelEventArgs(_batteryLevel));
                }
            }
        }

        public bool HeadphonesConnected { get; set; }
        public Processor DeviceProcessor { get; protected set; }
        public Memory DeviceMemory { get; protected set; }

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

        // Implementation of IObservable<DeviceStateEventArgs> interface
        public IDisposable Subscribe(IObserver<DeviceStateEventArgs> observer)
        {
            if (!_deviceStateObservers.Contains(observer))
            {
                _deviceStateObservers.Add(observer);
            }
            return new Unsubscriber<DeviceStateEventArgs>(_deviceStateObservers, observer);
        }

        // Additional Subscribe methods for other event types
        public IDisposable Subscribe(IObserver<NetworkStateEventArgs> observer)
        {
            if (!_networkStateObservers.Contains(observer))
            {
                _networkStateObservers.Add(observer);
            }
            return new Unsubscriber<NetworkStateEventArgs>(_networkStateObservers, observer);
        }

        public IDisposable Subscribe(IObserver<BatteryLevelEventArgs> observer)
        {
            if (!_batteryLevelObservers.Contains(observer))
            {
                _batteryLevelObservers.Add(observer);
            }
            return new Unsubscriber<BatteryLevelEventArgs>(_batteryLevelObservers, observer);
        }

        // Unsubscriber class for managing Observer pattern
        private class Unsubscriber<T> : IDisposable
        {
            private readonly List<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }

        public abstract void ToggleHeadphonesConnection();

        public virtual bool CanPerformAction(bool requireNetwork, string[] requiredSoftware, DeviceType? requiredDevice)
        {
            bool hasNetwork = !requireNetwork || NetworkConnected;
            bool hasSoftware = requiredSoftware == null || requiredSoftware.All(sw => InstalledSoftware.Contains(sw));
            bool isCompatibleDevice = !requiredDevice.HasValue || Type == requiredDevice.Value;

            return hasNetwork && hasSoftware && isCompatibleDevice;
        }

        public virtual void SimulateBatteryUsage(LoadIntensity intensity)
        {
            if (BatteryCapacity == 0)
            {
                Console.WriteLine("Пристрій працює без батареї.");
                return;
            }

            int usageRate = intensity switch
            {
                LoadIntensity.Low => 3,
                LoadIntensity.Medium => 8,
                LoadIntensity.High => 15,
                _ => 5
            };

            BatteryLevel = Math.Max(0, BatteryLevel - usageRate);
            Console.WriteLine($"Рівень заряду батареї: {BatteryLevel}%");
        }

        public virtual void InstallSoftware(string software)
        {
            if (!InstalledSoftware.Contains(software))
            {
                InstalledSoftware.Add(software);
                OnDeviceStateChanged(new DeviceStateEventArgs($"Додаток '{software}' встановлено"));
            }
            else
            {
                Console.WriteLine($"Додаток '{software}' вже встановлено.");
            }
        }

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

        public virtual void DisplayDeviceInfo()
        {
            Console.WriteLine("\n=== ІНФОРМАЦІЯ ПРО ПРИСТРІЙ ===");
            Console.WriteLine($"Тип пристрою: {Type}");
            Console.WriteLine($"Ємність батареї: {BatteryCapacity} мАг");
            Console.WriteLine($"Рівень заряду: {BatteryLevel}%");
            Console.WriteLine($"Підключення до мережі: {(NetworkConnected ? "Так" : "Ні")}");
            Console.WriteLine($"Гарнітура: {(HeadphonesConnected ? "Підключена" : "Відключена")}");

            // These methods will be implemented by derived classes
            DisplaySpecificInfo();
            DisplayPeripheralConnections();
            DisplayProcessorInfo();
            DisplayMemoryInfo();
            DisplayInstalledSoftware();
        }

        protected abstract void DisplaySpecificInfo();

        public virtual double CalculateBatteryLife()
        {
            if (BatteryCapacity == 0)
                return HasPowerSupport ? 0.5 : 0;

            if (BatteryCapacity >= 2000 && BatteryCapacity <= 3000)
                return BatteryLevel / 100.0 * (16 * 0.33 + 48 * 0.67);

            if (BatteryCapacity >= 5000 && BatteryCapacity <= 7000)
                return BatteryLevel / 100.0 * (4 * 0.33 + 12 * 0.67);

            return 0.5;
        }

        public virtual bool UseInternet()
        {
            if (CanPerformAction(true, new[] { "Браузер" }, null))
            {
                OnDeviceStateChanged(new DeviceStateEventArgs("Інтернет підключено"));
                SimulateProcessorLoad(LoadIntensity.Low);
                SimulateBatteryUsage(LoadIntensity.Low);
                return true;
            }
            Console.WriteLine("Неможливо користуватися інтернетом.");
            return false;
        }

        public virtual bool UseMessenger()
        {
            if (CanPerformAction(false, new[] { "Месенджер" }, null))
            {
                OnDeviceStateChanged(new DeviceStateEventArgs("Спілкування доступне"));
                SimulateBatteryUsage(LoadIntensity.Low);
                return true;
            }
            Console.WriteLine("Неможливо користуватися месенджером.");
            return false;
        }

        public virtual bool PlayMusic()
        {
            if (HeadphonesConnected)
            {
                OnDeviceStateChanged(new DeviceStateEventArgs("Музика грає у навушниках"));
                SimulateProcessorLoad(LoadIntensity.Medium);
                SimulateBatteryUsage(LoadIntensity.Medium);
                return true;
            }
            Console.WriteLine("Будь ласка, підключіть гарнітуру для прослуховування музики.");
            return false;
        }

        public virtual bool WatchVideo()
        {
            if (CanPerformAction(true, new[] { "Ютуб" }, null))
            {
                OnDeviceStateChanged(new DeviceStateEventArgs("Відео запущено"));
                SimulateProcessorLoad(LoadIntensity.Low);
                SimulateBatteryUsage(LoadIntensity.High);
                return true;
            }
            Console.WriteLine("Неможливо дивитися відео.");
            return false;
        }

        public abstract void DisplaySpecificOptions();

        public abstract bool HandleSpecificOption(char choice);

        protected virtual void OnBatteryLow(BatteryLevelEventArgs e)
        {
            BatteryLowEvent?.Invoke(this, e);

            // Notify all battery level observers
            foreach (var observer in _batteryLevelObservers)
            {
                observer.OnNext(e);
            }
        }

        protected virtual void OnNetworkStateChanged(NetworkStateEventArgs e)
        {
            NetworkStateChangedEvent?.Invoke(this, e);

            // Notify all network state observers
            foreach (var observer in _networkStateObservers)
            {
                observer.OnNext(e);
            }
        }

        protected virtual void OnDeviceStateChanged(DeviceStateEventArgs e)
        {
            DeviceStateChangedEvent?.Invoke(this, e);

            // Notify all device state observers
            foreach (var observer in _deviceStateObservers)
            {
                observer.OnNext(e);
            }
        }
    }
}