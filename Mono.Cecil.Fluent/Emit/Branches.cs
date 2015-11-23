using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		class IfBlock
		{
			public Instruction StartInstruction;
			public OpCode OpCode;
		}

		private Stack<IfBlock> IfBlocks = new Stack<IfBlock>();

		public FluentMethodBody If()
		{
			Nop();
			var block = new IfBlock() { StartInstruction = LastEmittedInstruction, OpCode = OpCodes.Brfalse }; // it jumps if value top on stack is false
			IfBlocks.Push(block);
			return this;
		}

		public FluentMethodBody IfNot()
		{
			Nop();
			var block = new IfBlock() { StartInstruction = LastEmittedInstruction, OpCode = OpCodes.Brtrue }; // it jumps if value top on stack is true
			IfBlocks.Push(block);
			return this;
		}

		public FluentMethodBody Iflt()
		{
			Nop();
			var block = new IfBlock() { StartInstruction = LastEmittedInstruction, OpCode = OpCodes.Bge };
			IfBlocks.Push(block);
			return this;
		}

		public FluentMethodBody Ifgt()
		{
			Nop();
			var block = new IfBlock() { StartInstruction = LastEmittedInstruction, OpCode = OpCodes.Ble };
			IfBlocks.Push(block);
			return this;
		}

		public FluentMethodBody Iflte()
		{
			Nop();
			var block = new IfBlock() { StartInstruction = LastEmittedInstruction, OpCode = OpCodes.Bgt };
			IfBlocks.Push(block);
			return this;
		}

		public FluentMethodBody Ifgte()
		{
			Nop();
			var block = new IfBlock() { StartInstruction = LastEmittedInstruction, OpCode = OpCodes.Blt };
			IfBlocks.Push(block);
			return this;
		}

		public FluentMethodBody Else()
		{
			Nop();
			var jumpstart = LastEmittedInstruction;
			EndIf();
			var block = new IfBlock() { StartInstruction = jumpstart, OpCode = OpCodes.Br }; // it jumps if value top on stack is true
			IfBlocks.Push(block);
			return this;
		}

		public FluentMethodBody EndIf()
		{
			if(IfBlocks.Count == 0)
				throw new NotSupportedException("no if-block to close"); // ncrunch: no coverage

			var block = IfBlocks.Pop();

			if (block.StartInstruction?.Previous.OpCode == OpCodes.Ret)
			{
				Body.Instructions.Remove(block.StartInstruction);
				return this;
			}

			var firstinstructionafterblock = LastEmittedInstruction.Next;
			if(firstinstructionafterblock == null)
			{
				Nop();
				firstinstructionafterblock = LastEmittedInstruction;
			}
			var newstartinstruction = Instruction.Create(block.OpCode, firstinstructionafterblock);
			Body.GetILProcessor().Replace(block.StartInstruction, newstartinstruction);

			// remove Nops
			Func<FluentMethodBody, bool> postemitaction = (body) =>
			{
				if (firstinstructionafterblock.Next == null)
					return false;
				foreach (var instruction in body.Body.Instructions.Where(i => i.Operand == firstinstructionafterblock))
					instruction.Operand = firstinstructionafterblock.Next;
				Body.GetILProcessor().Remove(firstinstructionafterblock);
				return true;
			};

			PostEmitActions.Enqueue(postemitaction);
			
			return this;
		}
	}
}