using System;
using Machine.Specifications;
using Mono.Cecil.Fluent.Utils;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
	public class Disassembly_Field : TestsBase
	{
		static readonly string LF = Environment.NewLine;
		static readonly TypeDefinition TestType = CreateType();

		static readonly FieldDefinition testfield = CreateField();

		It should_disassemble_field = () =>
			testfield
				.Disassemble()
				.Should().Equal($".field family object {testfield.Name}" + LF);

	}
}
