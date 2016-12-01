using System;
using Machine.Specifications;
using Mono.Cecil.Fluent.Utils;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
	public class Disassembly_Property : TestsBase
	{
		static readonly string LF = Environment.NewLine;
		static readonly TypeDefinition TestType = CreateType();
		
		static readonly PropertyDefinition testprop = CreateProperty();

		static PropertyDefinition CreateProperty()
		{
			var prop = new PropertyDefinition(Generate.Name.ForMethod(), PropertyAttributes.None, TestModule.TypeSystem.Boolean);
			TestType.Properties.Add(prop);
			prop.DeclaringType = TestType;
			prop.GetMethod = TestType.CreateMethod("get_" + prop.Name)
				.Returns<bool>()
                .AppendIL()
				    .Ldc(true)
				    .Ret()
				    .MethodDefinition;
			return prop;
		}

		It should_disassemble_property = () =>
			testprop
				.Disassemble()
				.Should().Equal($".property instance bool {testprop.Name}()" + LF +
								 "{" + LF +
          						$"	.get instance bool {TestType.Name}::get_{testprop.Name}$PST06000000()" + LF +
								 "}" + LF);

	}
}
