using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
    [TestClass]
    public class Disassembly_Type : TestsBase
	{
		static readonly string LF = Environment.NewLine;

        [TestMethod]
        public void disassemble_type_string () =>
			TestModule.SafeImport(typeof(string)).Resolve()
				.Disassemble()
				.Should().StartWith(
						".class public auto ansi sealed serializable beforefieldinit System.String" + LF +
						"	extends System.Object" + LF +
						"	implements System.IComparable," + LF);

        [TestMethod]
        public void disassemble_type_datetime () =>
			TestModule.SafeImport(typeof(DateTime)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_console () =>
			TestModule.SafeImport(typeof(Console)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_marshal () =>
			TestModule.SafeImport(typeof(Marshal)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_attribute () =>
			TestModule.SafeImport(typeof(Attribute)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_assembly () =>
			TestModule.SafeImport(typeof(Assembly)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_float () =>
			TestModule.SafeImport(typeof(float)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_char () =>
			TestModule.SafeImport(typeof(char)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_double () =>
			TestModule.SafeImport(typeof(double)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_gc () =>
			TestModule.SafeImport(typeof(GC)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_iconvertible () =>
			TestModule.SafeImport(typeof(IConvertible)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_filestream () =>
			TestModule.SafeImport(typeof(FileStream)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_path () =>
			TestModule.SafeImport(typeof(Path)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_fieldinfo () =>
			TestModule.SafeImport(typeof(FieldInfo)).Resolve()
				.Disassemble();

        [TestMethod]
        public void disassemble_type_task () =>
			TestModule.SafeImport(typeof(Task)).Resolve()
				.Disassemble();
	}
}
