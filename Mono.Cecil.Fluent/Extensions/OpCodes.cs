using System.Collections.Generic;
using System.Reflection.Emit;

namespace Mono.Cecil.Fluent
{
	public static partial class CecilExtensions
    {
        private static readonly Dictionary<string, OpCode> SystemOpCodes = new Dictionary<string, OpCode>();

		static CecilExtensions()
		{
			var fields = typeof(OpCodes).GetFields();
			foreach (var field in fields)
			{
				SystemOpCodes.Add(field.Name.ToLower().Replace('_', '.'), (OpCode)field.GetValue(null));
			}
		}

		public static OpCode ToSystem(this Cil.OpCode that)
		{
			return SystemOpCodes[that.Name];
		}
	}
}
