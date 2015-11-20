using System;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
	public class Disassembly_MethodBody : TestsBase
	{
		static readonly string LF = Environment.NewLine;

		It should_disassemble_body = () =>
			CreateStaticMethod()
			.Returns<int>()
				.WithVariable<int>()
				.Ldc(1)
				.Stloc(0)
				.Ldc(2)
				.Ldloc(0)
				.Add()
				.Ret()
			.DisassembleBody()
			.Should().Equal("// Code size 6 (0x6)"+ LF +
						    ".maxstack 2" + LF +
						    ".locals (" + LF + 
					        "\t[0] int32" + LF +
						    ")" + LF +
						    "" + LF +
						    "IL_0000: ldc.i4.1" + LF +
							"IL_0001: stloc.0" + LF +
							"IL_0002: ldc.i4.2" + LF +
							"IL_0003: ldloc.0" + LF +
							"IL_0004: add" + LF +
							"IL_0005: ret" + LF);

		It should_disassemble_big_method_body_with_branches = () =>
			TestModule.SafeImport(typeof (Uri).GetMethod("GetComponents")).Resolve()
				.DisassembleBody()
				.Should().Contain("IL_003d: ldarg.0" + LF +
								  "IL_003e: call instance bool System.Uri::get_IsNotAbsoluteUri()" + LF +
								  "IL_0043: brfalse.s IL_0065");

		It should_decompile_method_with_loop = () =>
			TestModule.SafeImport(typeof (string).GetMethod("Join", new[] {typeof (string), typeof (object[])})).Resolve()
				.DisassembleBody()
				.Should().Contain("\tIL_006f: conv.i4" + LF +
								  "\tIL_0070: blt.s IL_0047" + LF +
								  "// end loop");

		It should_disassemble_method_with_try_catch = () =>
			TestModule.SafeImport(typeof (decimal).GetMethod("ToInt16")).Resolve()
				.DisassembleBody()
				.Should().Contain("} // end .try" + LF +
								  "catch System.OverflowException" + LF +
								  "{");

		It should_disassemble_method_with_try_finally = () =>
			TestModule.SafeImport(typeof(Console).GetProperty("IsInputRedirected").GetMethod).Resolve()
				.DisassembleBody()
				.Should().Contain("} // end .try" + LF +
								  "finally" + LF +
								  "{");
	}
}
