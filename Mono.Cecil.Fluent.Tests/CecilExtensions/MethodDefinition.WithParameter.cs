using System.Collections;
using System.IO;
using System.Linq;
using Machine.Specifications;
using Machine.Specifications.Model;

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
				.Parameters.ShouldNotBeEmpty();

		It should_create_param_with_typerefernce =
			() => NewTestMethod
				.WithParameter(TestType)
				.Parameters.First().ParameterType.ShouldEqual(TestType);

		It should_create_param_with_system_type =
			() => NewTestMethod
				.WithParameter(typeof(ArrayList))
				.Parameters.First().ParameterType.FullName.ShouldEqual(typeof(ArrayList).FullName);

		It should_create_param_with_system_type_generic =
			() => NewTestMethod
				.WithParameter<FileInfo>()
				.Parameters.First().ParameterType.FullName.ShouldEqual(typeof(FileInfo).FullName);

		It should_create_three_parameters =
			() => NewTestMethod
				.WithParameter<bool>()
				.WithParameter(typeof (int))
				.WithParameter(TestType)
				.Parameters.Count.ShouldEqual(3);
	}
}
