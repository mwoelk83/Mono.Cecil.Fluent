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

		internal static MethodDefinition CreateMethod()
		{
			return new MethodDefinition(Generate.Name.ForMethod(), MethodAttributes.Family, TestModule.TypeSystem.Void) { DeclaringType = CreateType() };
		}

		internal static FieldDefinition CreateField()
		{
			return new FieldDefinition(Generate.Name.ForMethod(), FieldAttributes.Family, TestModule.TypeSystem.Object) { DeclaringType = CreateType() };
		}
	}
}
