using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent.Tests
{
	public class TestsBase
	{
		internal static readonly ModuleDefinition TestModule = ModuleDefinition.CreateModule(Generate.Name.ForClass(), ModuleKind.Dll);

		internal static TypeDefinition CreateType()
		{
			var t = new TypeDefinition("", Generate.Name.ForClass(), TypeAttributes.Class);
			TestModule.Types.Add(t);
			return t;
		}
	}
}
