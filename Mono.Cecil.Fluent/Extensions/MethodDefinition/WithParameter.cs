using System;

namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody WithParameter(this MethodDefinition method, SystemTypeOrTypeReference paramType, string name = null)
		{
			return new FluentMethodBody(method).WithParameter(paramType, name);
		}

		public static FluentMethodBody WithParameter<T>(this MethodDefinition method, string name = null)
		{
			return new FluentMethodBody(method).WithParameter<T>(name);
		}

		public static FluentMethodBody WithParameter(this MethodDefinition method, ParameterDefinition param)
		{
			return new FluentMethodBody(method).WithParameter(param);
		}
	}
}
