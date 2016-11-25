
namespace Mono.Cecil.Fluent
{
	internal enum ILType : byte { Object, String, ValueType, I4, I8, R4, R8 }

	internal static class GetILTypeExtension
	{
		public static ILType GetILType(this TypeReference type)
		{
			if(!type.IsValueType)
				return ILType.Object;
			switch (type.FullName)
			{
				case "System.String": return ILType.String;
				case "System.Boolean":
				case "System.Int16":
				case "System.UInt16": 
				case "System.Byte": 
				case "System.SByte":
				case "System.Int32":
				case "System.UInt32": return ILType.I4;
				case "System.Int64":
				case "System.UInt64": return ILType.I8;
				case "System.Single": return ILType.R4;
				case "System.Double": return ILType.R8;
			}
			return ILType.ValueType;
		}
	}
}
