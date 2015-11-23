using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	internal static class SimpleStackValidator
	{
		public static void ValidatePostEmit(Instruction instruction, FluentMethodBody method)
		{
			// todo: validate popped types function signatures

			var lastinstructions = new List<Instruction>();

			var curinstr = instruction.Previous;
			while (curinstr != null && curinstr.OpCode.FlowControl != FlowControl.Branch && curinstr.OpCode.FlowControl != FlowControl.Break
			       && curinstr.OpCode.FlowControl != FlowControl.Throw && curinstr.OpCode.FlowControl != FlowControl.Return 
				   && curinstr.OpCode.FlowControl != FlowControl.Cond_Branch)
			{
				if (method.IfBlocks.Any(block => block.StartInstruction == curinstr))
					break;
				lastinstructions.Add(curinstr);
				curinstr = curinstr.Previous;
			}

			lastinstructions.Reverse();

			var stacksize = 0;

			foreach (var instr in lastinstructions.AsEnumerable())
				MethodBodyExtensions.ComputeStackDelta(instr, ref stacksize);

			var popdelta = 0;
			MethodBodyExtensions.ComputeStackDelta(instruction, ref popdelta, addpush: false);

			if (instruction.OpCode == OpCodes.Ret && !method.ReturnType.SafeEquals(method.Module.TypeSystem.Void))
				popdelta = 1;
			else
				popdelta = Math.Abs(popdelta);

			var msg = "";
			if (stacksize != popdelta && (instruction.OpCode.FlowControl == FlowControl.Branch 
				|| instruction.OpCode.FlowControl == FlowControl.Break || instruction.OpCode.FlowControl == FlowControl.Throw
				|| instruction.OpCode.FlowControl == FlowControl.Return || instruction.OpCode.FlowControl == FlowControl.Cond_Branch))
			{
				ThrowFlowControl(instruction, lastinstructions, stacksize, popdelta, method);
			}

			if (stacksize >= popdelta)
				return;

			ThrowStackTooSmall(instruction, lastinstructions, stacksize, popdelta, method);
		}

		private static void ThrowFlowControl(Instruction instruction, List<Instruction> lastinstructions, int stacksize, int popdelta, FluentMethodBody method)
		{
			method.Body.ComputeOffsets();
			var msg = $"FlowControl of Opcode {instruction.OpCode} is Branch, Conditional Branch, Break, Throw or Return." +
				"After the Instruction pops the needed values from stack (like non void returns or conditional branches) " +
			    $"there must be 0 values on stack. But there are {stacksize} values on stack and the " + 
				$"Instruction pops {popdelta} values from stack. At:" + Environment.NewLine;
			lastinstructions.Add(instruction);
			msg = lastinstructions.Aggregate(msg, (m, instr) => m + (instr + Environment.NewLine));
			throw new Exception(msg);
		}

		private static void ThrowStackTooSmall(Instruction instruction, List<Instruction> lastinstructions, int stacksize, int popdelta, FluentMethodBody method)
		{
			method.Body.ComputeOffsets();
			var msg = $"Instruction {instruction} pops {Math.Abs(popdelta)} values from the stack,";
			if (stacksize == 0)
				msg += " but stack is empty. At:" + Environment.NewLine;
			else if (stacksize == 1)
				msg += " but there is only one value on stack. At:" + Environment.NewLine;
			else
				msg += $" but there are only {Math.Abs(stacksize)} values on stack. At:" + Environment.NewLine;

			lastinstructions.Add(instruction);
			msg = lastinstructions.Aggregate(msg, (m, instr) => m + (instr + Environment.NewLine));

			throw new Exception(msg);
		}
	}
}
