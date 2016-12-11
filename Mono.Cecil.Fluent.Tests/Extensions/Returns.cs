using System.Collections;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Extensions
{
    [TestClass]
	public class Extensions_Returns : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();
		static readonly MethodDefinition TestMethod = CreateMethod();

        [TestMethod]
		public void return_void () => 
			TestMethod
			.ReturnsVoid()
			.ReturnType.Should().Equal(TestModule.TypeSystem.Void);

        [TestMethod]
        public void return_typereference () => 
			TestMethod
			.Returns(TestType)
			.ReturnType.Should().Equal(TestType);

        [TestMethod]
        public void return_system_type () => 
			TestMethod
			.Returns(typeof(ArrayList))
			.ReturnType.Should().Equal<ArrayList>();

        [TestMethod]
        public void return_system_type_generic () =>
			TestMethod
			.Returns<FileInfo>()
			.ReturnType.Should().Equal<FileInfo>();
	}
}
