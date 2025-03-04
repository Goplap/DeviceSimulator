// Interfaces/IDeviceFactory.cs - Інтерфейс фабрики пристроїв
namespace DeviceSimulation
{
    /// <summary>
    /// Інтерфейс фабрики пристроїв (шаблон Factory Method)
    /// </summary>
    public interface IDeviceFactory
    {
        /// <summary>
        /// Створює пристрій вказаного типу
        /// </summary>
        IDevice CreateDevice(DeviceType type);
    }
}