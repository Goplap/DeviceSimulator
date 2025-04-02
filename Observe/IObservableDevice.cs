using System;
using System.Collections.Generic;

namespace DeviceSimulation.Patterns
{
    // Структура для передачі інформації про стан батареї
    public struct BatteryInfo
    {
        public int Level { get; }
        public bool IsCharging { get; }

        public BatteryInfo(int level, bool isCharging)
        {
            Level = level;
            IsCharging = isCharging;
        }
    }

    // Інтерфейс для спостережуваного об'єкта (Observable)
    public interface IObservableDevice
    {
        IDisposable Subscribe(IObserver<BatteryInfo> observer);
        void NotifyBatteryLevelChanged(BatteryInfo batteryInfo);
    }

    // Клас, який сповіщає спостерігачів про зміну рівня заряду
    public class BatteryNotifier : IObservableDevice
    {
        // Клас для відписки від спостережуваного об'єкта
        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<BatteryInfo>> _observers;
            private readonly IObserver<BatteryInfo> _observer;

            public Unsubscriber(List<IObserver<BatteryInfo>> observers, IObserver<BatteryInfo> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }

        private readonly List<IObserver<BatteryInfo>> _observers = new();

        public IDisposable Subscribe(IObserver<BatteryInfo> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }

        public void NotifyBatteryLevelChanged(BatteryInfo batteryInfo)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(batteryInfo);
            }
        }
    }

    // Спостерігач (Observer), який реагує на зміну рівня заряду
    public class BatteryStatusDisplay : IObserver<BatteryInfo>
    {
        public void OnNext(BatteryInfo batteryInfo)
        {
            string chargingStatus = batteryInfo.IsCharging ? "🔌 Заряджається" : "⚡ Розряджається";
            Console.WriteLine($"🔋 Рівень заряду: {batteryInfo.Level}% | {chargingStatus}");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"❌ Помилка: {error.Message}");
        }

        public void OnCompleted()
        {
            Console.WriteLine("✅ Моніторинг заряду завершено.");
        }
    }
}
