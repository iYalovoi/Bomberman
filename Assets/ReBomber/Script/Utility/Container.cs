using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Script
{

    public enum Scope
    {
        Single,
        Transient,
    }

    public class Part
    {
        public Type Type { get; set; }
        public Scope Scope { get; set; }
    }

    //Should be turned to proper DI Container at some point
    public class Container
    {
        private readonly Dictionary<Type, object> _singleMap = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Part> _typeMap = new Dictionary<Type, Part>();

        public void Single<T, TI>()
        {
            Register<T, TI>(Scope.Single);
        }

        public void Transient<T, TI>()
        {
            Register<T, TI>(Scope.Transient);
        }

        private void Register<T, TI>(Scope scope)
        {
            _typeMap.Add(typeof(T), new Part
            {
                Scope = scope,
                Type = typeof(TI)
            });
        }

        public void Register<T, TI>(TI instance)
        {
            _singleMap.Add(typeof(T), instance);
        }

        public T Get<T>()
        {
            var targetType = typeof (T);
            return (T) Get(targetType);
        }

        public object Get(Type targetType)
        {
            if (_singleMap.ContainsKey(targetType))
                return _singleMap[targetType];
            if (_typeMap.ContainsKey(targetType))
            {
                var retVal = Produce(_typeMap[targetType].Type);
                if (_typeMap[targetType].Scope == Scope.Single)
                    _singleMap.Add(targetType, retVal);
                return retVal;
            }
            throw new InvalidOperationException(string.Format("Can't resolve type {0}", targetType.Name));
        }

        private object Produce(Type targetType)
        {
            var ctor = targetType.GetConstructors().First();
            var list = ctor.GetParameters().Select(info => Get(info.ParameterType)).ToArray();
            return ctor.Invoke(list);
        }
    }
}