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
			.Invoke(null, null);

		It shoud_invert_bitwise = () =>
			CreateStaticMethod()
				.Returns<ushort>()
				.Ldc((ushort)0x00FF)
				.Not()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal((ushort)0xFF00);

		It shoud_ldnull = () =>
			CreateStaticMethod()
				.Returns<object>()
				.LdNull()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(null);

		It shoud_ret_int = () =>
			CreateStaticMethod()
				.Returns<int>()
				.Ret(10d)
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(10);

		It shoud_ret_long = () =>
			CreateStaticMethod()
				.Returns<long>()
				.Ret(1f)
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(1L);

		It shoud_ret_float = () =>
			CreateStaticMethod()
				.Returns<float>()
				.Ret(10d)
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(10f);

		It shoud_ret_double = () =>
			CreateStaticMethod()
				.Returns<double>()
				.Ret(10f)
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(10d);

		It shoud_ret_loc_with_name = () =>
			CreateStaticMethod()
				.WithVariable<int>("ret")
				.Returns<int>()
				.Stloc(100, "ret")
				.RetLoc("ret")
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100);

		It shoud_ret_loc_with_index = () =>
			CreateStaticMethod()
				.WithVariable<int>("ret")
				.Returns<int>()
				.Stloc(1100, "ret")
				.RetLoc(0)
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(1100);

		It shoud_ret_arg_with_name = () =>
			CreateStaticMethod()
				.WithParameter<int>("ret")
				.Returns<int>()
				.RetArg("ret")
			.ToDynamicMethod()
			.Invoke(null, new object[] { 100 }).Should().Equal(100);

		It should_validate_stack_for_string_compare_method = () =>
		{
			var m = new FluentMethodBody(TestModule.SafeImport(typeof (string).GetMethod("Compare",
				new[] {typeof (string), typeof (string), typeof (StringComparison)})).Resolve());
				foreach(var instruction in m.Body.Instructions)
					SimpleStackValidator.ValidatePostEmit(instruction, m);
		};
	}
}
