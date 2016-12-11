using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Cil;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Emit
{
    [TestClass]
    public class Emit : TestsBase
	{
		static FluentEmitter NewTestMethod => new FluentEmitter(CreateMethod());

        [TestMethod]
        public void emit_instruction () =>
			NewTestMethod
				.Emit(Instruction.Create(OpCodes.Nop))
			.Body.Instructions.Count.Should().Equal(1);

        [TestMethod]
        public void emit_opcode () =>
			NewTestMethod
				.Emit(OpCodes.Nop)
			.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Nop);

        [TestMethod]
        public void emit_opcode_ldstr () =>
			NewTestMethod
				.Emit(OpCodes.Ldstr, "teststring")
			.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Ldstr);

        [TestMethod]
        public void emit_opcode_ldc_sbyte () =>
			NewTestMethod
				.Emit(OpCodes.Ldc_I4_S, (sbyte)100)
			.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Ldc_I4_S);

        [TestMethod]
        public void emit_opcode_ldc_int () =>
			NewTestMethod
				.Emit(OpCodes.Ldc_I4, -100)
			.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Ldc_I4);

        [TestMethod]
        public void emit_opcode_ldc_long () =>
			NewTestMethod
				.Emit(OpCodes.Ldc_I8, -100000000000L)
			.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Ldc_I8);

        [TestMethod]
        public void emit_opcode_ldc_float () =>
			NewTestMethod
				.Emit(OpCodes.Ldc_R4, 0.001f)
			.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Ldc_R4);

        [TestMethod]
        public void emit_opcode_ldc_double () =>
			NewTestMethod
				.Emit(OpCodes.Ldc_R8, 0.0001d)
			.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Ldc_R8);

        [TestMethod]
        public void emit_opcode_box_system_type () =>
			NewTestMethod
				.LdNull() // suppress exception from stack validator
				.Emit(OpCodes.Box, typeof(Console))
			.Body.Instructions.Last().OpCode.Should().Equal(OpCodes.Box);

        [TestMethod]
        public void emit_opcode_box_system_typeinfo () =>
			NewTestMethod
				.LdNull() // suppress exception from stack validator
				.Emit(OpCodes.Box, typeof(Console).GetTypeInfo())
			.Body.Instructions.Last().OpCode.Should().Equal(OpCodes.Box);

        [TestMethod]
        public void emit_opcode_call_system_constructorinfo () =>
			NewTestMethod
				.LdNull() // suppress exception from stack validator
				.LdNull() // suppress exception from stack validator
				.LdNull() // suppress exception from stack validator
				.Emit(OpCodes.Call, typeof(Tuple<string, string>).GetConstructors().First())
			.Body.Instructions.Last().OpCode.Should().Equal(OpCodes.Call);

        [TestMethod]
        public void emit_opcode_call_system_methodinfo () =>
			NewTestMethod
				.Emit(OpCodes.Call, typeof(Console).GetMethods().First(m => m.GetParameters().Length == 0))
			.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Call);

        [TestMethod]
        public void emit_opcode_ldfld_system_fieldinfo () =>
			NewTestMethod
				.LdNull() // suppress exception from stack validator
				.Emit(OpCodes.Ldfld, typeof(Console).GetRuntimeFields().First())
			.Body.Instructions.Last().OpCode.Should().Equal(OpCodes.Ldfld);

        [TestMethod]
        public void emit_opcode_box_typereference () =>
			NewTestMethod
				.LdNull() // suppress exception from stack validator
				.Emit(OpCodes.Box, CreateType())
			.Body.Instructions.Last().OpCode.Should().Equal(OpCodes.Box);

        [TestMethod]
        public void emit_opcode_call_methodreference () =>
			NewTestMethod
				.LdNull() // suppress exception from stack validator
				.Emit(OpCodes.Call, CreateMethod())
			.Body.Instructions.Last().OpCode.Should().Equal(OpCodes.Call);

        [TestMethod]
        public void emit_opcode_ldfld_fieldreference () =>
			NewTestMethod
				.LdNull() // suppress exception from stack validator
				.Emit(OpCodes.Ldfld, CreateField())
			.Body.Instructions.Last().OpCode.Should().Equal(OpCodes.Ldfld);

        [TestMethod]
        public void emit_opcode_br_to_instruction_with_selector () =>
			NewTestMethod
				.Emit(OpCodes.Nop)
				.Emit(OpCodes.Br, (instructions) => instructions.First())
			.Body.Instructions[1].OpCode.Should().Equal(OpCodes.Br);
	}
}
