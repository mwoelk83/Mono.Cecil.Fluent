using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.FluentMethodBody
{
	public class FluentMethodBody_Basics : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static Fluent.FluentMethodBody NewTestMethod => new Fluent.FluentMethodBody(CreateMethod());

		It shoud_dup_value = () =>
			CreateStaticMethod()
				.Returns<int>()
				.Ldc(10)
				.Dup()
				.Add()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(20);

		It shoud_add_nops = () =>
			CreateStaticMethod()
				.ReturnsVoid()
				.Nop()
				.Nop()
				.Nop()
			.Body.Instructions.Count.Should().Equal(3);

		It shoud_pop_value = () =>
			CreateStaticMethod()
				.ReturnsVoid()
				.Ldc(0)
				.Pop()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null,null);
	}
}
