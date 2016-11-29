using System.Linq;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class CecilExtensions
    {
        public static bool IsCompilerGenerated(this ICustomAttributeProvider provider)
        {
            if (provider != null && provider.HasCustomAttributes)
            {
                return provider.CustomAttributes.Any(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute");
            }
            return false;
        }

        public static bool HasCompilerGeneratedAttribute(this ICustomAttributeProvider provider)
        {
            return provider.IsCompilerGenerated();
        }
    }
}
