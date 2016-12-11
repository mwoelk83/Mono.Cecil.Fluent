using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
    [TestClass]
    public class Disassembly_Field : TestsBase
	{
		static readonly string LF = Environment.NewLine;

		static readonly FieldDefinition testfield = CreateField();

        [TestMethod]
        public void disassemble_field () =>
			testfield
				.Disassemble()
				.Should().Equal($".field family object {testfield.Name}" + LF);

	}
}
