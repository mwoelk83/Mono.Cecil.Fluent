using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Disassembler;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    public static partial class MethodDefinitionExtensions
    {
        public static string DisassembleBody(this MethodDefinition method)
        {
            return method.Body.Disassemble();
        }

        public static string Disassemble(this MethodDefinition method)
        {
            var writer = new PlainTextOutput();
            var dasm = new ReflectionDisassembler(writer);
            dasm.DisassembleMethod(method);
            return writer.ToString();
        }
    }
}