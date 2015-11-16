using System.Collections;
using System.IO;
using System.Linq;
using Machine.Specifications;
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
			.Variables.Should().Not.Be.Empty();

		It should_create_var_with_typerefernce = () =>
			NewTestMethod
			.WithVariable(TestType)
			.Variables.First().VariableType.Should().Equal(TestType);

		It should_create_var_with_system_type = () =>
			NewTestMethod
			.WithVariable(typeof(ArrayList))
			.Variables.First().VariableType.Should().Equal<ArrayList>();

		It should_create_var_with_system_type_generic = () =>
			NewTestMethod
			.WithVariable<FileInfo>()
			.Variables.First().VariableType.Should().Equal<FileInfo>();

		It should_create_named_var_with_typerefernce = () =>
			NewTestMethod
			.WithVariable(TestType, "param1")
			.Variables.First().Name.Should().Equal("param1");

		It should_create_named_var_with_system_type = () =>
			NewTestMethod
			.WithVariable(typeof(ArrayList), "param2")
			.Variables.First().Name.Should().Equal("param2");

		It should_create_named_var_with_system_type_generic = () =>
			NewTestMethod
			.WithVariable<FileInfo>("param3")
			.Variables.First().Name.Should().Equal("param3");

		It should_create_three_variables = () =>
			NewTestMethod
			.WithVariable<bool>()
			.WithVariable(typeof(int))
			.WithVariable(TestType)
			.Variables.Count.Should().Equal(3);
	}
}
