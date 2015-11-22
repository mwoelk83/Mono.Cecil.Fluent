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

		internal override FluentMethodBody EmitLdc(FluentMethodBody method)
		{
			return method.Emit(OpCodes.Ldc_I8, Number);
		}

		internal override FluentMethodBody EmitLdcI4(FluentMethodBody method)
		{
			if(Number > int.MaxValue || Number < int.MinValue)
				throw new OverflowException("can not convert long or ulong to int32. number is greater than Int32.MaxValue or smaller than Int32.MinValue");
			return new MagicNumberArgumentI4(unchecked((int)Number)).EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcI8(FluentMethodBody method)
		{
			return EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcR4(FluentMethodBody method)
		{
			return new MagicNumberArgumentR4(Number).EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcR8(FluentMethodBody method)
		{
			return new MagicNumberArgumentR8(Number).EmitLdc(method);
		}
	}
}
