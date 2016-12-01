using System;
using System.Linq;
using System.Reflection;
using Machine.Specifications;
using Mono.Cecil.Fluent.Analyzer;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Analyzer
{
	public class StackValidation : TestsBase
    {
        It should_disable_stack_validation = () =>
            CreateStaticMethod()
            .ReturnsVoid()
                .AppendIL()
                    .DisableStackValidationOnEmit()
                    .Pop()
                    .Ret();

        It should_enable_stack_validation = () =>
        {
            var exceptionThrown = false;

            try
            {
                CreateStaticMethod()
                    .ReturnsVoid()
                    .AppendIL()
                        .DisableStackValidationOnEmit()
                        .EnableStackValidationOnEmit()
                        .Pop()
                        .Ret();
            } //ncrunch: no coverage
            catch { exceptionThrown = true; }

            if (!exceptionThrown)
                throw new Exception(); //ncrunch: no coverage
        };

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
