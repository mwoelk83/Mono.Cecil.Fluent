using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody ConvI()
		{
			return Emit(OpCodes.Conv_I);
		}

		public FluentMethodBody ConvI1()
		{
			return Emit(OpCodes.Conv_I1);
		}

		public FluentMethodBody ConvI2()
		{
			return Emit(OpCodes.Conv_I2);
		}

		public FluentMethodBody ConvI4()
		{
			return Emit(OpCodes.Conv_I4);
		}

		public FluentMethodBody ConvI8()
		{
			return Emit(OpCodes.Conv_I8);
		}

		public FluentMethodBody ConvU()
		{
			return Emit(OpCodes.Conv_U);
		}

		public FluentMethodBody ConvU1()
		{
			return Emit(OpCodes.Conv_U1);
		}

		public FluentMethodBody ConvU2()
		{
			return Emit(OpCodes.Conv_U2);
		}

		public FluentMethodBody ConvU4()
		{
			return Emit(OpCodes.Conv_U4);
		}

		public FluentMethodBody ConvU8()
		{
			return Emit(OpCodes.Conv_U8);
		}

		public FluentMethodBody ConvR4()
		{
			return Emit(OpCodes.Conv_R4);
		}

		public FluentMethodBody ConvR8()
		{
			return Emit(OpCodes.Conv_R8);
		}
	}
}