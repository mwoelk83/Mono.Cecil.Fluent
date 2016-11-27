using System;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
	    private static void CheckTypeEquality(TypeReference a, TypeReference b)
        {
            // todo: better check, e.g. for inherited classes or generics
            if (!a.SafeEquals(b))
                throw new InvalidOperationException("return type does not match"); // ncrunch: no coverage
        }

		public FluentMethodBody Ret()
		{
			return Emit(OpCodes.Ret);
		}

		public FluentMethodBody Ret(MagicNumberArgument value)
		{
		    // ReSharper disable once SwitchStatementMissingSomeCases
			switch (ReturnType.GetILType())
			{
				case ILType.I4: value.EmitLdcI4(this); break;
				case ILType.I8: value.EmitLdcI8(this); break;
				case ILType.R8: value.EmitLdcR8(this); break;
				case ILType.R4: value.EmitLdcR4(this); break;
				default:
					throw new NotSupportedException( // ncrunch: no coverage
						"returned type must be primitive valuetype and convertible to I4, I8, R4 or R8");
			}
			return Ret();
		}

		public FluentMethodBody RetLoc(string varname)
		{
            return RetLoc((uint)GetVariable(varname).Index);
		}

		public FluentMethodBody RetLoc(uint varindex)
		{
			if (varindex >= Variables.Count)
				throw new ArgumentException($"no variable at index {varindex}"); // ncrunch: no coverage

			var var = Variables[(int)varindex];

            CheckTypeEquality(ReturnType, var.VariableType);

			return Ldloc((uint)var.Index)
				.Ret();
		}
		
		public FluentMethodBody RetThis()
        {
            CheckTypeEquality(ReturnType, DeclaringType);

            return LdThis()
				.Ret();
		}

		public FluentMethodBody RetArg(string varname)
		{
			var arg = GetParameter(varname);

            CheckTypeEquality(ReturnType, arg.ParameterType);

            return Ldarg((uint)arg.Index)
				.Ret();
		}
	}
}