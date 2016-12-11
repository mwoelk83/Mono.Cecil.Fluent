
// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class TypeDefinitionExtensions
	{
		public static TypeDefinition WithField(this TypeDefinition member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null)
		{
			return member.WithFieldInternal(name, fieldType, attributes);
		}

		public static TypeDefinition WithField<T>(this TypeDefinition member, string name, FieldAttributes? attributes = null)
		{
			return WithField(member, name, typeof(T), attributes);
		}
	}
}
