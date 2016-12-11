using System.Collections;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Extensions
{
    [TestClass]
	public class Extensions_WithParameter : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static MethodDefinition NewTestMethod => CreateMethod();

		static readonly ParameterDefinition testparam = new ParameterDefinition(TestModule.TypeSystem.String);

        [TestMethod]
        public void create_param () => 
			NewTestMethod
			.WithParameter(TestType)
			.Parameters.Should().Not.Be.Empty();

        [TestMethod]
        public void create_param_with_typerefernce () =>
			NewTestMethod
			.WithParameter(TestType)
			.Parameters.First().ParameterType.Should().Equal(TestType);

        [TestMethod]
        public void add_parameterdefinition () =>
			NewTestMethod
			.WithParameter(testparam)
			.Parameters.First().ParameterType.Should().Equal(testparam.ParameterType);

        [TestMethod]
        public void create_param_with_system_type () => 
			NewTestMethod
			.WithParameter(typeof(ArrayList))
			.Parameters.First().ParameterType.Should().Equal<ArrayList>();

        [TestMethod]
        public void create_param_with_system_type_generic () =>
			NewTestMethod
			.WithParameter<FileInfo>()
			.Parameters.First().ParameterType.Should().Equal<FileInfo>();

        [TestMethod]
        public void create_named_param_with_typerefernce () =>
			NewTestMethod
			.WithParameter(TestType, "param1")
			.Parameters.First().Name.Should().Equal("param1");

        [TestMethod]
        public void create_named_param_with_system_type () =>
			NewTestMethod
			.WithParameter(typeof (ArrayList), "param2")
			.Parameters.First().Name.Should().Equal("param2");

        [TestMethod]
        public void create_named_param_with_system_type_generic () =>
			NewTestMethod
			.WithParameter<FileInfo>("param3")
			.Parameters.First().Name.Should().Equal("param3");

        [TestMethod]
        public void create_three_parameters () => 
			NewTestMethod
			.WithParameter<bool>()
			.WithParameter(typeof(int))
			.WithParameter(TestType)
			.Parameters.Count.Should().Equal(3);
	}
}
