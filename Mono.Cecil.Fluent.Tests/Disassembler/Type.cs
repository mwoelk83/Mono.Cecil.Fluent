using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Disassembler
{
	public class Disassembly_Type : TestsBase
	{
		static readonly string LF = Environment.NewLine;

		It should_disassemble_type_string = () =>
			TestModule.SafeImport(typeof(string)).Resolve()
				.Disassemble()
				.Should().StartWith(
						".class public auto ansi sealed serializable beforefieldinit System.String" + LF +
						"	extends System.Object" + LF +
						"	implements System.IComparable," + LF);

		It should_disassemble_type_datetime = () =>
			TestModule.SafeImport(typeof(DateTime)).Resolve()
				.Disassemble();

		It should_disassemble_type_console = () =>
			TestModule.SafeImport(typeof(Console)).Resolve()
				.Disassemble();

		It should_disassemble_type_marshal = () =>
			TestModule.SafeImport(typeof(Marshal)).Resolve()
				.Disassemble();

		It should_disassemble_type_attribute = () =>
			TestModule.SafeImport(typeof(Attribute)).Resolve()
				.Disassemble();

		It should_disassemble_type_assembly = () =>
			TestModule.SafeImport(typeof(Assembly)).Resolve()
				.Disassemble();

		It should_disassemble_type_float = () =>
			TestModule.SafeImport(typeof(float)).Resolve()
				.Disassemble();

		It should_disassemble_type_char = () =>
			TestModule.SafeImport(typeof(char)).Resolve()
				.Disassemble();

		It should_disassemble_type_double = () =>
			TestModule.SafeImport(typeof(double)).Resolve()
				.Disassemble();

		It should_disassemble_type_gc = () =>
			TestModule.SafeImport(typeof(GC)).Resolve()
				.Disassemble();

		It should_disassemble_type_iconvertible = () =>
			TestModule.SafeImport(typeof(IConvertible)).Resolve()
				.Disassemble();

		It should_disassemble_type_filestream = () =>
			TestModule.SafeImport(typeof(FileStream)).Resolve()
				.Disassemble();

		It should_disassemble_type_path = () =>
			TestModule.SafeImport(typeof(Path)).Resolve()
				.Disassemble();

		It should_disassemble_type_fieldinfo = () =>
			TestModule.SafeImport(typeof(FieldInfo)).Resolve()
				.Disassemble();

		It should_disassemble_type_task = () =>
			TestModule.SafeImport(typeof(Task)).Resolve()
				.Disassemble();
	}
}
