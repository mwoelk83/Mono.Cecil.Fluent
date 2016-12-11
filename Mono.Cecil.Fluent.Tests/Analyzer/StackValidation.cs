using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Fluent.Analyzer;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Analyzer
{
    [TestClass]
	public class StackValidation : TestsBase
    {
        public void disable_stack_validation () =>
            CreateStaticMethod()
            .ReturnsVoid()
                .AppendIL()
                    .SetStackValidationMode(StackValidationMode.Manual)
                    .Pop()
                    .Ret();

        [TestMethod]
        public void enable_stack_validation_on_return ()
        {
            var exceptionThrown = false;

            try
            {
                CreateStaticMethod()
                    .ReturnsVoid()
                    .AppendIL()
                        .SetStackValidationMode(StackValidationMode.OnReturn)
                        .Pop()
                        .Ret(); // << must throw exception here
            }
            catch { exceptionThrown = true; }

            if (!exceptionThrown)
                throw new Exception();
        }

        [TestMethod]
        public void enable_stack_validation_on_emit ()
        {
            var exceptionThrown = false;

            try
            {
                CreateStaticMethod()
                    .ReturnsVoid()
                    .AppendIL()
                        .SetStackValidationMode(StackValidationMode.OnEmit)
                        .Pop(); // << must throw exception here
            }
            catch { exceptionThrown = true; }

            if (!exceptionThrown)
                throw new Exception();
        }

        [TestMethod]
        public void validate_all_methods_of_types_in_system_namespace ()
		{
			foreach (var method in typeof(string).Assembly.GetTypes().Where(t => t.Namespace == "System")
				.SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)))
			{
				var body = TestModule.SafeImport(method).Resolve().Body;
                var analyzer = new FlowControlAnalyzer(body);
				analyzer.ValidateFullStackOrThrow();
			}
		}
	}
}
