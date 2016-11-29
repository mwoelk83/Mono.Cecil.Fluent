using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class FluentMethodBodyExtensions
	{
		public static FluentMethodBody WithVariable(this FluentMethodBody method, SystemTypeOrTypeReference varType, string name = null)
		{
			var var = new VariableDefinition(varType.GetTypeReference(method.GetModule()));
			if (!string.IsNullOrEmpty(name))
				var.Name = name;
			method.Variables.Add(var);
			return method;
		}

		public static FluentMethodBody WithVariable<T>(this FluentMethodBody method, string name = null)
		{
			return WithVariable(method, typeof(T), name);
		}

		public static FluentMethodBody WithVariable(this FluentMethodBody method, VariableDefinition var)
		{
			method.Variables.Add(var);
			return method;
		}
	}
}
