using System.Collections;
using System.IO;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.CecilExtensions
{
	public class Extensions_NewMethod : TestsBase
	{
		static TypeDefinition TestType = CreateType();

		It should_create_method_without_return_type = () => 
			TestType
			.NewMethod()
			.ReturnType.Should().Equal(TestModule.TypeSystem.Void);

		It should_create_method_with_cecil_return_type = () =>
			TestType
			.NewMethod(null, TestType)
			.ReturnType.Should().Equal(TestType);

		It should_create_method_with_system_return_type = () =>
			TestType
			.NewMethod(typeof (ArrayList))
			.ReturnType.Should().Equal<ArrayList>();

		It should_create_named_method_with_system_return_type = () =>
			TestType
			.NewMethod("named_method1", typeof(ArrayList))
			.Name.Should().Equal("named_method1");

		It should_create_method_with_system_return_type_generic = () => 
			TestType
			.NewMethod<FileInfo>()
			.ReturnType.Should().Equal<FileInfo>();

		It should_create_named_method_with_system_return_type_generic = () =>
			TestType
			.NewMethod<FileInfo>("named_method2")
			.Name.Should().Equal("named_method2");

		It should_create_method_with_name = () => 
			TestType
			.NewMethod("named_method3")
			.Name.Should().Equal("named_method3");
	}
}
