
// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public abstract class MagicNumberArgument
	{
		internal readonly bool IsFloatingPoint;
		internal readonly bool Is64Bit;
		internal readonly bool IsUnsigned;

		internal abstract bool IsZero { get; }

		internal MagicNumberArgument(bool is64Bit, bool isfp, bool isunsigned)
		{
			IsFloatingPoint = isfp;
			Is64Bit = is64Bit;
			IsUnsigned = isunsigned;
		}

		internal abstract FluentEmitter EmitLdc(FluentEmitter method);
		internal abstract FluentEmitter EmitLdcI4(FluentEmitter method);
		internal abstract FluentEmitter EmitLdcI8(FluentEmitter method);
		internal abstract FluentEmitter EmitLdcR4(FluentEmitter method);
		internal abstract FluentEmitter EmitLdcR8(FluentEmitter method);

		public static implicit operator MagicNumberArgument(byte val)
		{
			return new MagicNumberArgumentI4((uint) val); // force unsigned
		}

		public static implicit operator MagicNumberArgument(bool val)
		{
			return new MagicNumberArgumentBool(val);
		}

		public static implicit operator MagicNumberArgument(sbyte val)
		{
			return new MagicNumberArgumentI4(val);
		}

		public static implicit operator MagicNumberArgument(short val)
		{
			return new MagicNumberArgumentI4(val);
		}

		public static implicit operator MagicNumberArgument(ushort val)
		{
			return new MagicNumberArgumentI4((uint)val); // force unsigned
		}

		public static implicit operator MagicNumberArgument(int val)
		{
			return new MagicNumberArgumentI4(val);
		}

		public static implicit operator MagicNumberArgument(uint val)
		{
			return new MagicNumberArgumentI4(val);
		}

		public static implicit operator MagicNumberArgument(long val)
		{
			return new MagicNumberArgumentI8(val);
		}

		public static implicit operator MagicNumberArgument(ulong val)
		{
			return new MagicNumberArgumentI8(val);
		}

		public static implicit operator MagicNumberArgument(float val)
		{
			return new MagicNumberArgumentR4(val);
		}

		public static implicit operator MagicNumberArgument(double val)
		{
			return new MagicNumberArgumentR8(val);
		}
	}
}
