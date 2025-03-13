using DeviceSimulation.Devices;
using DeviceSimulation;

public class DeviceFactory : IDeviceFactory
{
    public IDevice CreateDevice(DeviceType type)
    {
        // Шаблон Factory Method - створюємо конкретний пристрій залежно від типу
        return type switch
        {
            DeviceType.Computer => new Computer(),
            DeviceType.Laptop => new Laptop(),
            DeviceType.Smartphone => new Smartphone(),
            _ => throw new ArgumentException("Невідомий тип пристрою")
        };
    }
}
