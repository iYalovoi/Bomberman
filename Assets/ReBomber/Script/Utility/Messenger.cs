using System;
using System.Collections.Generic;

namespace Assets.Script
{
    public class Messenger
    {
        private readonly Dictionary<string, List<Action>> _signals = new Dictionary<string, List<Action>>();

        public void Signal(string signal)
        {
            if (_signals.ContainsKey(signal))
                _signals[signal].ForEach(o => o());
        }

        public Action Subscribe(string signal, Action action)
        {
            if (_signals.ContainsKey(signal))
                _signals[signal].Add(action);
            else
                _signals.Add(signal, new List<Action> { action });
            return () => _signals[signal].Remove(action);
        }

        private readonly Dictionary<Type, List<MulticastDelegate>> _map = new Dictionary<Type, List<MulticastDelegate>>();

        public void Signal<T>(T signal)
        {
            var type = signal.GetType();
            if (_map.ContainsKey(type))
                _map[type].ForEach(o => ((Action<T>)o)(signal));
        }

        public Action Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (_map.ContainsKey(type))
                _map[type].Add(action);
            else
                _map.Add(type, new List<MulticastDelegate> { action });
            return () => _map[type].Remove(action);
        }
    }
}