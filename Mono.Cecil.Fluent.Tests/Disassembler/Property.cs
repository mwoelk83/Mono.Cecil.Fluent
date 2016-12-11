using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Fluent.Utils;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
    [TestClass]
    public class Disassembly_Property : TestsBase
	{
		static readonly string LF = Environment.NewLine;

		static readonly TypeDefinition TestType = CreateType();
		static readonly PropertyDefinition testprop = CreateProperty();

        [TestMethod]
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

        [TestMethod]
        public void disassemble_property () =>
			testprop
				.Disassemble()
				.Should().Equal($".property instance bool {testprop.Name}()" + LF +
								 "{" + LF +
          						$"	.get instance bool {TestType.Name}::get_{testprop.Name}$PST06000000()" + LF +
								 "}" + LF);

	}
}
