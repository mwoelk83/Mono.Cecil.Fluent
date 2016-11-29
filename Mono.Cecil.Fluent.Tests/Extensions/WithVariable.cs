using System.Collections;
using System.IO;
using System.Linq;
using Machine.Specifications;
using Mono.Cecil.Cil;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_WithVariable : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static MethodDefinition NewTestMethod => CreateMethod();

		It should_create_var = () =>
			NewTestMethod
			.WithVariable(TestType)
			.Body.Variables.Should().Not.Be.Empty();

		It should_create_var_with_typerefernce = () =>
			NewTestMethod
			.WithVariable(TestType)
            .Body.Variables.First().VariableType.Should().Equal(TestType);

		It should_create_var_with_system_type = () =>
			NewTestMethod
			.WithVariable(typeof(ArrayList))
            .Body.Variables.First().VariableType.Should().Equal<ArrayList>();

		It should_create_var_with_system_type_generic = () =>
			NewTestMethod
			.WithVariable<FileInfo>()
            .Body.Variables.First().VariableType.Should().Equal<FileInfo>();

		It should_create_named_var_with_typerefernce = () =>
			NewTestMethod
			.WithVariable(TestType, "param1")
            .Body.Variables.First().Name.Should().Equal("param1");

		It should_create_named_var_with_system_type = () =>
			NewTestMethod
			.WithVariable(typeof(ArrayList), "param2")
            .Body.Variables.First().Name.Should().Equal("param2");

		It should_create_named_var_with_system_type_generic = () =>
			NewTestMethod
			.WithVariable<FileInfo>("param3")
            .Body.Variables.First().Name.Should().Equal("param3");

		It should_add_variable_using_variable_definition = () =>
			NewTestMethod
			.WithVariable(new VariableDefinition(TestModule.SafeImport<bool>()))
			.WithVariable(new VariableDefinition(TestModule.SafeImport<uint>()))
			.WithVariable(new VariableDefinition(TestModule.SafeImport<object>()))
            .Body.Variables.Last().Index.Should().Equal(2);

		It should_create_three_variables = () =>
			NewTestMethod
			.WithVariable<bool>()
			.WithVariable(typeof(int))
			.WithVariable(TestType)
            .Body.Variables.Count.Should().Equal(3);
	}
}
