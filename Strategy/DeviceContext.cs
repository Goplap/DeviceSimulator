using lab_3.Strategy;

public class DeviceContext
{
    private IDeviceStrategy _strategy;

    public DeviceContext(IDeviceStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IDeviceStrategy strategy)
    {
        _strategy = strategy;
    }

    public void DisplaySpecificOptions()
    {
        _strategy.DisplaySpecificOptions();
    }

    public bool HandleSpecificOption(char choice)
    {
        return _strategy.HandleSpecificOption(choice);
    }
}
