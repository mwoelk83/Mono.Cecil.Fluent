using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class FluentMethodBody_BasicMath_Abs : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		It should_return_abs_of_positive_I4 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(int.MaxValue)
				.AbsI4()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(int.MaxValue);

		It should_return_abs_of_negative_I4 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(int.MinValue + 1)
				.AbsI4()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(int.MaxValue);

		It should_return_abs_of_positive_I8 = () =>
			CreateStaticMethod()
			.Returns<long>()
				.Ldc(100L)
				.AbsI8()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100L);

		It should_return_abs_of_negative_I8 = () =>
			CreateStaticMethod()
			.Returns<long>()
				.Ldc(long.MinValue + 1)
				.AbsI8()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(long.MaxValue);

		It should_return_abs_of_positive_R4 = () =>
			CreateStaticMethod()
			.Returns<float>()
				.Ldc(100f)
				.AbsR4()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100f);

		It should_return_abs_of_negative_R4 = () =>
			CreateStaticMethod()
			.Returns<float>()
				.Ldc(-100f)
				.AbsR4()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100f);

		It should_return_abs_of_positive_R8 = () =>
			CreateStaticMethod()
			.Returns<double>()
				.Ldc(100d)
				.AbsR8()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100d);

		It should_return_abs_of_negative_R8 = () =>
			CreateStaticMethod()
			.Returns<double>()
				.Ldc(-100d)
				.AbsR8()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100d);
	}
}
