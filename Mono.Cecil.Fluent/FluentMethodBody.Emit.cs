using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using OpCode = Mono.Cecil.Cil.OpCode;

// ReSharper disable MemberCanBePrivate.Global
namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		private Action<Instruction> _emitAction;
		internal Instruction LastEmittedInstruction = null;
		internal readonly Queue<Func<FluentMethodBody, bool>> PostEmitActions = new Queue<Func<FluentMethodBody, bool>>(); 

		public FluentMethodBody Emit(Instruction instruction)
		{
			if (_emitAction == null)
				_emitAction = i => MethodDefinition.Body.Instructions.Add(i);

			_emitAction(instruction);
			LastEmittedInstruction = instruction;
			
			while (PostEmitActions.Count != 0)
			{
				var action = PostEmitActions.Dequeue();
				if(!action(this))
					PostEmitActions.Enqueue(action);
			}

			SimpleStackValidator.ValidatePostEmit(instruction, this);

			return this;
		}

		public FluentMethodBody Emit(OpCode opcode)
		{
			return Emit(Instruction.Create(opcode));
		}
		
		public FluentMethodBody Emit(OpCode opcode, string arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, int arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}
		
		public FluentMethodBody Emit(OpCode opcode, sbyte arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}
		
		public FluentMethodBody Emit(OpCode opcode, long arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, float arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, double arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, MethodInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, ConstructorInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, FieldInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, Type arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, TypeInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, TypeReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, FieldReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, MethodReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, VariableDefinition arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, ParameterDefinition arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, Instruction arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, Func<Collection<Instruction>, Instruction> selector)
		{
			return Emit(opcode, selector(Body.Instructions));
		}
	}
}
