using System;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class BasicMath : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static FluentEmitter NewTestMethod => new FluentEmitter(CreateMethod());

		It should_return_remainder_of_ten_by_four = () =>
			CreateStaticMethod()
			.Returns<byte>()
            .AppendIL()
				.Ldc((byte)10)
				.Rem(4)
				.Ret()
			.Compile<Func<byte>>()
			().Should().Equal((byte)2);

		It should_return_remainder_of_twenty_by_seven = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc((ushort)20)
				.Ldc((short)7)
				.Rem()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(6);

		It should_return_remainder_of_twenty_by_seven_unsigned = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(20)
				.RemUn(7)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(6);

		It should_return_remainder_of_tenmillion_by_1303 = () =>
			CreateStaticMethod()
			.Returns<uint>()
            .AppendIL()
                .Ldc(10000000U)
				.Ldc(1303U)
				.RemUn()
				.Ret()
			.Compile<Func<uint>>()
			().Should().Equal(778U);

		It should_return_remainder_of_tenmillion_by_1303_ulong = () =>
			CreateStaticMethod()
			.Returns<ulong>()
            .AppendIL()
                .Ldc(10000000UL)
				.Ldc(1303UL)
				.RemUn()
				.Ret()
			.Compile<Func<ulong>>()
			().Should().Equal(778UL);

		It should_add_3 = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(0)
				.Ldc(3)
				.Add()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(3);

		It should_add_3_with_arg = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(0)
				.Add(3u)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(3);

		It should_sub_3 = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(0)
				.Ldc(3)
				.Sub()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(-3);

		It should_sub_3_with_arg = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(0U)
				.Sub(3)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(-3);

		It should_mul_by_3 = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(5)
				.Ldc(3)
				.Mul()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(15);

		It should_mul_by_3_with_arg = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(5)
				.Mul(3U)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(15);

		It should_div_by_3 = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(15)
				.Ldc(3)
				.Div()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(5);

		It should_div_by_3_with_arg = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(15)
				.Div(3U)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(5);

		It should_div_by_3_un = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(15)
				.Ldc(3)
				.DivUn()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(5);

		It should_div_by_3_with_arg_un = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(15)
				.DivUn(3U)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(5);

		// improves code coverage
		It should_add_a_bool_and_a_sbyte = () => 
			CreateStaticMethod()
			.Returns<byte>()
            .AppendIL()
                .Ldc(true)
			    .Ldc((sbyte) 2)
			    .Add()
			    .Ret()
			.Compile<Func<byte>>()
			().Should().Equal((byte) 3);
	}
}
