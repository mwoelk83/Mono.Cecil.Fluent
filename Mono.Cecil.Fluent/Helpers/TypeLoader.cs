using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    public class TypeLoader
    {
        private readonly object _syncRoot = new object();
        private readonly IDictionary<string, Type> _typesByName = new Dictionary<string, Type>();

        public static TypeLoader Instance { get; private set; }

        static TypeLoader()
        {
            Instance = new TypeLoader();
        }

        /// <summary>
        /// Get the System.Type from a TypeReference. Type must exist in current AppDomain.
        /// </summary>
        public Type Load(TypeReference typeReference)
        {
            // from SO
            //Type.GetType(tr.FullName + ", " + tr.Module.Assembly.FullName);
            // will look up in all assemnblies loaded into the current appDomain and fire the AppDomain.Resolve event if no Type could be found
            var fullName = typeReference.FullName.Replace('/', '+');
            Type type;

            lock (_syncRoot)
                if (_typesByName.TryGetValue(fullName, out type))
                    return type;

            type = AppDomain.CurrentDomain.GetAssemblies()
                .Select(asm => asm.GetType(fullName))
                .FirstOrDefault(t => t != null);

            lock (_syncRoot)
                if (!_typesByName.ContainsKey(fullName))
                    _typesByName[fullName] = type;

            return type;
        }

        public FieldInfo Load(FieldReference fieldReference)
        {
            return Load(fieldReference.DeclaringType)?
                .GetField(fieldReference.Name);
        }

        public MethodInfo Load(MethodReference methodReference)
        {
            Type[] paramTypes = methodReference.Parameters.Select(p => Load(p.ParameterType)).ToArray();
            if (paramTypes.Any(t => t == null))
                return null;

            return Load(methodReference.DeclaringType)?
                .GetMethod(methodReference.Name, paramTypes);
        }

        public ConstructorInfo LoadConstructor(MethodReference methodReference)
        {
            if (methodReference.Name != ".ctor")
                throw new Exception($"the method {methodReference.FullName} is not a constructor");

            Type[] paramTypes = methodReference.Parameters.Select(p => Load(p.ParameterType)).ToArray();
            if (paramTypes.Any(t => t == null))
                return null;

            return Load(methodReference.DeclaringType)?
                .GetConstructor(paramTypes);
        }
    }
}