using System;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class CecilExtensions
    {
        public static TypeReference MakeGeneric(this TypeReference self, params TypeReference[] arguments)
        {
            if (self.GenericParameters.Count != arguments.Length)
                throw new ArgumentException($"generic parameters count of type {self.Name} does not match given number of types");

            var instance = new GenericInstanceType(self);
            foreach (var argument in arguments)
                instance.GenericArguments.Add(argument);

            return instance;
        }
	}
}
