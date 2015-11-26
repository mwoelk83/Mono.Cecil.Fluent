using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Rem()
		{
			return Emit(OpCodes.Rem);
		}
		public FluentMethodBody RemUn()
		{
			return Emit(OpCodes.Rem_Un);
		}

		public FluentMethodBody Rem(MagicNumberArgument divisor)
		{
			return divisor.EmitLdc(this)
				.Emit(OpCodes.Rem);
		}

		public FluentMethodBody RemUn(MagicNumberArgument divisor)
		{
			return divisor.EmitLdc(this)
				.Emit(OpCodes.Rem_Un);
		}

		public FluentMethodBody Add()
		{
			return Emit(OpCodes.Add);
		}

		public FluentMethodBody Add(MagicNumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Add);
		}

		public FluentMethodBody Sub()
		{
			return Emit(OpCodes.Sub);
		}

		public FluentMethodBody Sub(MagicNumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Sub);
		}

		public FluentMethodBody Mul()
		{
			return Emit(OpCodes.Mul);
		}

		public FluentMethodBody Mul(MagicNumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Mul);
		}

		public FluentMethodBody Div()
		{
			return Emit(OpCodes.Div);
		}

		public FluentMethodBody Div(MagicNumberArgument divisor)
		{
			if (divisor.IsZero)
				throw new DivideByZeroException(); //ncrunch: no coverage

			return divisor.EmitLdc(this)
				.Emit(OpCodes.Div);
		}

		public FluentMethodBody DivUn()
		{
			return Emit(OpCodes.Div_Un);
		}

		public FluentMethodBody DivUn(MagicNumberArgument divisor)
		{
			if (divisor.IsZero)
				throw new DivideByZeroException(); //ncrunch: no coverage

			return divisor.EmitLdc(this)
				.Emit(OpCodes.Div_Un);
		}
	}
}
