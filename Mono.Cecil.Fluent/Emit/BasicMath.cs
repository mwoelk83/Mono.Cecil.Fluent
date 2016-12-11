using System;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class FluentEmitter
	{
		public FluentEmitter Neg()
		{
			return Emit(OpCodes.Neg);
		}

		public FluentEmitter Rem()
		{
			return Emit(OpCodes.Rem);
		}

		public FluentEmitter RemUn()
		{
			return Emit(OpCodes.Rem_Un);
		}

		public FluentEmitter Rem(MagicNumberArgument divisor)
		{
			return divisor.EmitLdc(this)
				.Emit(OpCodes.Rem);
		}

		public FluentEmitter RemUn(MagicNumberArgument divisor)
		{
			return divisor.EmitLdc(this)
				.Emit(OpCodes.Rem_Un);
		}

		public FluentEmitter Add()
		{
			return Emit(OpCodes.Add);
		}

		public FluentEmitter Add(MagicNumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Add);
		}

		public FluentEmitter Sub()
		{
			return Emit(OpCodes.Sub);
		}

		public FluentEmitter Sub(MagicNumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Sub);
		}

		public FluentEmitter Mul()
		{
			return Emit(OpCodes.Mul);
		}

		public FluentEmitter Mul(MagicNumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Mul);
		}

		public FluentEmitter Div()
		{
			return Emit(OpCodes.Div);
		}

		public FluentEmitter Div(MagicNumberArgument divisor)
		{
			if (divisor.IsZero)
				throw new DivideByZeroException("this instruction will cause a DivideByZeroException");

			return divisor.EmitLdc(this)
				.Emit(OpCodes.Div);
		}

		public FluentEmitter DivUn()
		{
			return Emit(OpCodes.Div_Un);
		}

		public FluentEmitter DivUn(MagicNumberArgument divisor)
		{
			if (divisor.IsZero)
				throw new DivideByZeroException("this instruction will cause a DivideByZeroException");

			return divisor.EmitLdc(this)
				.Emit(OpCodes.Div_Un);
		}
	}
}
