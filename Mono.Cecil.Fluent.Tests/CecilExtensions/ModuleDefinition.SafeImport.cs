using System.IO;
using System.Xml.Linq;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.CecilExtensions
{
	public class Extensions_ModuleDefinition_SafeImport : TestsBase
	{
		It schould_import_type_reference = () => 
			TestModule
			.SafeImport(CreateType())
			.Should().Not.Be.Null();

		It schould_import_type_system_type_generic = () => 
			TestModule
			.SafeImport<FileInfo>()
			.Should().Not.Be.Null();

		It schould_import_type_system_type = () => 
			TestModule
			.SafeImport(typeof (XNode))
			.Should().Not.Be.Null();
	}
}
