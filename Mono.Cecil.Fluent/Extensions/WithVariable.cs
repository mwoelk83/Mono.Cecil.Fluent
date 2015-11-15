using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody WithVariable(this MethodDefinition method, TypeReference varType, string name = null)
		{
			return new FluentMethodBody(method).WithVariable(varType, name);
		}

		public static FluentMethodBody WithVariable(this MethodDefinition method, Type varType, string name = null)
		{
			return new FluentMethodBody(method).WithVariable(varType, name);
		}

		public static FluentMethodBody WithVariable<T>(this MethodDefinition method, string name = null)
		{
			return new FluentMethodBody(method).WithVariable<T>(name);
		}
	}

	public static partial class FluentMethodBodyExtensions
	{
		public static FluentMethodBody WithVariable(this FluentMethodBody method, TypeReference varType, string name = null)
		{
			var var = new VariableDefinition(method.Module.SafeImport(varType));
			if (!string.IsNullOrEmpty(name))
				var.Name = name;
			method.Variables.Add(var);
			return method;
		}

		public static FluentMethodBody WithVariable(this FluentMethodBody method, Type varType, string name = null)
		{
			var var = new VariableDefinition(method.Module.SafeImport(varType));
			if (!string.IsNullOrEmpty(name))
				var.Name = name;
			method.Variables.Add(var);
			return method;
		}

		public static FluentMethodBody WithVariable<T>(this FluentMethodBody method, string name = null)
		{
			var var = new VariableDefinition(method.Module.SafeImport(typeof(T)));
			if (!string.IsNullOrEmpty(name))
				var.Name = name;
			method.Variables.Add(var);
			return method;
		}
	}
}
