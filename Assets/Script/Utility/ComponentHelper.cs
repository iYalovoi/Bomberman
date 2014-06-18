using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Utility
{
    public static class ComponentHelper
    {
        public static T GetInterface<T>(this GameObject inObj) where T : class
        {
            return inObj.GetComponents<Component>().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetInterfaces<T>(this GameObject inObj) where T : class
        {
            return inObj.GetComponents<Component>().OfType<T>();
        }
    }
}