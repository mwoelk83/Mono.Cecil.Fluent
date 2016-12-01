using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class FluentEmitter
	{
		public FluentEmitter LdStr(params string[] args)
		{
			foreach (var arg in args)
				Emit(OpCodes.Ldstr, arg);
			return this;
		}

		public FluentEmitter Ldc(params MagicNumberArgument[] args)
		{
			foreach (var arg in args)
				arg.EmitLdc(this);
			return this;
		}
	}
}
