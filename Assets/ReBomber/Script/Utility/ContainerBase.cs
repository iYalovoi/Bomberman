using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Script
{
    //Why this class even exist?
    //Because Unity sucks in terms of extensibility
    public class ContainerBase : MonoBehaviour
    {
        protected virtual void Start()
        {
            var launcher = FindObjectOfType<ContainerLauncher>();
            var targetType = GetType();
            var field = launcher.GetType().GetField("_container", BindingFlags.NonPublic | BindingFlags.Instance);
            var container = field.GetValue(launcher) as Container;
            var onInjected = targetType.GetMethod("OnInjected", BindingFlags.NonPublic | BindingFlags.Instance);
            if(onInjected != null)
            {
                var list = onInjected.GetParameters().Select(info => container.Get(info.ParameterType)).ToArray();
                onInjected.Invoke(this, list);
            }
        }
    }
}