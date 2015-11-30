using System.Linq;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		/// <summary>
		/// Computes the absolute Value of the 32 bit signed integer on top of stack. 
		/// This value should never be Int32.MinValue. Otherwise it will not throw an 
		/// exception but the result would stay Int32.MinValue in this case. 
		/// 
		/// The implementation use no branches. It computes the result with formular
		/// (x ^ (x >> 31)) - (x >> 31)
		/// </summary>
		/// <returns></returns>
		public FluentMethodBody AbsI4()
		{
			if(Variables.All(v => v.Name != "<>___absmaski32"))
				this.WithVariable<int>("<>___absmaski32");

											// stack
			return Dup()					// n, n
				.Ldc(31)					// n, n, 31
				.Emit(OpCodes.Shr)			// n, mask
				.Dup()						// n, mask, mask
				.Stloc("<>___absmaski32")	// n, mask
				.Emit(OpCodes.Xor)			// (x ^ mask)
				.Ldloc("<>___absmaski32")	// (x ^ mask), mask
				.Sub();						// (x ^ mask) - mask
		}
		
		/// <summary>
		/// Computes the absolute Value of the 64 bit signed integer on top of stack. 
		/// This value should never be Int64.MinValue. Otherwise it will not throw an 
		/// exception but the result would stay Int64.MinValue in this case. 
		/// 
		/// The implementation use no branches. It computes the result with formular
		/// (x ^ (x >> 31)) - (x >> 31)
		/// </summary>
		public FluentMethodBody AbsI8()
		{
			if (Variables.All(v => v.Name != "<>___absmaski64"))
				this.WithVariable<long>("<>___absmaski64");

											// stack
			return Dup()					// n, n
				.Ldc(63)					// n, n, 63
				.Emit(OpCodes.Shr)			// n, mask
				.Dup()						// n, mask, mask
				.Stloc("<>___absmaski64")	// n, mask
				.Emit(OpCodes.Xor)			// (x ^ mask)
				.Ldloc("<>___absmaski64")	// (x ^ mask), mask
				.Sub();						// (x ^ mask) - mask
		}

		public FluentMethodBody AbsR4()
		{
			return Dup()
				.Ldc(0.0f)
				.Iflt()
				.Neg()
				.EndIf();
		}

		public FluentMethodBody AbsR8()
		{
			return Dup()
				.Ldc(0.0d)
				.Iflt()
				.Neg()
				.EndIf();
		}
	}
}
