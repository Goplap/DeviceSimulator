using System;
using System.Collections.Generic;
using DeviceSimulation.EventArgs;
using DeviceSimulation;
using DeviceSimulation.Devices;
using lab_3.Strategy;
using DeviceSimulation.Patterns;

public class DeviceSimulator :
    IObserver<DeviceStateEventArgs>,
    IObserver<NetworkStateEventArgs>,
    IObserver<BatteryLevelEventArgs>
{
    private readonly IDeviceFactory _computerFactory;
    private readonly IDeviceFactory _laptopFactory;
    private readonly IDeviceFactory _smartphoneFactory;
    private IDevice? _currentDevice;
    private DeviceContext? _deviceContext;  // Додаємо DeviceContext
    private List<IDisposable> _subscriptions = new();

    public DeviceSimulator()
    {
        _computerFactory = new ComputerFactory();
        _laptopFactory = new LaptopFactory();
        _smartphoneFactory = new SmartphoneFactory();
    }

    public void Run()
    {
        while (true)
        {
            DisplayMainMenu();
            var choice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (!HandleMainMenuChoice(choice))
                break;
        }
    }

    private void DisplayMainMenu()
    {
        Console.WriteLine("\n=== СИМУЛЯТОР ПРИСТРОЇВ ===");
        Console.WriteLine("Виберіть пристрій:");
        Console.WriteLine("1. Комп'ютер");
        Console.WriteLine("2. Ноутбук");
        Console.WriteLine("3. Смартфон");
        Console.WriteLine("0. Вихід");

        // Відображаємо специфічні опції пристрою через DeviceContext
        _deviceContext?.DisplaySpecificOptions();

        Console.Write("Ваш вибір: ");
    }

    private bool HandleMainMenuChoice(char choice)
    {
        // Очистка підписок перед вибором нового пристрою
        _subscriptions.ForEach(s => s.Dispose());
        _subscriptions.Clear();

        switch (choice)
        {
            case '1':
                _currentDevice = _computerFactory.CreateDevice(DeviceType.Computer);
                break;
            case '2':
                _currentDevice = _laptopFactory.CreateDevice(DeviceType.Laptop);
                break;
            case '3':
                _currentDevice = _smartphoneFactory.CreateDevice(DeviceType.Smartphone);
                _deviceContext = new DeviceContext(new SmartphoneStrategy((Smartphone)_currentDevice));
                break;
            case '0':
                Console.WriteLine("Вихід із програми...");
                return false;
            default:
                Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                return true;
        }

        if (_currentDevice != null)
        {
            SubscribeToDeviceEvents();
            DeviceMenu();
        }

        return true;
    }

    private void SubscribeToDeviceEvents()
    {
        if (_currentDevice == null) return;

        var device = _currentDevice as BaseDevice;
        if (device != null)
        {
            _subscriptions.Add(device.Subscribe(this as IObserver<BatteryLevelEventArgs>));
            _subscriptions.Add(device.Subscribe(this as IObserver<NetworkStateEventArgs>));
            _subscriptions.Add(device.Subscribe(this as IObserver<DeviceStateEventArgs>));
        }
    }

    public void OnNext(BatteryLevelEventArgs value)
    {
        Console.WriteLine($"⚠️ ПОПЕРЕДЖЕННЯ: Рівень заряду {value.BatteryLevel}%! Підключіть зарядний пристрій.");
    }

    public void OnNext(BatteryInfo value)
    {
        string chargingStatus = value.IsCharging ? "🔌 Заряджається" : "⚡ Розряджається";
        Console.WriteLine($"⚠️ ПОПЕРЕДЖЕННЯ: Рівень заряду {value.Level}% | {chargingStatus}");
    }


    public void OnNext(NetworkStateEventArgs value)
    {
        Console.WriteLine(value.IsConnected ? "🌐 Мережа підключена." : "🚫 Мережа відключена.");
    }

    public void OnNext(DeviceStateEventArgs value)
    {
        Console.WriteLine($"ℹ️ Стан пристрою змінено: {value.Message}");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine($"Помилка: {error.Message}");
    }

    public void OnCompleted()
    {
        Console.WriteLine("Спостереження завершено.");
    }

    private void DeviceMenu()
    {
        if (_currentDevice == null) return;

        while (true)
        {
            DisplayDeviceMenu();
            var choice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (!HandleDeviceMenuChoice(choice))
                break;
        }
    }

    private void DisplayDeviceMenu()
    {
        if (_currentDevice == null) return;

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

        // Використовуємо DeviceContext для специфічних опцій пристрою
        _deviceContext?.DisplaySpecificOptions();

        Console.WriteLine("0. Повернутися");
        Console.Write("Ваш вибір: ");
    }

    private bool HandleDeviceMenuChoice(char choice)
    {
        if (_currentDevice == null) return false;

        // Спочатку перевіряємо, чи це специфічна опція пристрою через DeviceContext
        if (_deviceContext != null && _deviceContext.HandleSpecificOption(choice))
        {
            return true;
        }

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
                Console.WriteLine(hours > 0 ? $"Залишковий час автономної роботи: {hours:F1} год." : "Пристрій розряджений.");
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
                Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                break;
        }
        return true;
    }
}
