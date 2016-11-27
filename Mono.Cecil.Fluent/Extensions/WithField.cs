using System;

namespace Mono.Cecil.Fluent
{
	public static partial class IMemberDefinitionExtensions
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

		public static FieldDefinition WithField(this FieldDefinition member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null)
		{
			return WithFieldInternal(member, name, fieldType, attributes);
		}

		public static PropertyDefinition WithField(this PropertyDefinition member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null)
		{
			return WithFieldInternal(member, name, fieldType, attributes);
		}

		public static EventDefinition WithField(this EventDefinition member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null)
		{
			return WithFieldInternal(member, name, fieldType, attributes);
		}

		public static FluentMethodBody WithField(this MethodDefinition member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null)
		{
			return new FluentMethodBody(WithFieldInternal(member, name, fieldType, attributes));
		}

		public static FluentMethodBody WithField(this FluentMethodBody member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null)
		{
			return WithFieldInternal(member, name, fieldType, attributes);
		}

		public static TypeDefinition WithField<T>(this TypeDefinition member, string name, FieldAttributes? attributes = null)
		{
			return WithField(member, name, typeof(T), attributes);
		}

		public static FieldDefinition WithField<T>(this FieldDefinition member, string name, FieldAttributes? attributes = null)
        {
            return WithField(member, name, typeof(T), attributes);
        }

		public static PropertyDefinition WithField<T>(this PropertyDefinition member, string name, FieldAttributes? attributes = null)
        {
            return WithField(member, name, typeof(T), attributes);
        }

		public static EventDefinition WithField<T>(this EventDefinition member, string name, FieldAttributes? attributes = null)
        {
            return WithField(member, name, typeof(T), attributes);
        }

		public static FluentMethodBody WithField<T>(this MethodDefinition member, string name, FieldAttributes? attributes = null)
        {
            return WithField(member, name, typeof(T), attributes);
        }

		public static FluentMethodBody WithField<T>(this FluentMethodBody member, string name, FieldAttributes? attributes = null)
        {
            return WithField(member, name, typeof(T), attributes);
        }
	}
}
