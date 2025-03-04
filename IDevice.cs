// Interfaces/IDevice.cs - Інтерфейс для всіх пристроїв
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DeviceSimulation.Components;
using DeviceSimulation.EventArgs;

namespace DeviceSimulation
{
    /// <summary>
    /// Інтерфейс IDevice визначає загальний контракт для всіх типів пристроїв
    /// </summary>
    public interface IDevice
    {
        // Шаблон Observer - визначення подій для спостереження за станом пристрою
        event EventHandler<BatteryLevelEventArgs> BatteryLowEvent;
        event EventHandler<NetworkStateEventArgs> NetworkStateChangedEvent;
        event EventHandler<DeviceStateEventArgs> DeviceStateChangedEvent;

        // Загальні властивості пристрою
        DeviceType Type { get; }
        int BatteryLevel { get; }
        int BatteryCapacity { get; }
        bool NetworkConnected { get; set; }
        List<string> InstalledSoftware { get; }
        bool HeadphonesConnected { get; set; }
        Processor DeviceProcessor { get; }
        Memory DeviceMemory { get; }

        // Загальні методи для роботи з пристроєм
        void SimulateProcessorLoad(LoadIntensity intensity);
        void SimulateMemoryUsage(LoadIntensity intensity);
        bool CanPerformAction(bool requireNetwork, string[] requiredSoftware, DeviceType? requiredDevice);
        void SimulateBatteryUsage(LoadIntensity intensity);
        void InstallSoftware(string software);
        void ToggleHeadphonesConnection();
        void DisplayDeviceInfo();
        double CalculateBatteryLife();

        // Методи для основних функцій пристрою
        bool UseInternet();
        bool UseMessenger();
        bool PlayMusic();
        bool WatchVideo();

        // Методи для шаблону Strategy
        void DisplaySpecificOptions();
        bool HandleSpecificOption(char choice);
    }
}