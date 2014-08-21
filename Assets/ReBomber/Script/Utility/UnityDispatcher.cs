using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script
{
    public interface IDispatcher
    {
        void Dispatch(Action action);
        void Dispatch(Func<IEnumerator> func);
    }

    public class UnityDispatcher : MonoBehaviour, IDispatcher
    {
        private readonly object _lockMe = new object();
        private readonly Queue<MulticastDelegate> _queue = new Queue<MulticastDelegate>();

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
            lock (_lockMe)
                if (_queue.Any())
                {
                    var action = _queue.Dequeue();
                    if (action is Action)
                        (action as Action).Invoke();
                    else StartCoroutine((action as Func<IEnumerator>).Invoke());
                }
        }

        public void Dispatch(Action action)
        {
            lock (_lockMe)
                _queue.Enqueue(action);
        }

        public void Dispatch(Func<IEnumerator> func)
        {
            lock(_lockMe)
                _queue.Enqueue(func);
        }
    }
}