using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public DynamicMethod ToDynamicMethod()
		{
			return ToDynamicMethod(null) as DynamicMethod;
		}

	    public MethodInfo ToDynamicMethod(MethodInfo method)
	    {
	        return ToDynamicMethod(null, method);
	    }

	    public MethodInfo ToDynamicMethod(TypeBuilder declaringtype, MethodInfo method = null)
		{
			// todo: exception blocks, scopes ...
			if(!MethodDefinition.IsStatic && declaringtype == null)
				throw new InvalidOperationException("only static methods can be converted to dynamic methods"); // ncrunch: no coverage

			var returntype = TypeLoader.Instance.Load(ReturnType);
			if(returntype == null)
				throw new InvalidOperationException($"can not find return type {ReturnType.FullName} in current appdomain"); // ncrunch: no coverage

			var paramtypes = new List<Type>();
			foreach (var param in Parameters)
			{
				var paramtype = TypeLoader.Instance.Load(param.ParameterType);
				if (paramtype == null)
					throw new InvalidOperationException($"can not find parameter type {param.ParameterType.FullName} in current appdomain"); // ncrunch: no coverage
				paramtypes.Add(paramtype);
			}
			
			if(method == null)
				method = new DynamicMethod(Name, returntype, paramtypes.ToArray()) { InitLocals = true };

			ILGenerator ilgen;

			if (method is MethodBuilder)
				ilgen = ((MethodBuilder)method).GetILGenerator();
			else
				ilgen = ((DynamicMethod)method).GetILGenerator();

			var parameters = new List<ParameterBuilder>();
			foreach (var p in Parameters)
			{
				var paramtype = TypeLoader.Instance.Load(p.ParameterType);
				if(paramtype == null)
					throw new InvalidOperationException($"can not find parameter type {p.ParameterType.FullName} in current appdomain"); // ncrunch: no coverage
				var param = method is MethodBuilder 
					? ((MethodBuilder)method).DefineParameter(p.Index, (System.Reflection.ParameterAttributes) p.Attributes, p.Name)
					: ((DynamicMethod)method).DefineParameter(p.Index, (System.Reflection.ParameterAttributes)p.Attributes, p.Name);
				parameters.Add(param);
			}
			var locals = new List<LocalBuilder>();

			foreach (var var in Variables)
			{
				var vartype = TypeLoader.Instance.Load(var.VariableType);
				if(vartype == null)
					throw new InvalidOperationException($"can not find variable type {var.VariableType.FullName} in current appdomain"); // ncrunch: no coverage
				locals.Add(ilgen.DeclareLocal(vartype));
			}

			var labelstomark = new Dictionary<int, string>(); // key is instruction index, value is labelname
			var labels = new Dictionary<int, Label>();

			foreach (var instr in Body.Instructions.Where(i => i.Operand is Instruction))
			{	try
				{
					labels.Add(Body.Instructions.IndexOf((Instruction) instr.Operand), ilgen.DefineLabel());
				}
				catch (ArgumentException) { }
			}

			var index = 0;
			foreach (var instruction in Body.Instructions)
			{
				if(labels.ContainsKey(index))
					ilgen.MarkLabel(labels[index]);

				var opcode = instruction.OpCode.ToSystem();
				if (instruction.Operand == null)
					ilgen.Emit(opcode);
				else if (instruction.Operand is VariableReference)
					ilgen.Emit(opcode, locals[((VariableReference)instruction.Operand).Index]);
				else if (instruction.Operand is ParameterReference)
					ilgen.Emit(opcode, ((ParameterReference)instruction.Operand).Index);
				else if (instruction.Operand is int)
					ilgen.Emit(opcode, (int)instruction.Operand);
				else if (instruction.Operand is long)
					ilgen.Emit(opcode, (long)instruction.Operand);
				else if (instruction.Operand is string)
					ilgen.Emit(opcode, (string)instruction.Operand);
				else if (instruction.Operand is MethodReference && ((MethodReference)instruction.Operand).Name != ".ctor")
				{
					var operand = TypeLoader.Instance.Load((MethodReference) instruction.Operand);
					if (operand == null)
						throw new InvalidOperationException($"can not find operand method {instruction.Operand} in current appdomain"); // ncrunch: no coverage
					ilgen.Emit(opcode, operand);
                }
                else if (instruction.Operand is MethodReference) // constructor
                {
                    var operand = TypeLoader.Instance.LoadConstructor((MethodReference)instruction.Operand);
                    if (operand == null)
                        throw new InvalidOperationException($"can not find operand method {instruction.Operand} in current appdomain"); // ncrunch: no coverage
                    ilgen.Emit(opcode, operand);
                }
                else if (instruction.Operand is FieldReference)
				{
					var operand = TypeLoader.Instance.Load((FieldReference) instruction.Operand);
					if (operand == null)
						throw new InvalidOperationException($"can not find operand field {instruction.Operand} in current appdomain"); // ncrunch: no coverage
					ilgen.Emit(opcode, operand);
				}
				else if (instruction.Operand is TypeReference)
				{
					var operand = TypeLoader.Instance.Load((TypeReference) instruction.Operand);
					if(operand == null)
						throw new InvalidOperationException($"can not find operand type {instruction.Operand} in current appdomain"); // ncrunch: no coverage
					ilgen.Emit(opcode, operand);
				}
				else if (instruction.Operand is byte)
					ilgen.Emit(opcode, (byte)instruction.Operand);
				else if (instruction.Operand is sbyte)
					ilgen.Emit(opcode, (sbyte)instruction.Operand);
				else if (instruction.Operand is short)
					ilgen.Emit(opcode, (short)instruction.Operand);
				else if (instruction.Operand is ushort)
					ilgen.Emit(opcode, (ushort)instruction.Operand);
				else if (instruction.Operand is uint)
					ilgen.Emit(opcode, (uint)instruction.Operand);
				else if (instruction.Operand is ulong)
					ilgen.Emit(opcode, (ulong)instruction.Operand);
				else if (instruction.Operand is float)
					ilgen.Emit(opcode, (float)instruction.Operand);
				else if (instruction.Operand is double)
					ilgen.Emit(opcode, (double)instruction.Operand);
				else if (instruction.Operand is Instruction)
					ilgen.Emit(opcode, labels[Body.Instructions.IndexOf((Instruction)instruction.Operand)]);
				else 
					throw new NotImplementedException(instruction.Operand.GetType().FullName);

				++index;
			}

			return method;
		}
	}
}
