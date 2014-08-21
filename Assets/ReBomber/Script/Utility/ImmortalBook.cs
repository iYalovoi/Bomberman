using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Script
{
    public class ImmortalBook
    {
        private static readonly List<Type> Instance = new List<Type>();

        public static bool Add(MonoBehaviour self)
        {
            if (!Instance.Contains(self.GetType()))
            {
                Instance.Add(self.GetType());
                Object.DontDestroyOnLoad(self);
                return true;
            }
            self.gameObject.SetActive(false);
            Object.Destroy(self.gameObject);
            return false;
        }

        public static void Remove(MonoBehaviour self)
        {
            if (Instance.Contains(self.GetType()))
                Instance.Remove(self.GetType());
        }
    }
}