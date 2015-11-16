using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody LdStr(params string[] args)
		{
			foreach (var arg in args)
				Emit(OpCodes.Ldstr, arg);
			return this;
		}
	}
}
