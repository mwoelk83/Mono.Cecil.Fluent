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
	}
}
