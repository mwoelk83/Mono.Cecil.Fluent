using System;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
	public class Disassembly_Method : TestsBase
	{
		static readonly string LF = Environment.NewLine;
		static readonly TypeDefinition TestType = CreateType();
		static readonly MethodDefinition TestMethod = CreateMethod();

		It should_disassemble_method = () =>
			TestModule.SafeImport(typeof(Uri).GetMethod("GetComponents")).Resolve()
				.Disassemble()
				.Should().StartWith(
						".method public hidebysig " + LF +
						"	instance string GetComponents (" + LF +
						"		valuetype System.UriComponents components," + LF +
						"		valuetype System.UriFormat format" + LF +
						"	) cil managed " + LF +
						"{" + LF +
						"	.custom instance void __DynamicallyInvokableAttribute::.ctor() = (" + LF +
						"		01 00 00 00" + LF +
						"	)" + LF +
						"	// Code size 138 (0x8a)");

		It should_disassemble_fluentmethod = () =>
			TestMethod
				.ReturnsVoid()
				.Ret()
				.Disassemble()
				.Should().StartWith(".method family");
	}
}
