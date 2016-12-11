using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
    [TestClass]
    public class Disassembly_Method : TestsBase
	{
		static readonly string LF = Environment.NewLine;
		static readonly MethodDefinition TestMethod = CreateMethod();

        [TestMethod]
        public void disassemble_method () =>
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

        [TestMethod]
        public void disassemble_fluentmethod () =>
			TestMethod
				.ReturnsVoid()
                .AppendIL()
                    .Ret()
                .EndEmitting()
				.Disassemble()
				.Should().StartWith(".method family");
	}
}
