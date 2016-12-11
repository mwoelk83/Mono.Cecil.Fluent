using System;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class IMemberDefinitionExtensions
	{
		internal static T WithFieldInternal<T>(this T member, string name, SystemTypeOrTypeReference fieldType, FieldAttributes? attributes = null) 
            where T : IMemberDefinition
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
	}
}
