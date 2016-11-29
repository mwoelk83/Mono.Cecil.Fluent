using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Disassembler;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static string DisassembleBody(this FluentMethodBody method)
		{
			return method.MethodDefinition.Body.Disassemble();
		}

        public static string Disassemble(this FluentMethodBody method)
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
			dasm.DisassembleMethod(method.MethodDefinition);
			return writer.ToString();
		}
	}
}
