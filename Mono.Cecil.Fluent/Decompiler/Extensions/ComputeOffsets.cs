using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodBodyExtensions
	{
		public static void ComputeOffsets(this MethodBody body)
		{
			var offset = 0;
			foreach (var instr in body.Instructions)
			{
				instr.Offset = offset;
				offset += instr.GetSize();
			}
		}
	}
}
