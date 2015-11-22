using System;
using System.Collections.Generic;
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

		public FluentMethodBody EndIf()
		{
			if(IfBlocks.Count == 0)
				throw new NotSupportedException("no if-block to close"); // ncrunch: no coverage

			var block = IfBlocks.Pop();
			var firstinstructionafterblock = LastEmittedInstruction.Next;
			if(firstinstructionafterblock == null)
			{
				Nop(); // todo: remove the nop later
				firstinstructionafterblock = LastEmittedInstruction;
			}
			Body.GetILProcessor().Replace(block.StartInstruction, Instruction.Create(block.OpCode, firstinstructionafterblock));
			
			return this;
		}
	}
}