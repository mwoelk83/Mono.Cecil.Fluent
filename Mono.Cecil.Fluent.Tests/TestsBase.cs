using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent.Tests
{
	public class TestsBase
	{
		internal static readonly ModuleDefinition TestModule = ModuleDefinition.CreateModule(Generate.Name.ForClass(), ModuleKind.Dll);
		internal static TypeDefinition methodsholdertype = CreateType();

		internal static TypeDefinition CreateType()
		{
			var t = new TypeDefinition("", Generate.Name.ForClass(), TypeAttributes.Class);
			TestModule.Types.Add(t);
			return t;
		}

		internal static MethodDefinition CreateMethod()
		{
			var m = new MethodDefinition(Generate.Name.ForMethod(), MethodAttributes.Family, TestModule.TypeSystem.Void) { DeclaringType = methodsholdertype };
			methodsholdertype.Methods.Add(m);
			return m;
		}

		internal static MethodDefinition CreateStaticMethod()
		{
			var m = new MethodDefinition(Generate.Name.ForMethod(), MethodAttributes.Family | MethodAttributes.Static, TestModule.TypeSystem.Void) { DeclaringType = methodsholdertype };
			methodsholdertype.Methods.Add(m);
			return m;
		}

		internal static FieldDefinition CreateField()
		{
			var f = new FieldDefinition(Generate.Name.ForMethod(), FieldAttributes.Family, TestModule.TypeSystem.Object) { DeclaringType = methodsholdertype };
			methodsholdertype.Fields.Add(f);
			return f;
		}
	}
}
