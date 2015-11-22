using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Ret()
		{
			return Emit(OpCodes.Ret);
		}

		public FluentMethodBody Ret(MagicNumberArgument Value)
		{
			switch (ReturnType.GetILType())
			{
				case ILType.I4: Value.EmitLdcI4(this); break;
				case ILType.I8: Value.EmitLdcI8(this); break;
				case ILType.R8: Value.EmitLdcR8(this); break;
				case ILType.R4: Value.EmitLdcR4(this); break;
				default:
					throw new NotSupportedException( // ncrunch: no coverage
						"return value type must be primitive valuetype in system namespace and convertible to I4, I8, R4 or R8");
			}
			return Ret();
		}
	}
}