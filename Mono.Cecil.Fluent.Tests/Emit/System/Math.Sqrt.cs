using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class FluentMethodBody_System_Math : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static FluentMethodBody NewTestMethod => new FluentMethodBody(CreateMethod());

		It should_return_sqrt_for_double = () =>
			CreateStaticMethod()
			.Returns<double>()
				.Ldc(100d)
				.System.Math.Sqrt()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(10d);

		It should_return_sqrt_for_float = () =>
			CreateStaticMethod()
			.Returns<float>()
				.Ldc(100f)
				.System.Math.Sqrt()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(10f);
	}
}
