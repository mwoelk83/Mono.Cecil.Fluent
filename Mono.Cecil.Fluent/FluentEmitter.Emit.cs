using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using Mono.Cecil.Fluent.Analyzer;
using Mono.Collections.Generic;
using OpCode = Mono.Cecil.Cil.OpCode;

// ReSharper disable MemberCanBePrivate.Global
namespace Mono.Cecil.Fluent
{
	partial class FluentEmitter
	{
		private Action<Instruction> _emitAction;
		internal Instruction LastEmittedInstruction = null;
		internal readonly Queue<Func<FluentEmitter, bool>> PostEmitActions = new Queue<Func<FluentEmitter, bool>>(); 

		public FluentEmitter Emit(Instruction instruction)
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

		    if (!StackValidationOnEmitEnabled) return this;
		    var validator = new FlowControlAnalyzer(Body);
		    validator.ValidateFullStackOrThrow();

		    return this;
		}

		public FluentEmitter Emit(OpCode opcode)
		{
			return Emit(Instruction.Create(opcode));
		}
		
		public FluentEmitter Emit(OpCode opcode, string arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, int arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}
		
		public FluentEmitter Emit(OpCode opcode, sbyte arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}
		
		public FluentEmitter Emit(OpCode opcode, long arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, float arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, double arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, MethodInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, ConstructorInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, FieldInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, SystemTypeOrTypeReference arg)
		{
			return Emit(Instruction.Create(opcode, arg.GetTypeReference(Module)));
		}

		public FluentEmitter Emit(OpCode opcode, FieldReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, MethodReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, VariableDefinition arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, ParameterDefinition arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, Instruction arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, Func<Collection<Instruction>, Instruction> selector)
		{
			return Emit(opcode, selector(Body.Instructions));
		}
	}
}
