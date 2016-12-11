using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
    [TestClass]
    public class Disassembly_Event : TestsBase
	{
		static readonly EventDefinition testevent = CreateEvent();
        
        [TestMethod]
        public void disassemble_event () =>
			testevent
				.Disassemble()
				.Should().StartWith(".event [mscorlib]System.Object");

	}
}
