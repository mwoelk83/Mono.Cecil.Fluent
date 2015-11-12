using System;
using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent
{
	public static class TypeDefinitionExtensions
	{
		public static MethodDefinition NewMethod(this TypeDefinition type, string name = null, 
			TypeReference returnType = null, MethodAttributes? attributes = null)
		{
			returnType = returnType != null ? type.Module.SafeImport(returnType) : type.Module.TypeSystem.Void;
			var method = new MethodDefinition(name ?? Generate.Name.ForMethod(), attributes ?? 0, returnType ?? type.Module.TypeSystem.Void);
			type.Methods.Add(method);
			return method;
		}

		public static MethodDefinition NewMethod<T>(this TypeDefinition type, string name = null,
			MethodAttributes? attributes = null)
		{
			return NewMethod(type, name, typeof(T), attributes);
		}

		public static MethodDefinition NewMethod(this TypeDefinition type, string name, Type returnType,
			MethodAttributes? attributes = null)
		{
			return NewMethod(type, name, type.Module.SafeImport(returnType), attributes);
		}

		public static MethodDefinition NewMethod(this TypeDefinition type, Type returnType, MethodAttributes? attributes = null)
		{
			return NewMethod(type, null, type.Module.SafeImport(returnType), attributes);
		}
	}
}
