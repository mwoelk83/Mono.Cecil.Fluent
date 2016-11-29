
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    public static partial class CecilExtensions
    {
        public static bool IsVoid(this TypeReference type)
        {
            while (type is OptionalModifierType || type is RequiredModifierType)
                type = ((TypeSpecification)type).ElementType;
            return type.MetadataType == MetadataType.Void;
        }
    }
}
