using Mono.Collections.Generic;

namespace Mono.Cecil.Fluent.FluentMethodBody
{
	public sealed class FluentMethodBody : IMemberDefinition
	{
		public readonly MethodDefinition MethodDefinition;
		public readonly ModuleDefinition Module;

		public string Name
		{
			get { return MethodDefinition.Name; }
			set { MethodDefinition.Name = value; }
		}

		public string FullName => MethodDefinition.FullName;

		public bool IsSpecialName
		{
			get { return MethodDefinition.IsSpecialName; }
			set { MethodDefinition.IsSpecialName = value; }
		}

		public bool IsRuntimeSpecialName
		{
			get { return MethodDefinition.IsRuntimeSpecialName; }
			set { MethodDefinition.IsRuntimeSpecialName = value; }
		}

		public TypeDefinition DeclaringType
		{
			get { return MethodDefinition.DeclaringType; }
			set { MethodDefinition.DeclaringType = value; }
		}

		public TypeReference ReturnType
		{
			get { return MethodDefinition.ReturnType; }
			set { MethodDefinition.ReturnType = value; }
		}

		public MetadataToken MetadataToken
		{
			get { return MethodDefinition.MetadataToken; }
			set { MethodDefinition.MetadataToken = value; }
		}

		public Collection<CustomAttribute> CustomAttributes => MethodDefinition.CustomAttributes;

		public bool HasCustomAttributes => MethodDefinition.HasCustomAttributes;

		internal FluentMethodBody(MethodDefinition methodDefinition)
		{
			MethodDefinition = methodDefinition;
			Module = methodDefinition.Module;
		}
	}
}
