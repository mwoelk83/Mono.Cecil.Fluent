using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Machine.Specifications;
using Machine.Specifications.Utility;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_SafeImport : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static MethodDefinition NewTestMethod => CreateMethod();

		static FieldDefinition NewTestField => CreateField();

		It should_import_typereference = () =>
			TestModule
			.SafeImport(CreateType())
			.Should().Not.Be.Null();

		It should_import_methodreference = () =>
			TestModule
			.SafeImport(CreateMethod())
			.Should().Not.Be.Null();

		It should_import_fieldreference = () =>
			TestModule
			.SafeImport(CreateField())
			.Should().Not.Be.Null();

		It should_import_system_type_generic = () => 
			TestModule
			.SafeImport<FileInfo>()
			.Should().Not.Be.Null();

		It should_import_system_type = () =>
			TestModule
			.SafeImport(typeof(XNode))
			.Should().Not.Be.Null();

		It should_import_system_typeinfo = () =>
			TestModule
			.SafeImport(typeof(XNode).GetTypeInfo())
			.Should().Not.Be.Null();

		It should_import_system_fieldinfo = () =>
			TestModule
			.SafeImport(typeof(Tuple<string, string>).GetInstanceFields().First())
			.Should().Not.Be.Null();

		It should_import_system_methodinfo = () =>
			TestModule
			.SafeImport(typeof(Console).GetMethods().First())
			.Should().Not.Be.Null();

		It should_import_system_constructorinfo = () =>
			TestModule
			.SafeImport(typeof(Tuple<string, string>).GetConstructors().First())
			.Should().Not.Be.Null();
	}
}
