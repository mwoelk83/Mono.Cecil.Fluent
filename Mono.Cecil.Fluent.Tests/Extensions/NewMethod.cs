using System.Collections;
using System.IO;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_NewMethod : TestsBase
	{
		static TypeDefinition TestType = CreateType();

		It should_create_method_without_return_type = () => 
			TestType
			.CreateMethod()
			.ReturnType.Should().Equal(TestModule.TypeSystem.Void);

		It should_create_method_with_cecil_return_type = () =>
			TestType
			.CreateMethod(null, TestType)
			.ReturnType.Should().Equal(TestType);

		It should_create_method_with_system_return_type = () =>
			TestType
			.CreateMethod(typeof (ArrayList))
			.ReturnType.Should().Equal<ArrayList>();

		It should_create_named_method_with_system_return_type = () =>
			TestType
			.CreateMethod("named_method1", typeof(ArrayList))
			.Name.Should().Equal("named_method1");

		It should_create_method_with_system_return_type_generic = () => 
			TestType
			.CreateMethod<FileInfo>()
			.ReturnType.Should().Equal<FileInfo>();

		It should_create_named_method_with_system_return_type_generic = () =>
			TestType
			.CreateMethod<FileInfo>("named_method2")
			.Name.Should().Equal("named_method2");

		It should_create_method_with_name = () =>
			TestType
			.CreateMethod("named_method3")
			.Name.Should().Equal("named_method3");

		It should_create_many_methods = () =>
			TestType
				.CreateMethod()
				.CreateMethod()
				.CreateMethod()
				.CreateMethod()
				.CreateMethod()
				.CreateMethod()
			.DeclaringType
			.Methods.Count.Should().Be.GreaterThanOrEqualTo(6);
	}
}
