using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script
{
    public interface IDispatcher
    {
        void Dispatch(Action action);
    }

    public class UnityDispatcher : MonoBehaviour, IDispatcher
    {
        private readonly object _lockMe = new object();
        private readonly Queue<Action> _queue = new Queue<Action>();

        void Awake()
        {
            ImmortalBook.Add(this);
        }

        void Reset()
        {
            lock(_lockMe)
                _queue.Clear();
        }

        void Update()
        {
            Action action = null;
            lock (_lockMe)
                if (_queue.Any())
                    action = _queue.Dequeue();
            if (action != null)
                action();
        }

        public void Dispatch(Action action)
        {
            lock (_lockMe)
                _queue.Enqueue(action);
        }
    }
}