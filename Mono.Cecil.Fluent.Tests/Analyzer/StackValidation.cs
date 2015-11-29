using System.Linq;
using System.Reflection;
using Machine.Specifications;
using Mono.Cecil.Fluent.StackValidation;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Analyzer
{
	public class FluentMethodBody_StackValidation : TestsBase
	{
		It should_validate_all_methods_of_types_in_system_namespace = () =>
		{
			foreach (var method in typeof(string).Assembly.GetTypes().Where(t => t.Namespace == "System")
				.SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)))
			{
				var body = TestModule.SafeImport(method).Resolve().Body;
                var analyzer = new FlowControlAnalyzer(body);
				analyzer.ValidateFullStackOrThrow();
			}
		};
	}
}
