using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
		public static DynamicMethod ToDynamicMethod(this MethodDefinition method)
		{
			return method.ToDynamicMethod(null) as DynamicMethod;
		}

	    public static MethodInfo ToDynamicMethod(this MethodDefinition method, MethodInfo target)
	    {
	        return ToDynamicMethod(method, null, target);
	    }

	    public static MethodInfo ToDynamicMethod(this MethodDefinition method, TypeBuilder declaringtype, MethodInfo target = null)
		{
			// todo: exception blocks, scopes ...
			if(!method.IsStatic && declaringtype == null)
				throw new InvalidOperationException("only static methods can be converted to dynamic methods"); // ncrunch: no coverage

			var returntype = TypeLoader.Instance.Load(method.ReturnType);
			if(returntype == null)
				throw new InvalidOperationException($"can not find return type {method.ReturnType.FullName} in current appdomain"); // ncrunch: no coverage

			var paramtypes = new List<Type>();
			foreach (var param in method.Parameters)
			{
				var paramtype = TypeLoader.Instance.Load(param.ParameterType);
				if (paramtype == null)
					throw new InvalidOperationException($"can not find parameter type {param.ParameterType.FullName} in current appdomain"); // ncrunch: no coverage
				paramtypes.Add(paramtype);
			}
			
			if(target == null)
				target = new DynamicMethod(method.Name, returntype, paramtypes.ToArray()) { InitLocals = true };

			ILGenerator ilgen;

			if (target is MethodBuilder)
				ilgen = ((MethodBuilder)target).GetILGenerator();
			else
				ilgen = ((DynamicMethod)target).GetILGenerator();

			var parameters = new List<ParameterBuilder>();
			foreach (var p in method.Parameters)
			{
				var paramtype = TypeLoader.Instance.Load(p.ParameterType);
				if(paramtype == null)
					throw new InvalidOperationException($"can not find parameter type {p.ParameterType.FullName} in current appdomain"); // ncrunch: no coverage
				var param = target is MethodBuilder 
					? ((MethodBuilder)target).DefineParameter(p.Index, (System.Reflection.ParameterAttributes) p.Attributes, p.Name)
					: ((DynamicMethod)target).DefineParameter(p.Index, (System.Reflection.ParameterAttributes)p.Attributes, p.Name);
				parameters.Add(param);
			}
			var locals = new List<LocalBuilder>();

			foreach (var var in method.Body.Variables)
			{
				var vartype = TypeLoader.Instance.Load(var.VariableType);
				if(vartype == null)
					throw new InvalidOperationException($"can not find variable type {var.VariableType.FullName} in current appdomain"); // ncrunch: no coverage
				locals.Add(ilgen.DeclareLocal(vartype));
			}

			var labelstomark = new Dictionary<int, string>(); // key is instruction index, value is labelname
			var labels = new Dictionary<int, Label>();

			foreach (var instr in method.Body.Instructions.Where(i => i.Operand is Instruction))
			{	try
				{
					labels.Add(method.Body.Instructions.IndexOf((Instruction) instr.Operand), ilgen.DefineLabel());
				}
				catch (ArgumentException) { }
			}

			var index = 0;
			foreach (var instruction in method.Body.Instructions)
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
					ilgen.Emit(opcode, labels[method.Body.Instructions.IndexOf((Instruction)instruction.Operand)]);
				else 
					throw new NotImplementedException(instruction.Operand.GetType().FullName);

				++index;
			}

			return target;
		}
	}
}
