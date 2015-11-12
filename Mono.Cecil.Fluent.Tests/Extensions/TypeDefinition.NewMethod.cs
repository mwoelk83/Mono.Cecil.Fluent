using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using Machine.Specifications;
// ReSharper disable All

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_TypeDefinition_NewMethod : TestsBase
	{
		static TypeDefinition TestType = CreateType();

		It should_create_method_without_return_type = 
			() => TestType.NewMethod().ReturnType.ShouldEqual(TestModule.TypeSystem.Void);

		It should_create_method_with_cecil_return_type =
			() => TestType.NewMethod(null, TestType).ReturnType.ShouldEqual(TestType);

		It should_create_method_with_system_return_type =
			() => TestType.NewMethod(typeof(ArrayList)).ReturnType.ShouldMatch(t => t.FullName == typeof(ArrayList).FullName);

		It should_create_named_method_with_system_return_type =
			() => TestType.NewMethod("named_method1", typeof (ArrayList)).Name.ShouldEqual("named_method1");

		It should_create_method_with_system_return_type_generic = 
			() => TestType.NewMethod<FileInfo>().ReturnType.ShouldMatch(t => t.FullName == typeof(FileInfo).FullName);

		It should_create_method_with_name =
			() => TestType.NewMethod("named_method2").Name.ShouldEqual("named_method2");
	}
}
