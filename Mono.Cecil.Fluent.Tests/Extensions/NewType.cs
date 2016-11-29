using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_NewType : TestsBase
	{
		It should_create_type = () => 
			TestModule
			.CreateType()
			.Should().Not.Be.Null();

		It should_create_and_add_type_to_module = () => 
			TestModule
			.CreateType()
			.Module.Should().Not.Be.Null();
	}
}
