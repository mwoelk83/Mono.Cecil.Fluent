using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Ret()
		{
			return Emit(OpCodes.Ret);
		}

		public FluentMethodBody Ret(MagicNumberArgument Value)
		{
			switch (ReturnType.GetILType())
			{
				case ILType.I4: Value.EmitLdcI4(this); break;
				case ILType.I8: Value.EmitLdcI8(this); break;
				case ILType.R8: Value.EmitLdcR8(this); break;
				case ILType.R4: Value.EmitLdcR4(this); break;
				default:
					throw new NotSupportedException( // ncrunch: no coverage
						"return value type must be primitive valuetype in system namespace and convertible to I4, I8, R4 or R8");
			}
			return Ret();
		}

		public FluentMethodBody RetLoc(string varname)
		{
			var var = GetVariable(varname);

			if (var.VariableType.GetILType() != ReturnType.GetILType())
				throw new InvalidOperationException("variable type and return type must be oft same type"); // ncrunch: no coverage
			
			// todo: better check, e.g. for inherited classes or generics
			if (var.VariableType.GetILType() == ILType.Object || var.VariableType.GetILType() == ILType.ValueType)
				if (var.VariableType.FullName != ReturnType.FullName)                                           // ncrunch: no coverage
					throw new InvalidOperationException("variable type and return type must be oft same type"); // ncrunch: no coverage

			return Ldloc((uint)var.Index)
				.Ret();
		}

		public FluentMethodBody RetLoc(uint varindex)
		{
			if (varindex >= Variables.Count)
				throw new ArgumentException($"no variable at index {varindex}"); // ncrunch: no coverage

			var var = Variables[(int)varindex];

			if (var.VariableType.GetILType() != ReturnType.GetILType())
				throw new InvalidOperationException("variable type and return type must be oft same type"); // ncrunch: no coverage

			// todo: better check, e.g. for inherited classes or generics
			if (var.VariableType.GetILType() == ILType.Object || var.VariableType.GetILType() == ILType.ValueType)
				if (var.VariableType.FullName != ReturnType.FullName)                                           // ncrunch: no coverage
					throw new InvalidOperationException("variable type and return type must be oft same type"); // ncrunch: no coverage

			return Ldloc((uint)var.Index)
				.Ret();
		}
		
		public FluentMethodBody RetThis()
		{
			if (DeclaringType.GetILType() != ReturnType.GetILType())
				throw new InvalidOperationException("declaring type and return type must be oft same type"); // ncrunch: no coverage

			// todo: better check, e.g. for inherited classes or generics
			if (DeclaringType.GetILType() == ILType.Object || DeclaringType.GetILType() == ILType.ValueType)
				if (DeclaringType.FullName != ReturnType.FullName)												// ncrunch: no coverage
					throw new InvalidOperationException("declaring type and return type must be oft same type");// ncrunch: no coverage

			return LdThis()
				.Ret();
		}

		public FluentMethodBody RetArg(string varname)
		{
			var arg = GetParameter(varname);

			if (arg.ParameterType.GetILType() != ReturnType.GetILType())
				throw new InvalidOperationException("parameter type and return type must be oft same type"); // ncrunch: no coverage
			
			// todo: better check, e.g. for inherited classes or generics
			if (arg.ParameterType.GetILType() == ILType.Object || arg.ParameterType.GetILType() == ILType.ValueType)
				if (arg.ParameterType.FullName != ReturnType.FullName)                                           // ncrunch: no coverage
					throw new InvalidOperationException("parameter type and return type must be oft same type"); // ncrunch: no coverage

			return Ldarg((uint)arg.Index)
				.Ret();
		}
	}
}