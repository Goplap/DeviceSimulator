using System;
using System.Collections.Generic;

namespace DeviceSimulation.Patterns
{
    // Інтерфейс для спостережуваного об'єкта (Observable)
    public interface IObservableDevice
    {
        IDisposable Subscribe(IObserver<int> observer);
        void NotifyBatteryLevelChanged(int batteryLevel);
    }

    // Клас, який сповіщає спостерігачів про зміну рівня заряду
    public class BatteryNotifier : IObservableDevice
    {
        private readonly List<IObserver<int>> _observers = new();

        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }

        public void NotifyBatteryLevelChanged(int batteryLevel)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(batteryLevel);
            }
        }
    }

    // Клас для відписки від спостережуваного об'єкта
    public class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<int>> _observers;
        private readonly IObserver<int> _observer;

        public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
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

    // Спостерігач (Observer), який реагує на зміну рівня заряду
    public class BatteryStatusDisplay : IObserver<int>
    {
        public void OnNext(int batteryLevel)
        {
            Console.WriteLine($"🔋 Рівень заряду: {batteryLevel}%");
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
