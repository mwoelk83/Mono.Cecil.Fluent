using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Nop
		{
			get
			{
				Emit(OpCodes.Nop);
				return this;
			}
		}
	}
}