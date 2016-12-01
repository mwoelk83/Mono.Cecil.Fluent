using System;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	internal sealed class MagicNumberArgumentR8 : MagicNumberArgument
	{
		internal readonly double Number;

		internal override bool IsZero => double.IsNaN(Number) || Math.Abs(Number) < double.Epsilon;

		internal MagicNumberArgumentR8(double num) : base(true, true, false)
		{
			Number = num;
		}

		internal override FluentEmitter EmitLdc(FluentEmitter method)
		{
			return method.Emit(OpCodes.Ldc_R8, Number);
		}

		internal override FluentEmitter EmitLdcI4(FluentEmitter method)
		{
			return new MagicNumberArgumentI4((int) Number).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcI8(FluentEmitter method)
		{
			return new MagicNumberArgumentI8((long) Number).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcR4(FluentEmitter method)
		{
			return new MagicNumberArgumentR4((float) Number).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcR8(FluentEmitter method)
		{
			return EmitLdc(method);
		}
	}
}
