using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	internal sealed class MagicNumberArgumentI4 : MagicNumberArgument
	{
		internal readonly int Number;

		internal override bool IsZero => Number == 0;

		internal MagicNumberArgumentI4(int num) : base(false, false, false)
		{
			Number = num;
		}

		internal MagicNumberArgumentI4(uint num) : base(false, false, true)
		{
			Number = unchecked((int) num);
		}

		internal override FluentEmitter EmitLdc(FluentEmitter method)
		{
			switch (Number)
			{
				case -1: return method.Emit(OpCodes.Ldc_I4_M1);
				case 0: return method.Emit(OpCodes.Ldc_I4_0);
				case 1: return method.Emit(OpCodes.Ldc_I4_1);
				case 2: return method.Emit(OpCodes.Ldc_I4_2);
				case 3: return method.Emit(OpCodes.Ldc_I4_3);
				case 4: return method.Emit(OpCodes.Ldc_I4_4);
				case 5: return method.Emit(OpCodes.Ldc_I4_5);
				case 6: return method.Emit(OpCodes.Ldc_I4_6);
				case 7: return method.Emit(OpCodes.Ldc_I4_7);
				case 8: return method.Emit(OpCodes.Ldc_I4_8);
			}

			if (Number < sbyte.MinValue || Number > sbyte.MaxValue)
				return method.Emit(OpCodes.Ldc_I4, Number);

			return method.Emit(OpCodes.Ldc_I4_S, (sbyte) Number);
		}

		internal override FluentEmitter EmitLdcI4(FluentEmitter method)
		{
			return EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcI8(FluentEmitter method)
		{
			return new MagicNumberArgumentI8(Number).EmitLdc(method);
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
