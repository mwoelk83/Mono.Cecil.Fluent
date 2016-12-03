using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Disassembler;
using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodBodyExtensions
	{
        public static string Disassemble(this MethodBody body)
        {
            var writer = new PlainTextOutput();
            var dasm = new MethodBodyDisassembler(writer);
            dasm.Disassemble(body);
            return writer.ToString();
        }
	}
}
