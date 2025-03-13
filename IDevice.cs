using System;
using System.Collections.Generic;
using DeviceSimulation.Components;
using DeviceSimulation.EventArgs;

namespace DeviceSimulation
{
    // Extended IDevice interface to include IObservable support
    public interface IDevice : IObservable<DeviceStateEventArgs>
    {
        event EventHandler<BatteryLevelEventArgs> BatteryLowEvent;
        event EventHandler<NetworkStateEventArgs> NetworkStateChangedEvent;
        event EventHandler<DeviceStateEventArgs> DeviceStateChangedEvent;

        // Allow subscribing to different event types
        IDisposable Subscribe(IObserver<BatteryLevelEventArgs> observer);
        IDisposable Subscribe(IObserver<NetworkStateEventArgs> observer);

        DeviceType Type { get; }
        int BatteryLevel { get; }
        int BatteryCapacity { get; }
        bool NetworkConnected { get; set; }
        List<string> InstalledSoftware { get; }
        bool HeadphonesConnected { get; set; }
        Processor DeviceProcessor { get; }
        Memory DeviceMemory { get; }

        void SimulateProcessorLoad(LoadIntensity intensity);
        void SimulateMemoryUsage(LoadIntensity intensity);
        bool CanPerformAction(bool requireNetwork, string[] requiredSoftware, DeviceType? requiredDevice);
        void SimulateBatteryUsage(LoadIntensity intensity);
        void InstallSoftware(string software);
        void ToggleHeadphonesConnection();
        void DisplayDeviceInfo();
        double CalculateBatteryLife();
        bool UseInternet();
        bool UseMessenger();
        bool PlayMusic();
        bool WatchVideo();
        void DisplaySpecificOptions();
        bool HandleSpecificOption(char choice);
    }
}