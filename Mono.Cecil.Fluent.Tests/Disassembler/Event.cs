using System;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
	public class Disassembly_Event : TestsBase
	{
		static readonly string LF = Environment.NewLine;
		static readonly TypeDefinition TestType = CreateType();

		static readonly EventDefinition testevent = CreateEvent();

		It should_disassemble_event = () =>
			testevent
				.Disassemble()
				.Should().StartWith(".event [mscorlib]System.Object");

	}
}
