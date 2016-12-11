using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent.Tests
{
	public class TestsBase
	{
		internal static readonly ModuleDefinition TestModule = ModuleDefinition.CreateModule(Generate.Name.ForClass(), ModuleKind.Dll);
	    private static readonly TypeDefinition Methodsholdertype = CreateType();

		internal static TypeDefinition CreateType()
		{
			var t = new TypeDefinition("", Generate.Name.ForClass(), TypeAttributes.Class);
			TestModule.Types.Add(t);
			return t;
		}

		internal static MethodDefinition CreateMethod()
		{
			var m = new MethodDefinition(Generate.Name.ForMethod(), MethodAttributes.Family, TestModule.TypeSystem.Void) { DeclaringType = Methodsholdertype };
			Methodsholdertype.Methods.Add(m);
			return m;
		}

		internal static MethodDefinition CreateStaticMethod()
		{
			var m = new MethodDefinition(Generate.Name.ForMethod(), MethodAttributes.Family | MethodAttributes.Static, TestModule.TypeSystem.Void) { DeclaringType = Methodsholdertype };
			Methodsholdertype.Methods.Add(m);
			return m;
		}

		internal static FieldDefinition CreateField()
		{
			var f = new FieldDefinition(Generate.Name.ForMethod(), FieldAttributes.Family, TestModule.TypeSystem.Object) { DeclaringType = Methodsholdertype };
			Methodsholdertype.Fields.Add(f);
			return f;
		}

		internal static EventDefinition CreateEvent()
		{
			var e = new EventDefinition(Generate.Name.ForMethod(), EventAttributes.None, TestModule.TypeSystem.Object);
			e.DeclaringType = Methodsholdertype;
			Methodsholdertype.Events.Add(e);
			return e;
		}
	}
}
