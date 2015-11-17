using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Nop()
		{
			return Emit(OpCodes.Nop);
		}

		public FluentMethodBody Dup()
		{
			return Emit(OpCodes.Dup);
		}

		public FluentMethodBody Pop()
		{
			return Emit(OpCodes.Pop);
		}

		public FluentMethodBody Not()
		{
			return Emit(OpCodes.Not);
		}

		public FluentMethodBody LdNull()
		{
			return Emit(OpCodes.Ldnull);
		}
	}
}