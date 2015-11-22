using System;

namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody WithParameter(this MethodDefinition method, TypeReference paramType, string name = null)
		{
			return new FluentMethodBody(method).WithParameter(paramType, name);
		}

		public static FluentMethodBody WithParameter(this MethodDefinition method, Type paramType, string name = null)
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

	public static partial class FluentMethodBodyExtensions
	{
		public static FluentMethodBody WithParameter(this FluentMethodBody method, TypeReference paramType, string name = null)
		{
			var param = new ParameterDefinition(method.Module.SafeImport(paramType));
			if (!string.IsNullOrEmpty(name))
				param.Name = name;
			method.Parameters.Add(param);
			return method;
		}

		public static FluentMethodBody WithParameter(this FluentMethodBody method, Type paramType, string name = null)
		{
			var param = new ParameterDefinition(method.Module.SafeImport(paramType));
			if (!string.IsNullOrEmpty(name))
				param.Name = name;
			method.Parameters.Add(param);
			return method;
		}

		public static FluentMethodBody WithParameter<T>(this FluentMethodBody method, string name = null)
		{
			var param = new ParameterDefinition(method.Module.SafeImport(typeof(T)));
			if (!string.IsNullOrEmpty(name))
				param.Name = name;
			method.Parameters.Add(param);
			return method;
		}

		public static FluentMethodBody WithParameter(this FluentMethodBody method, ParameterDefinition param)
		{
			method.Parameters.Add(param);
			return method;
		}
	}
}
