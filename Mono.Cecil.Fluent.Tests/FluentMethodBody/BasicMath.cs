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
				.Rem(4)
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
				.Ldc(20)
				.RemUn(7)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(6);

		It should_return_remainder_of_tenmillion_by_1303 = () =>
			CreateStaticMethod()
			.Returns<uint>()
				.Ldc(10000000U)
				.Ldc(1303U)
				.RemUn()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(778U);

		It should_return_remainder_of_tenmillion_by_1303_ulong = () =>
			CreateStaticMethod()
			.Returns<ulong>()
				.Ldc(10000000UL)
				.Ldc(1303UL)
				.RemUn()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(778UL);

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
				.Ldc(0)
				.Add(3u)
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
				.Ldc(0U)
				.Sub(3)
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
				.Mul(3U)
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
				.Div(3U)
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
				.DivUn(3U)
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(5);
	}
}
