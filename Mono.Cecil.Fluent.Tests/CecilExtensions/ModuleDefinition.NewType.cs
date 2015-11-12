using Machine.Specifications;
// ReSharper disable All

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_ModuleDefinition_NewType : TestsBase
	{
		It should_create_type = 
			() => TestModule.NewType().ShouldNotBeNull();

		It should_create_and_add_type_to_module = 
			() => TestModule.NewType().Module.ShouldNotBeNull();
	}
}
