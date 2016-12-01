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

		internal override FluentEmitter EmitLdc(FluentEmitter method)
		{
			return method.Emit(Number ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
		}

		internal override FluentEmitter EmitLdcI4(FluentEmitter method)
		{
			return EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcI8(FluentEmitter method)
		{
			return new MagicNumberArgumentI8(Number ? 1 : 0).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcR4(FluentEmitter method)
		{
			return new MagicNumberArgumentR4(Number ? 1.0f : 0.0f).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcR8(FluentEmitter method)
		{
			return new MagicNumberArgumentR8(Number ? 1.0d : 0.0d).EmitLdc(method);
		}
	}
}
