using System.Collections;
using System.IO;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.CecilExtensions
{
	public class Extensions_Returns : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();
		static readonly MethodDefinition TestMethod = CreateMethod();

		It should_return_void = () => 
			TestMethod
			.ReturnsVoid()
			.ReturnType.Should().Equal(TestModule.TypeSystem.Void);

		It should_return_typereference = () => 
			TestMethod
			.Returns(TestType)
			.ReturnType.Should().Equal(TestType);

		It should_return_system_type = () => 
			TestMethod
			.Returns(typeof(ArrayList))
			.ReturnType.Should().Equal<ArrayList>();

		It should_return_system_type_generic = () =>
			TestMethod
			.Returns<FileInfo>()
			.ReturnType.Should().Equal<FileInfo>();
	}
}
