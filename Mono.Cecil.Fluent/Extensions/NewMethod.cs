using System;
using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent
{
	public static partial class IMemberDefinitionExtensions
	{
		public static FluentMethodBody NewMethod(this IMemberDefinition member, string name = null, 
			TypeReference returnType = null, MethodAttributes? attributes = null)
		{
			var module = member.GetModule();
			returnType = returnType != null ? module.SafeImport(returnType) : module.TypeSystem.Void;
			var method = new MethodDefinition(name ?? Generate.Name.ForMethod(), attributes ?? 0, returnType ?? module.TypeSystem.Void);
			if(member is TypeDefinition) 
				((TypeDefinition)member).Resolve().Methods.Add(method);
			else
				member.DeclaringType.Methods.Add(method);
			return new FluentMethodBody(method);
		}

		public static FluentMethodBody NewMethod<T>(this IMemberDefinition member, string name = null,
			MethodAttributes? attributes = null)
		{
			return NewMethod(member, name, typeof(T), attributes);
		}

		public static FluentMethodBody NewMethod(this IMemberDefinition member, string name, Type returnType,
			MethodAttributes? attributes = null)
		{
			return NewMethod(member, name, member.GetModule().SafeImport(returnType), attributes);
		}

		public static FluentMethodBody NewMethod(this IMemberDefinition member, Type returnType, MethodAttributes? attributes = null)
		{
			return NewMethod(member, null, member.GetModule().SafeImport(returnType), attributes);
		}
	}
}
