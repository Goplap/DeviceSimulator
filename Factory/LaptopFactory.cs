using DeviceSimulation.Devices;
using DeviceSimulation;

public class LaptopFactory : IDeviceFactory
{
    public IDevice CreateDevice(DeviceType type)
    {
        if (type == DeviceType.Laptop)
            return new Laptop();
        throw new ArgumentException("Invalid device type");
    }
}
