using System;

namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static MethodDefinition ReturnsVoid(this MethodDefinition method)
		{
			method.ReturnType = method.Module.TypeSystem.Void;
		    return method;
		}

		public static MethodDefinition Returns(this MethodDefinition method, TypeReference type)
        {
            method.ReturnType = method.Module.SafeImport(type);
            return method;
        }

		public static MethodDefinition Returns(this MethodDefinition method, Type type)
        {
            method.ReturnType = method.Module.SafeImport(type);
            return method;
        }

		public static MethodDefinition Returns<T>(this MethodDefinition method)
        {
            method.ReturnType = method.Module.SafeImport(typeof(T));
            return method;
        }
	}
}
