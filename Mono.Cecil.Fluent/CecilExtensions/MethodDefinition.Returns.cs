using System;

namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody ReturnsVoid(this MethodDefinition method)
		{
			method.ReturnType = method.Module.TypeSystem.Void;
			return new FluentMethodBody(method);
		}

		public static FluentMethodBody Returns(this MethodDefinition method, TypeReference type)
		{
			method.ReturnType = method.Module.SafeImport(type);
			return new FluentMethodBody(method);
		}

		public static FluentMethodBody Returns(this MethodDefinition method, Type type)
		{
			method.ReturnType = method.Module.SafeImport(type);
			return new FluentMethodBody(method);
		}

		public static FluentMethodBody Returns<T>(this MethodDefinition method)
		{
			method.ReturnType = method.Module.SafeImport(typeof(T));
			return new FluentMethodBody(method);
		}
	}
}
