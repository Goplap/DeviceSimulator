using DeviceSimulation.Devices;
using DeviceSimulation;

public class SmartphoneFactory : DeviceFactory
{
    public IDevice CreateDevice(DeviceType type)
    {
        if (type == DeviceType.Smartphone)
            return new Smartphone();
        throw new ArgumentException("Invalid device type");
    }
}
