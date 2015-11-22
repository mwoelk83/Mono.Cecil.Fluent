using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	internal sealed class MagicNumberArgumentBool : MagicNumberArgument
	{
		internal readonly bool Number;

		internal override bool IsZero => Number == false;

		internal MagicNumberArgumentBool(bool Value) : base(false, false, true)
		{
			Number = Value;
		}

		internal override FluentMethodBody EmitLdc(FluentMethodBody method)
		{
			return method.Emit(Number ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
		}

		internal override FluentMethodBody EmitLdcI4(FluentMethodBody method)
		{
			return EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcI8(FluentMethodBody method)
		{
			return new MagicNumberArgumentI8(Number ? 1 : 0).EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcR4(FluentMethodBody method)
		{
			return new MagicNumberArgumentR4(Number ? 1.0f : 0.0f).EmitLdc(method);
		}

		internal override FluentMethodBody EmitLdcR8(FluentMethodBody method)
		{
			return new MagicNumberArgumentR8(Number ? 1.0d : 0.0d).EmitLdc(method);
		}
	}
}
