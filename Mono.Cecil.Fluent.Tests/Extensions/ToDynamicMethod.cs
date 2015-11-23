using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class FluentMethodBody_ToDynamicMethod : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		It should_create_and_invoke_simple_dynamic_method_that_returns_a_string = () =>
			CreateStaticMethod()
			.Returns<string>()
				.LdStr("teststring")
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal("teststring");
	}
}
