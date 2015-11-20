using System;
using System.Globalization;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	public class NumberArgument
	{
		internal readonly object Number;
		internal readonly bool IsFP;
		internal readonly bool Is64Bit;
		internal readonly bool IsUnsigned;

		internal NumberArgument(object num, bool is64bit, bool isfp, bool isunsigned)
		{
			Number = num;
			IsFP = isfp;
			Is64Bit = is64bit;
			IsUnsigned = isunsigned;
		}

		internal FluentMethodBody EmitLdc(FluentMethodBody method)
		{
			if (IsFP)
			{
				return Is64Bit
					? method.Emit(OpCodes.Ldc_R8, (double) Number)
					: method.Emit(OpCodes.Ldc_R4, (float) Number);
			}

			if (Is64Bit && !IsUnsigned)
				return method.Emit(OpCodes.Ldc_I8, (long)Number);
			if (Is64Bit)
				return method.Emit(OpCodes.Ldc_I8, ((IConvertible) Number).ToInt64(CultureInfo.InvariantCulture));

			var snum = ((IConvertible) Number).ToInt64(CultureInfo.InvariantCulture);

			if (snum < sbyte.MinValue || snum > sbyte.MaxValue)
			{
				return Is64Bit
					? method.Emit(OpCodes.Ldc_I8, ((IConvertible)Number).ToInt64(CultureInfo.InvariantCulture))
					: method.Emit(OpCodes.Ldc_I4, ((IConvertible)Number).ToInt32(CultureInfo.InvariantCulture));
			}

			switch (snum)
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

			return method.Emit(OpCodes.Ldc_I4_S, (sbyte) snum);
		}

		public static implicit operator NumberArgument(byte val)
		{
			return new NumberArgument(val, false, false, true);
		}

		public static implicit operator NumberArgument(bool val)
		{
			return new NumberArgument(val, false, false, true);
		}

		public static implicit operator NumberArgument(sbyte val)
		{
			return new NumberArgument(val, false, false, false);
		}

		public static implicit operator NumberArgument(short val)
		{
			return new NumberArgument(val, false, false, false);
		}

		public static implicit operator NumberArgument(ushort val)
		{
			return new NumberArgument(val, false, false, true);
		}

		public static implicit operator NumberArgument(int val)
		{
			return new NumberArgument(val, false, false, false);
		}

		public static implicit operator NumberArgument(uint val)
		{
			return new NumberArgument(val, false, false, true);
		}

		public static implicit operator NumberArgument(long val)
		{
			return new NumberArgument(val, true, false, false);
		}

		public static implicit operator NumberArgument(ulong val)
		{
			return new NumberArgument(val, true, false, true);
		}

		public static implicit operator NumberArgument(float val)
		{
			return new NumberArgument(val, false, true, false);
		}

		public static implicit operator NumberArgument(double val)
		{
			return new NumberArgument(val, true, true, false);
		}
	}
}
