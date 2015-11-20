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

		public FluentMethodBody Ldc(params NumberArgument[] args)
		{
			foreach (var arg in args)
				arg.EmitLdc(this);
			return this;
		}
	}
}
