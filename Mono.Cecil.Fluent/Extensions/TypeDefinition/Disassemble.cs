using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Disassembler;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static string Disassemble(this TypeDefinition type)
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
			dasm.DisassembleType(type);
			return writer.ToString();
		}
	}
}
