using System.Diagnostics.CodeAnalysis;
using Machine.Specifications;

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class Extensions_NewType : TestsBase
	{
		It should_add_type_to_module = () => TestModule.NewType().Module.ShouldNotBeNull();
	}
}
