
// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static MethodDefinition WithParameter(this MethodDefinition method, SystemTypeOrTypeReference paramType, string name = null)
        {
            var param = new ParameterDefinition(paramType.GetTypeReference(method.GetModule()));
            if (!string.IsNullOrEmpty(name))
                param.Name = name;
            method.Parameters.Add(param);
            return method;
        }

		public static MethodDefinition WithParameter<T>(this MethodDefinition method, string name = null)
        {
            var param = new ParameterDefinition(method.Module.SafeImport(typeof(T)));
            if (!string.IsNullOrEmpty(name))
                param.Name = name;
            method.Parameters.Add(param);
            return method;
        }

		public static MethodDefinition WithParameter(this MethodDefinition method, ParameterDefinition param)
        {
            method.Parameters.Add(param);
            return method;
        }
	}
}
