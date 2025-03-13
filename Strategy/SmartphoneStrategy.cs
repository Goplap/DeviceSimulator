using DeviceSimulation.Devices;
using lab_3.Strategy;

public class SmartphoneStrategy : IDeviceStrategy
{
    private Smartphone _smartphone;

    public SmartphoneStrategy(Smartphone smartphone)
    {
        _smartphone = smartphone;
    }

    public void DisplaySpecificOptions()
    {
        Console.WriteLine("T. Увімкнути/вимкнути біометричну автентифікацію");
        Console.WriteLine("C. Зробити фотографію");
        Console.WriteLine("M. Увімкнути мобільні дані");
    }

    public bool HandleSpecificOption(char choice)
    {
        switch (choice)
        {
            case 't':
            case 'T':
                _smartphone.ToggleBiometricAuthentication();
                return true;
            case 'c':
            case 'C':
                _smartphone.TakePhoto();
                return true;
            case 'm':
            case 'M':
                _smartphone.ToggleMobileData();
                return true;
            default:
                return false;
        }
    }
}
