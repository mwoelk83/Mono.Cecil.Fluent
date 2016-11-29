using Mono.Cecil.Fluent.Utils;

// ReSharper disable InconsistentNaming

namespace Mono.Cecil.Fluent
{
	public static partial class IMemberDefinitionExtensions
	{
		public static FluentMethodBody CreateMethod(this IMemberDefinition member, string name = null, SystemTypeOrTypeReference returnType = null, MethodAttributes? attributes = null)
		{
			var module = member.GetModule();
			var t = returnType != null ? returnType.GetTypeReference(member.GetModule()) : module.TypeSystem.Void;
			var method = new MethodDefinition(name ?? Generate.Name.ForMethod(), attributes ?? 0, t);

			var definition = member as TypeDefinition;
			if(definition != null) 
				definition.Resolve().Methods.Add(method);
			else
				member.DeclaringType.Methods.Add(method);

			return new FluentMethodBody(method);
		}

		public static FluentMethodBody CreateMethod<T>(this IMemberDefinition member, string name = null, MethodAttributes? attributes = null)
		{
			return CreateMethod(member, name, typeof(T), attributes);
		}

		public static FluentMethodBody CreateMethod(this IMemberDefinition member, SystemTypeOrTypeReference returnType, MethodAttributes? attributes = null)
		{
			return CreateMethod(member, null, returnType.GetTypeReference(member.GetModule()), attributes);
		}
	}
}
