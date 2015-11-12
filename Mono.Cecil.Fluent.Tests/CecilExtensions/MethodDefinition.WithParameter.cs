using System.Collections;
using System.IO;
using System.Linq;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable All

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_MethodDefinition_WithParameter : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static MethodDefinition NewTestMethod => CreateMethod();

		It should_create_param =
			() => NewTestMethod
				.WithParameter(TestType)
				.Parameters.Should().Not.Be.Empty();

		It should_create_param_with_typerefernce =
			() => NewTestMethod
				.WithParameter(TestType)
				.Parameters.First().ParameterType.Should().Equal(TestType);

		It should_create_param_with_system_type =
			() => NewTestMethod
				.WithParameter(typeof(ArrayList))
				.Parameters.First().ParameterType.Should().Equal<ArrayList>();

		It should_create_param_with_system_type_generic =
			() => NewTestMethod
				.WithParameter<FileInfo>()
				.Parameters.First().ParameterType.Should().Equal<FileInfo>();

		It should_create_three_parameters =
			() => NewTestMethod
				.WithParameter<bool>()
				.WithParameter(typeof(int))
				.WithParameter(TestType)
				.Parameters.Count.Should().Equal(3);
	}
}
