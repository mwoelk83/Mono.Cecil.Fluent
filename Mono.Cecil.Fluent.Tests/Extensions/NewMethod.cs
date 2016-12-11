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
	public class Extensions_NewMethod : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

        [TestMethod]
		public void create_method_without_return_type () => 
			TestType
			.CreateMethod()
			.ReturnType.Should().Equal(TestModule.TypeSystem.Void);

        [TestMethod]
        public void create_method_with_cecil_return_type () =>
			TestType
			.CreateMethod(null, TestType)
			.ReturnType.Should().Equal(TestType);

        [TestMethod]
        public void create_method_with_system_return_type () =>
			TestType
			.CreateMethod(typeof (ArrayList))
			.ReturnType.Should().Equal<ArrayList>();

        [TestMethod]
        public void create_named_method_with_system_return_type () =>
			TestType
			.CreateMethod("named_method1", typeof(ArrayList))
			.Name.Should().Equal("named_method1");

        [TestMethod]
        public void create_method_with_system_return_type_generic () => 
			TestType
			.CreateMethod<FileInfo>()
			.ReturnType.Should().Equal<FileInfo>();

        [TestMethod]
        public void create_named_method_with_system_return_type_generic () =>
			TestType
			.CreateMethod<FileInfo>("named_method2")
			.Name.Should().Equal("named_method2");

        [TestMethod]
        public void create_method_with_name () =>
			TestType
			.CreateMethod("named_method3")
			.Name.Should().Equal("named_method3");
	}
}
