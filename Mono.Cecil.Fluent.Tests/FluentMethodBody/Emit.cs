using Machine.Specifications;
using Mono.Cecil.Cil;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.CecilExtensions
{
	public class FluentMethodBody_Emit : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static FluentMethodBody NewTestMethod => new FluentMethodBody(CreateMethod());

		It schould_emit_instruction = () =>
			NewTestMethod
				.Emit(Instruction.Create(OpCodes.Nop))
			.MethodDefinition
			.Body.Instructions.Count.Should().Equal(1);

	}
}
