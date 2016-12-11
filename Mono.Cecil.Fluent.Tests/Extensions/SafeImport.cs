using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Extensions
{
    [TestClass]
	public class Extensions_SafeImport : TestsBase
	{
        [TestMethod]
		public void import_typereference () =>
			TestModule
			.SafeImport(CreateType())
			.Should().Not.Be.Null();

        [TestMethod]
        public void import_methodreference () =>
			TestModule
			.SafeImport(CreateMethod())
			.Should().Not.Be.Null();

        [TestMethod]
        public void import_fieldreference () =>
			TestModule
			.SafeImport(CreateField())
			.Should().Not.Be.Null();

        [TestMethod]
        public void import_system_type_generic () => 
			TestModule
			.SafeImport<FileInfo>()
			.Should().Not.Be.Null();

        [TestMethod]
        public void import_system_type () =>
			TestModule
			.SafeImport(typeof(XNode))
			.Should().Not.Be.Null();

        [TestMethod]
        public void import_system_typeinfo () =>
			TestModule
			.SafeImport(typeof(XNode).GetTypeInfo())
			.Should().Not.Be.Null();

        [TestMethod]
        public void import_system_fieldinfo () =>
			TestModule
			.SafeImport(typeof(Tuple<string, string>).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).First())
			.Should().Not.Be.Null();

        [TestMethod]
        public void import_system_methodinfo () =>
			TestModule
			.SafeImport(typeof(Console).GetMethods().First())
			.Should().Not.Be.Null();

        [TestMethod]
        public void import_system_constructorinfo () =>
			TestModule
			.SafeImport(typeof(Tuple<string, string>).GetConstructors().First())
			.Should().Not.Be.Null();
	}
}
