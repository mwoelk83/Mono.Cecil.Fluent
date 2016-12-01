using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class FluentEmitter
	{
		public FluentEmitter Nop()
		{
			return Emit(OpCodes.Nop);
		}

		public FluentEmitter Dup()
		{
			return Emit(OpCodes.Dup);
		}

		public FluentEmitter Pop()
		{
			return Emit(OpCodes.Pop);
		}

		public FluentEmitter Not()
		{
			return Emit(OpCodes.Not);
		}

		public FluentEmitter LdNull()
		{
			return Emit(OpCodes.Ldnull);
		}
	}
}