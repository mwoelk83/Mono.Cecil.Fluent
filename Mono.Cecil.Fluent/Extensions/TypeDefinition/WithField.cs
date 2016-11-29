using System;

namespace Mono.Cecil.Fluent
{
	public static partial class TypeDefinitionExtensions
	{
		private static T WithFieldInternal<T>(this T member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null) where T : IMemberDefinition
		{
			if(name == null)
				throw new ArgumentNullException(nameof(name));
			if(fieldType == null)
				throw new ArgumentNullException(nameof(fieldType));
			
			var field = new FieldDefinition(name, attributes ?? FieldAttributes.FamORAssem | FieldAttributes.Public, fieldType.GetTypeReference(member.GetModule()));

			var definition = member as TypeDefinition;
			if (definition != null)
				definition.Resolve().Fields.Add(field);
			else
				member.DeclaringType.Fields.Add(field);

			return member;
		}

		public static TypeDefinition WithField(this TypeDefinition member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null)
		{
			return WithFieldInternal(member, name, fieldType, attributes);
		}

		public static TypeDefinition WithField<T>(this TypeDefinition member, string name, FieldAttributes? attributes = null)
		{
			return WithField(member, name, typeof(T), attributes);
		}
	}
}
