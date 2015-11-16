using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Ret
		{
			get
			{
				Emit(OpCodes.Ret);
				return this;
			}
		}

		public FluentMethodBody Return(bool value)
		{
			Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
			Emit(OpCodes.Ret);
			return this;
		}
	}
}