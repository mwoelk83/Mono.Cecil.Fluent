using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Disassembler;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static string DisassembleBody(this MethodDefinition method)
		{
			return method.Body.Disassemble();
		}

		public static string DisassembleBody(this FluentMethodBody method)
		{
			return method.MethodDefinition.Body.Disassemble();
		}

		public static string Disassemble(this MethodBody body)
		{
			var writer = new PlainTextOutput();
			var dasm = new MethodBodyDisassembler(writer);
			dasm.Disassemble(body);
			return writer.ToString();
		}

		public static string Disassemble(this MethodDefinition method)
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
			dasm.DisassembleMethod(method);
			return writer.ToString();
		}

		public static string Disassemble(this FluentMethodBody method)
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
			dasm.DisassembleMethod(method.MethodDefinition);
			return writer.ToString();
		}

		public static string Disassemble(this TypeDefinition type)
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
			dasm.DisassembleType(type);
			return writer.ToString();
		}

		public static string Disassemble(this PropertyDefinition prop)
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
			dasm.DisassembleProperty(prop);
			return writer.ToString();
		}

		public static string Disassemble(this FieldDefinition field)
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
			dasm.DisassembleField(field);
			return writer.ToString();
		}

		public static string Disassemble(this EventDefinition @event)
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
			dasm.DisassembleEvent(@event);
			return writer.ToString();
		}
	}
}
