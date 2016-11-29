using System;

namespace Mono.Cecil.Fluent
{
	public static partial class FluentMethodBodyExtensions
	{
		public static FluentMethodBody WithParameter(this FluentMethodBody method, SystemTypeOrTypeReference paramType, string name = null)
		{
			var param = new ParameterDefinition(paramType.GetTypeReference(method.GetModule()));
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
