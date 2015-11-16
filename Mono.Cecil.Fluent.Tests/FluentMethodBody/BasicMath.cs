using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.FluentMethodBody
{
	public class FluentMethodBody_BasicMath : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static Fluent.FluentMethodBody NewTestMethod => new Fluent.FluentMethodBody(CreateMethod());

		It should_return_remainder_of_ten_by_four = () =>
			CreateStaticMethod()
			.Returns<byte>()
				.Ldc((byte)10)
				.Rem((long)4)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal((byte)2);

		It should_return_remainder_of_twenty_by_seven = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc((ushort)20)
				.Ldc((short)7)
				.Rem()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(6);

		It should_return_remainder_of_twenty_by_seven_unsigned = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(20uL)
				.RemUn(7uL)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(6);

		It should_return_remainder_of_tenbillion_by_1303_UL = () =>
			CreateStaticMethod()
			.Returns<ulong>()
				.Ldc(10000000000UL)
				.Ldc(1303UL)
				.RemUn()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(109UL);

		It schould_add_3 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(0)
				.Ldc(3)
				.Add()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(3);

		It should_add_3_with_arg = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(0L)
				.Add(3uL)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(3);

		It schould_sub_3 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(0)
				.Ldc(3)
				.Sub()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(-3);

		It should_sub_3_with_arg = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(0UL)
				.Sub(3L)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(-3);

		It should_mul_by_3 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(5)
				.Ldc(3)
				.Mul()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(15);

		It should_mul_by_3_with_arg = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(5)
				.Mul(3UL)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(15);

		It should_div_by_3 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(15)
				.Ldc(3)
				.Div()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(5);

		It should_div_by_3_with_arg = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(15)
				.Div(3UL)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(5);

		It should_div_by_3_un = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(15)
				.Ldc(3)
				.DivUn()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(5);

		It should_div_by_3_with_arg_un = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(15)
				.DivUn(3UL)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(5);
	}
}
