using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class FluentMethodBody_ToDynamicMethod : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		It schould_emit_instruction = () =>
			CreateStaticMethod()
			.Returns<string>()
				.LdStr("teststring")
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal("teststring");
	}
}
