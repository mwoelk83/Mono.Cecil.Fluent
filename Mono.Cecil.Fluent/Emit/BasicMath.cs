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

		public FluentMethodBody Rem(NumberArgument divisor)
		{
			if (divisor.Number.IsZero())
				throw new DivideByZeroException();

			return divisor.EmitLdc(this)
				.Emit(OpCodes.Rem);
		}

		public FluentMethodBody RemUn(NumberArgument divisor)
		{
			if (divisor.Number.IsZero())
				throw new DivideByZeroException();

			return divisor.EmitLdc(this)
				.Emit(OpCodes.Rem_Un);
		}

		public FluentMethodBody Add()
		{
			return Emit(OpCodes.Add);
		}

		public FluentMethodBody Add(NumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Add);
		}

		public FluentMethodBody Sub()
		{
			return Emit(OpCodes.Sub);
		}

		public FluentMethodBody Sub(NumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Sub);
		}

		public FluentMethodBody Mul()
		{
			return Emit(OpCodes.Mul);
		}

		public FluentMethodBody Mul(NumberArgument arg)
		{
			return arg.EmitLdc(this)
				.Emit(OpCodes.Mul);
		}

		public FluentMethodBody Div()
		{
			return Emit(OpCodes.Div);
		}

		public FluentMethodBody Div(NumberArgument divisor)
		{
			if (divisor.Number.IsZero())
				throw new DivideByZeroException();

			return divisor.EmitLdc(this)
				.Emit(OpCodes.Div);
		}

		public FluentMethodBody DivUn()
		{
			return Emit(OpCodes.Div_Un);
		}

		public FluentMethodBody DivUn(NumberArgument divisor)
		{
			if (divisor.Number.IsZero())
				throw new DivideByZeroException();

			return divisor.EmitLdc(this)
				.Emit(OpCodes.Div_Un);
		}
	}
}