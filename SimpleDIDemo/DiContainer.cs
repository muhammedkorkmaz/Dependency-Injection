using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleDIDemo
{
    public class DiContainer
    {
        private Dictionary<Type, Type> _types = new Dictionary<Type, Type>();

        public void Register<TInterface, TType>()
        {
            if (_types.ContainsKey(typeof(TInterface)))
            {
                throw new Exception("Type already registere");
            }

            _types.Add(typeof(TInterface), typeof(TType));
        }


        public TInterface Resolve<TInterface>()
        {
            return (TInterface)(GetImplementation(typeof(TInterface)));
        }

        private object GetImplementation(Type type)
        {
            if (!_types.ContainsKey(type))
            {
                throw new Exception("Type does not exist");
            }

            Type implementation = _types.GetValueOrDefault(type);

            ConstructorInfo constructorInfo = implementation.GetConstructors()[0];
            var constructureParamTypes = constructorInfo.GetParameters();
            List<object> constructorParamsImplementations = new(constructureParamTypes.Length);

            foreach (var param in constructureParamTypes)
            {
                constructorParamsImplementations.Add(GetImplementation(param.ParameterType));
            }

            return constructorInfo.Invoke(constructorParamsImplementations.ToArray());
        }
    }
}
