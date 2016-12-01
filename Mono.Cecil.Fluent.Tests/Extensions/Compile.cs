using System;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class ToDynamicMethod : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		It should_create_simple_dynamic_method_that_returns_a_string = () =>
			CreateStaticMethod()
			.Returns<string>()
            .AppendIL()
                .LdStr("teststring")
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal("teststring");

		It should_compile_method_to_function = () => 
			CreateStaticMethod()
			.WithParameter<int>()
			.Returns<float>()
            .AppendIL()
                .LdParam(0)
				.ConvR4()
				.Ldc(10f)
				.Div()
				.Ret()
			.Compile<Func<int,float>>()(100).Should().Equal(10f);
	}
}
