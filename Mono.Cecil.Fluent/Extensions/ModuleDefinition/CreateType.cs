using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent
{
	public static partial class ModuleDefinitionExtensions
	{
		public static TypeDefinition CreateType(this ModuleDefinition that, string name = null, TypeAttributes? attributes = null)
		{
            // todo: create inherited type (enable specification of base class)

			if (string.IsNullOrEmpty(name))
				name = Generate.Name.ForClass();

			var type = new TypeDefinition("", name, attributes ?? TypeAttributes.Class);
			that.Types.Add(type);

			return type;
		}
	}
}
