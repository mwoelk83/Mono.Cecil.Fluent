using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.CecilExtensions
{
	public class Extensions_NewType : TestsBase
	{
		It should_create_type = () => 
			TestModule
			.NewType()
			.Should().Not.Be.Null();

		It should_create_and_add_type_to_module = () => 
			TestModule
			.NewType()
			.Module.Should().Not.Be.Null();
	}
}
