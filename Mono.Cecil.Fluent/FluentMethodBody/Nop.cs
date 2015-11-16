using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Nop()
		{
			return Emit(OpCodes.Nop);
		}
	}
}