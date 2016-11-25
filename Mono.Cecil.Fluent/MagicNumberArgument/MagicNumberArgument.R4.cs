using System;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	internal sealed class MagicNumberArgumentR4 : MagicNumberArgument
	{
		internal readonly float Number;

		internal override bool IsZero => float.IsNaN(Number) || Math.Abs(Number) < float.Epsilon;

		internal MagicNumberArgumentR4(float num) : base(false, true, false)
		{
			Number = num;
		}

		internal override FluentMethodBody EmitLdc(FluentMethodBody method)
		{
			return method.Emit(OpCodes.Ldc_R4, Number);
		}

		internal override FluentMethodBody EmitLdcI4(FluentMethodBody method)
		{
			return new MagicNumberArgumentI4((int) Number).EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcI8(FluentMethodBody method)
		{
			return new MagicNumberArgumentI8((long) Number).EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcR4(FluentMethodBody method)
		{
			return EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcR8(FluentMethodBody method)
		{
			return new MagicNumberArgumentR8(Number).EmitLdc(method);
		}
	}
}
