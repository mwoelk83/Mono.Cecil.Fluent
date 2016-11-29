using System;

namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody ReturnsVoid(this MethodDefinition method)
		{
			return new FluentMethodBody(method).ReturnsVoid();
		}

		public static FluentMethodBody Returns(this MethodDefinition method, TypeReference type)
		{
			return new FluentMethodBody(method).Returns(type);
		}

		public static FluentMethodBody Returns(this MethodDefinition method, Type type)
		{
			return new FluentMethodBody(method).Returns(type);
		}

		public static FluentMethodBody Returns<T>(this MethodDefinition method)
		{
			return new FluentMethodBody(method).Returns<T>();
		}
	}
}
