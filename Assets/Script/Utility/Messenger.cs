using System;
using System.Collections.Generic;

namespace Assets.Script.Utility
{
    public class Messenger
    {
        private readonly Dictionary<string, List<Action>> _map = new Dictionary<string, List<Action>>();

        public void Signal(string signal)
        {
            if (_map.ContainsKey(signal))
                _map[signal].ForEach(o => o());
        }
         
        public Action Subscribe(string signal, Action action)
        {
            if (_map.ContainsKey(signal))
                _map[signal].Add(action);
            else _map.Add(signal, new List<Action> { action });
            return () => _map[signal].Remove(action);
        }
    }
}