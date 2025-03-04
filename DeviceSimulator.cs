// DeviceSimulator.cs - Фасад для роботи з підсистемою симуляції пристроїв
using System;
using DeviceSimulation;
using DeviceSimulation.EventArgs;
using DeviceSimulation.Devices;

namespace lab1
{
    /// <summary>
    /// Клас DeviceSimulator реалізує шаблон проектування Фасад (Facade)
    /// Надає єдиний інтерфейс для взаємодії зі складною підсистемою симуляції пристроїв
    /// </summary>
    public class DeviceSimulator
    {
        // Шаблон Factory - використовується для створення пристроїв
        private readonly IDeviceFactory _deviceFactory;
        private IDevice? _currentDevice;

        public DeviceSimulator()
        {
            // Ініціалізуємо фабрику для створення пристроїв
            _deviceFactory = new DeviceFactory();
        }

        /// <summary>
        /// Головний метод запуску симулятора
        /// </summary>
        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                DisplayMainMenu();
                var choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (!HandleMainMenuChoice(choice))
                    break;
            }
        }

        /// <summary>
        /// Відображення головного меню вибору пристрою
        /// </summary>
        private void DisplayMainMenu()
        {
            Console.WriteLine("\n=== СИМУЛЯТОР ПРИСТРОЇВ ===");
            Console.WriteLine("Виберіть пристрій:");
            Console.WriteLine("1. Комп'ютер");
            Console.WriteLine("2. Ноутбук");
            Console.WriteLine("3. Смартфон");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");
        }

        /// <summary>
        /// Обробка вибору в головному меню
        /// </summary>
        private bool HandleMainMenuChoice(char choice)
        {
            switch (choice)
            {
                case '1':
                    // Використання шаблону Factory для створення пристрою
                    _currentDevice = _deviceFactory.CreateDevice(DeviceType.Computer);
                    DeviceMenu();
                    return true;
                case '2':
                    _currentDevice = _deviceFactory.CreateDevice(DeviceType.Laptop);
                    DeviceMenu();
                    return true;
                case '3':
                    _currentDevice = _deviceFactory.CreateDevice(DeviceType.Smartphone);
                    DeviceMenu();
                    return true;
                case '0':
                    Console.WriteLine("Вихід із програми...");
                    return false;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    return true;
            }
        }

        /// <summary>
        /// Меню для роботи з конкретним пристроєм
        /// </summary>
        private void DeviceMenu()
        {
            if (_currentDevice == null) return;

            // Шаблон Observer - підписуємося на події пристрою
            SubscribeToDeviceEvents();

            while (true)
            {
                DisplayDeviceMenu();
                var choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (!HandleDeviceMenuChoice(choice))
                    break;
            }

            // Шаблон Observer - відписуємося від подій пристрою
            UnsubscribeFromDeviceEvents();
        }

        /// <summary>
        /// Підписка на події пристрою (шаблон Observer)
        /// </summary>
        private void SubscribeToDeviceEvents()
        {
            if (_currentDevice == null) return;

            // Підписуємося на подію низького заряду батареї
            _currentDevice.BatteryLowEvent += OnBatteryLow;

            // Підписуємося на подію зміни стану мережі
            _currentDevice.NetworkStateChangedEvent += OnNetworkStateChanged;

            // Підписуємося на подію зміни стану пристрою
            _currentDevice.DeviceStateChangedEvent += OnDeviceStateChanged;
        }

        /// <summary>
        /// Відписка від подій пристрою (шаблон Observer)
        /// </summary>
        private void UnsubscribeFromDeviceEvents()
        {
            if (_currentDevice == null) return;

            // Відписуємося від події низького заряду батареї
            _currentDevice.BatteryLowEvent -= OnBatteryLow;

            // Відписуємося від події зміни стану мережі
            _currentDevice.NetworkStateChangedEvent -= OnNetworkStateChanged;

            // Відписуємося від події зміни стану пристрою
            _currentDevice.DeviceStateChangedEvent -= OnDeviceStateChanged;
        }

        /// <summary>
        /// Обробник події низького заряду батареї (шаблон Observer)
        /// </summary>
        private void OnBatteryLow(object? sender, BatteryLevelEventArgs e)
        {
            Console.WriteLine($"⚠️ ПОПЕРЕДЖЕННЯ: Рівень заряду {e.BatteryLevel}%! Підключіть зарядний пристрій.");
        }

        /// <summary>
        /// Обробник події зміни стану мережі (шаблон Observer)
        /// </summary>
        private void OnNetworkStateChanged(object? sender, NetworkStateEventArgs e)
        {
            Console.WriteLine(e.IsConnected
                ? "🌐 Мережа підключена."
                : "🚫 Мережа відключена.");
        }

        /// <summary>
        /// Обробник події зміни стану пристрою (шаблон Observer)
        /// </summary>
        private void OnDeviceStateChanged(object? sender, DeviceStateEventArgs e)
        {
            Console.WriteLine($"ℹ️ Стан пристрою змінено: {e.Message}");
        }

        /// <summary>
        /// Відображення меню пристрою
        /// </summary>
        private void DisplayDeviceMenu()
        {
            if (_currentDevice == null) return;

            // Показуємо загальну інформацію про пристрій
            Console.WriteLine($"\n=== {_currentDevice.Type} ===");
            Console.WriteLine($"Рівень заряду: {_currentDevice.BatteryLevel}%");
            Console.WriteLine($"Мережа: {(_currentDevice.NetworkConnected ? "Підключена" : "Відключена")}");

            Console.WriteLine("\nОберіть дію:");
            Console.WriteLine("1. Користуватися інтернетом");
            Console.WriteLine("2. Спілкуватися");
            Console.WriteLine("3. Слухати музику");
            Console.WriteLine("4. Дивитися відео");
            Console.WriteLine("5. Перемкнути мережу");
            Console.WriteLine("6. Встановити додаток");
            Console.WriteLine("7. Перевірити залишковий час автономної роботи");
            Console.WriteLine("8. Підключити гарнітуру");
            Console.WriteLine("9. Переглянути характеристики пристрою");

            // Шаблон Strategy - відображаємо специфічні операції для кожного типу пристрою
            _currentDevice.DisplaySpecificOptions();

            Console.WriteLine("0. Повернутися");
            Console.Write("Ваш вибір: ");
        }

        /// <summary>
        /// Обробка вибору в меню пристрою
        /// </summary>
        private bool HandleDeviceMenuChoice(char choice)
        {
            if (_currentDevice == null) return false;

            switch (choice)
            {
                case '1':
                    _currentDevice.UseInternet();
                    break;
                case '2':
                    _currentDevice.UseMessenger();
                    break;
                case '3':
                    _currentDevice.PlayMusic();
                    break;
                case '4':
                    _currentDevice.WatchVideo();
                    break;
                case '5':
                    // Використання властивості, що викликає подію (шаблон Observer)
                    _currentDevice.NetworkConnected = !_currentDevice.NetworkConnected;
                    break;
                case '6':
                    Console.Write("Введіть назву додатку: ");
                    string? software = Console.ReadLine();
                    if (!string.IsNullOrEmpty(software))
                    {
                        _currentDevice.InstallSoftware(software);
                    }
                    break;
                case '7':
                    double hours = _currentDevice.CalculateBatteryLife();
                    Console.WriteLine(hours > 0
                        ? $"Залишковий час автономної роботи: {hours:F1} год."
                        : "Пристрій розряджений.");
                    break;
                case '8':
                    _currentDevice.ToggleHeadphonesConnection();
                    break;
                case '9':
                    _currentDevice.DisplayDeviceInfo();
                    break;
                case '0':
                    return false;
                default:
                    // Шаблон Strategy - делегуємо обробку специфічних опцій конкретному пристрою
                    if (!_currentDevice.HandleSpecificOption(choice))
                    {
                        Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    }
                    break;
            }
            return true;
        }
    }
}