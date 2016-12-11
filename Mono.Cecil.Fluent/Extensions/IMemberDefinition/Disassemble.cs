using System;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Disassembler;

// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class IMemberDefinitionDefinitionExtensions
	{
		public static string Disassemble<T>(this T member)
            where T : class, IMemberDefinition
		{
			var writer = new PlainTextOutput();
			var dasm = new ReflectionDisassembler(writer);
            if (typeof(T) == typeof(EventDefinition))
                dasm.DisassembleEvent(member as EventDefinition);
            else if (typeof(T) == typeof(FieldDefinition))
                dasm.DisassembleField(member as FieldDefinition);
            else if (typeof(T) == typeof(PropertyDefinition))
                dasm.DisassembleProperty(member as PropertyDefinition);
            else if (typeof(T) == typeof(MethodDefinition))
                dasm.DisassembleMethod(member as MethodDefinition);
            else if (typeof(T) == typeof(TypeDefinition))
                dasm.DisassembleType(member as TypeDefinition);
            else
                throw new NotImplementedException($"the type {typeof(T).FullName} cannot be disassembled.");

            return writer.ToString();
		}
	}
}
