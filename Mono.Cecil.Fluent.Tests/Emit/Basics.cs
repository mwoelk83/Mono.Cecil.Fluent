using System;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class FluentMethodBody_Basics : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static FluentMethodBody NewTestMethod => new FluentMethodBody(CreateMethod());

		It should_dup_value = () =>
			CreateStaticMethod()
				.Returns<int>()
				.Ldc(10)
				.Dup()
				.Add()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(20);

		It should_add_nops = () =>
			CreateStaticMethod()
				.ReturnsVoid()
				.Nop()
				.Nop()
				.Nop()
			.Body.Instructions.Count.Should().Equal(3);

		It should_pop_value = () =>
			CreateStaticMethod()
				.ReturnsVoid()
				.Ldc(0)
				.Pop()
				.Ret()
			.Compile<Action>()();

		It should_invert_bitwise = () =>
			CreateStaticMethod()
				.Returns<ushort>()
				.Ldc((ushort)0x00FF)
				.Not()
				.Ret()
			.Compile<Func<ushort>>()
			().Should().Equal((ushort)0xFF00);

		It should_ldnull = () =>
			CreateStaticMethod()
				.Returns<object>()
				.LdNull()
				.Ret()
			.Compile<Func<object>>()
			().Should().Equal(null);

		It should_ret_int = () =>
			CreateStaticMethod()
				.Returns<int>()
				.Ret(10d)
			.Compile<Func<int>>()
			().Should().Equal(10);

		It should_ret_long = () =>
			CreateStaticMethod()
				.Returns<long>()
				.Ret(1f)
			.Compile<Func<long>>()
			().Should().Equal(1L);

		It should_ret_float = () =>
			CreateStaticMethod()
				.Returns<float>()
				.Ret(10d)
			.Compile<Func<float>>()
			().Should().Equal(10f);

		It should_ret_double = () =>
			CreateStaticMethod()
				.Returns<double>()
				.Ret(10f)
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(10d);

		It should_ret_loc_with_name = () =>
			CreateStaticMethod()
				.WithVariable<int>("ret")
				.Returns<int>()
				.Stloc(100, "ret")
				.RetLoc("ret")
			.Compile<Func<int>>()
			().Should().Equal(100);

		It should_ret_loc_with_index = () =>
			CreateStaticMethod()
				.WithVariable<int>("ret")
				.Returns<int>()
				.Stloc(1100, "ret")
				.RetLoc(0)
			.Compile<Func<int>>()
			().Should().Equal(1100);

		It should_ret_arg_with_name = () =>
			CreateStaticMethod()
				.WithParameter<int>("ret")
				.Returns<int>()
				.RetArg("ret")
			.Compile<Func<int,int>>()
			(100).Should().Equal(100);
	}
}
