using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Extensions
{
    [TestClass]
	public class Extensions_NewType : TestsBase
	{
        [TestMethod]
		public void create_type () => 
			TestModule
			.CreateType()
			.Should().Not.Be.Null();

        [TestMethod]
		public void create_and_add_type_to_module () => 
			TestModule
			.CreateType()
			.Module.Should().Not.Be.Null();
	}
}
