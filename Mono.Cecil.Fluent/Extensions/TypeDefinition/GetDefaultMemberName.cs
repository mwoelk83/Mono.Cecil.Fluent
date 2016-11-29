
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class TyoeDefinitionExtensions
    {
        public static string GetDefaultMemberName(this TypeDefinition type)
        {
            CustomAttribute attr;
            return type.GetDefaultMemberName(out attr);
        }

        public static string GetDefaultMemberName(this TypeDefinition type, out CustomAttribute defaultMemberAttribute)
        {
            if (type.HasCustomAttributes)
                foreach (var ca in type.CustomAttributes)
                    if (ca.Constructor.DeclaringType.Name == "DefaultMemberAttribute" && ca.Constructor.DeclaringType.Namespace == "System.Reflection"
                        && ca.Constructor.FullName == @"System.Void System.Reflection.DefaultMemberAttribute::.ctor(System.String)")
                    {
                        defaultMemberAttribute = ca;
                        return ca.ConstructorArguments[0].Value as string;
                    }
            defaultMemberAttribute = null;
            return null;
        }
    }
}
