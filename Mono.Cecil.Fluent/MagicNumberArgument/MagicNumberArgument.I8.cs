using System;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	internal sealed class MagicNumberArgumentI8 : MagicNumberArgument
	{
		internal readonly long Number;

		internal override bool IsZero => Number == 0L;

		internal MagicNumberArgumentI8(long num) : base(true, false, false)
		{
			Number = num;
		}

		internal MagicNumberArgumentI8(ulong num) : base(true, false, true)
		{
			Number = unchecked((int)num);
		}

		internal override FluentEmitter EmitLdc(FluentEmitter method)
		{
			return method.Emit(OpCodes.Ldc_I8, Number);
		}

		internal override FluentEmitter EmitLdcI4(FluentEmitter method)
		{
			if(Number > int.MaxValue || Number < int.MinValue)
				throw new OverflowException("can not convert long or ulong to int32. number is greater than Int32.MaxValue or smaller than Int32.MinValue");
			return new MagicNumberArgumentI4(unchecked((int)Number)).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcI8(FluentEmitter method)
		{
			return EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcR4(FluentEmitter method)
		{
			return new MagicNumberArgumentR4(Number).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcR8(FluentEmitter method)
		{
			return new MagicNumberArgumentR8(Number).EmitLdc(method);
		}
	}
}
