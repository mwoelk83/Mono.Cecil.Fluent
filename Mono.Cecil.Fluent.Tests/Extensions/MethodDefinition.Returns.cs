using System.Collections;
using System.IO;
using Machine.Specifications;
// ReSharper disable All

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_MethodDefinition_Returns : TestsBase
	{
		static TypeDefinition TestType = CreateType();
		static MethodDefinition TestMethod = TestType.NewMethod();

		It should_return_void =
			() => TestMethod.ReturnsVoid().ReturnType.ShouldEqual(TestModule.TypeSystem.Void);

		It should_return_typereference =
			() => TestMethod.Returns(TestType).ReturnType.ShouldEqual(TestType);

		It should_return_system_type =
			() => TestMethod.Returns(typeof(ArrayList)).ReturnType.FullName.ShouldEqual(typeof(ArrayList).FullName);

		It should_return_system_type_generic =
			() => TestMethod.Returns<FileInfo>().ReturnType.FullName.ShouldEqual(typeof(FileInfo).FullName);
	}
}
