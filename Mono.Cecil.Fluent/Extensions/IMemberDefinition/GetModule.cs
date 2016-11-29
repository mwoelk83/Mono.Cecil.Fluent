
// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	static partial class IMemberDefinitionExtensions
	{
		public static ModuleDefinition GetModule(this IMemberDefinition member)
		{
		    var reference = member as TypeReference;
		    return reference != null ? reference/*.Resolve()*/.Module : member.DeclaringType.Module;
		}
	}
}
