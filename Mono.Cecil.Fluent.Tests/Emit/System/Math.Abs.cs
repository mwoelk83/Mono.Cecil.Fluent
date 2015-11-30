using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class FluentMethodBody_System_Math_Abs : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static FluentMethodBody NewTestMethod => new FluentMethodBody(CreateMethod());

		It should_return_abs_of_positive_I4 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(int.MaxValue)
				.System.Math.Abs<int>()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(int.MaxValue);

		It should_return_abs_of_negative_I4 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc(int.MinValue + 1)
				.System.Math.Abs<int>()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(int.MaxValue);

		It should_return_abs_of_positive_I8 = () =>
			CreateStaticMethod()
			.Returns<long>()
				.Ldc(100L)
				.System.Math.Abs<long>()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100L);

		It should_return_abs_of_negative_I8 = () =>
			CreateStaticMethod()
			.Returns<long>()
				.Ldc(long.MinValue + 1)
				.System.Math.Abs<long>()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(long.MaxValue);

		It should_return_abs_of_positive_R4 = () =>
			CreateStaticMethod()
			.Returns<float>()
				.Ldc(100f)
				.System.Math.Abs<float>()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100f);

		It should_return_abs_of_negative_R4 = () =>
			CreateStaticMethod()
			.Returns<float>()
				.Ldc(-100f)
				.System.Math.Abs<float>()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100f);

		It should_return_abs_of_positive_R8 = () =>
			CreateStaticMethod()
			.Returns<double>()
				.Ldc(100d)
				.System.Math.Abs<double>()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100d);

		It should_return_abs_of_negative_R8 = () =>
			CreateStaticMethod()
			.Returns<double>()
				.Ldc(-100d)
				.System.Math.Abs<double>()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100d);
	}
}
