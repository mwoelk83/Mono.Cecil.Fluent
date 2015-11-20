using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Ret()
		{
			return Emit(OpCodes.Ret);
		}

		//public FluentMethodBody Ret(bool value)
		//{
		//	var allowedTypes = new List<string>
		//	{
		//		typeof (bool).FullName,
		//		typeof (byte).FullName,
		//		typeof (sbyte).FullName,
  //              typeof (short).FullName,
		//		typeof (ushort).FullName,
  //              typeof (int).FullName,
		//		typeof (uint).FullName,
		//		typeof (long).FullName,
		//		typeof (ulong).FullName,
		//	};

		//	if(allowedTypes.All(t => t != ReturnType.FullName))
		//		throw new NotSupportedException("cannot return bool for this return type");

		//	return Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0)
		//		.Emit(OpCodes.Ret);
		//}
	}
}