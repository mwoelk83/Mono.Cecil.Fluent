using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static MethodDefinition WithVariable(this MethodDefinition method, SystemTypeOrTypeReference varType, string name = null)
        {
            var var = new VariableDefinition(varType.GetTypeReference(method.GetModule()));
            if (!string.IsNullOrEmpty(name))
                var.Name = name;
            method.Body.Variables.Add(var);
            return method;
        }

		public static MethodDefinition WithVariable<T>(this MethodDefinition method, string name = null)
		{
            return WithVariable(method, typeof(T), name);
		}

		public static MethodDefinition WithVariable(this MethodDefinition method, VariableDefinition var)
		{
            method.Body.Variables.Add(var);
		    return method;
		}
	}
}
