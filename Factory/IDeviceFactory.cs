using DeviceSimulation;

public interface IDeviceFactory
{
    IDevice CreateDevice(DeviceType type);
}
