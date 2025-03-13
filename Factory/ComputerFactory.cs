using DeviceSimulation.Devices;
using DeviceSimulation;

public class ComputerFactory : IDeviceFactory
{
    public IDevice CreateDevice(DeviceType type)
    {
        if (type == DeviceType.Computer)
            return new Computer(); 
        throw new ArgumentException("Invalid device type");
    }
}
