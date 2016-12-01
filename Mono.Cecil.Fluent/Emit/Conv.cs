using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class FluentEmitter
	{
		public FluentEmitter ConvI()
		{
			return Emit(OpCodes.Conv_I);
		}

		public FluentEmitter ConvI1()
		{
			return Emit(OpCodes.Conv_I1);
		}

		public FluentEmitter ConvI2()
		{
			return Emit(OpCodes.Conv_I2);
		}

		public FluentEmitter ConvI4()
		{
			return Emit(OpCodes.Conv_I4);
		}

		public FluentEmitter ConvI8()
		{
			return Emit(OpCodes.Conv_I8);
		}

		public FluentEmitter ConvU()
		{
			return Emit(OpCodes.Conv_U);
		}

		public FluentEmitter ConvU1()
		{
			return Emit(OpCodes.Conv_U1);
		}

		public FluentEmitter ConvU2()
		{
			return Emit(OpCodes.Conv_U2);
		}

		public FluentEmitter ConvU4()
		{
			return Emit(OpCodes.Conv_U4);
		}

		public FluentEmitter ConvU8()
		{
			return Emit(OpCodes.Conv_U8);
		}

		public FluentEmitter ConvR4()
		{
			return Emit(OpCodes.Conv_R4);
		}

		public FluentEmitter ConvR8()
		{
			return Emit(OpCodes.Conv_R8);
		}
	}
}