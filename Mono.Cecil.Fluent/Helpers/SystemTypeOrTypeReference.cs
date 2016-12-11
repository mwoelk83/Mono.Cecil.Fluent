using System;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
    public sealed class SystemTypeOrTypeReference
    {
        private readonly object _type;

        internal SystemTypeOrTypeReference(Type t)
        {
            if (t == null)
                throw new ArgumentNullException();
            _type = t;
        }

        internal SystemTypeOrTypeReference(TypeReference t)
        {
            if(t == null)
                throw new ArgumentNullException();
            _type = t;
        }

        internal TypeReference GetTypeReference(ModuleDefinition module)
        {
            var ret = _type as Type;
            return ret != null 
                ? module.SafeImport(ret) 
                : module.SafeImport(_type as TypeReference);
        }

        public static implicit operator SystemTypeOrTypeReference(Type t) { return new SystemTypeOrTypeReference(t); }
        public static implicit operator SystemTypeOrTypeReference(TypeReference t) { return new SystemTypeOrTypeReference(t); }
    }
}
