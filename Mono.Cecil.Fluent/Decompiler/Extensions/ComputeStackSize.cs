using System.Collections.Generic;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodBodyExtensions
	{
		public static int ComputeMaxStackSize(this MethodBody body)
		{
			var stackSize = 0;
			var maxStack = 0;
			var stackSizes = new Dictionary<Instruction, int>();

			if (body.HasExceptionHandlers)
			{
				foreach (var eh in body.ExceptionHandlers)
				{
					if (eh.HandlerType != ExceptionHandlerType.Catch && eh.HandlerType != ExceptionHandlerType.Filter)
						continue;
					
					if (eh.HandlerStart != null)
						stackSizes[eh.HandlerStart] = 1;
					if (eh.FilterStart != null && eh.HandlerType == ExceptionHandlerType.Filter)
						stackSizes[eh.FilterStart] = 1;
				}
			}
			
			foreach (var instruction in body.Instructions)
			{
				int computedSize;
				if (stackSizes != null && stackSizes.TryGetValue(instruction, out computedSize))
					stackSize = computedSize;
				maxStack = System.Math.Max(maxStack, stackSize);
				ComputeStackDelta(instruction, ref stackSize);
				maxStack = System.Math.Max(maxStack, stackSize);
				CopyBranchStackSize(instruction, ref stackSizes, stackSize);
				ComputeStackSize(instruction, ref stackSize);
			}

			return maxStack;
		}

		internal static void ComputeStackDelta(Instruction instruction, ref int stackSize, bool addpush = true)
		{
		    // ReSharper disable once SwitchStatementMissingSomeCases
			switch (instruction.OpCode.FlowControl)
			{
				case FlowControl.Call:
					{
						var method = (IMethodSignature)instruction.Operand;
						// pop 'this' argument
						if (method.HasImplicitThis() && instruction.OpCode.Code != Code.Newobj)
							stackSize--;
						// pop normal arguments
						if (method.HasParameters)
							stackSize -= method.Parameters.Count;
						// pop function pointer
						if (instruction.OpCode.Code == Code.Calli)
							stackSize--;
						// push return value
						if (!method.ReturnType.IsVoid() || instruction.OpCode.Code == Code.Newobj)
							if(addpush)
								stackSize++;
						break;
					}
				default:
					ComputePopDelta(instruction.OpCode.StackBehaviourPop, ref stackSize);
					if(addpush)
						ComputePushDelta(instruction.OpCode.StackBehaviourPush, ref stackSize);
					break;
			}
		}

		private static void CopyBranchStackSize(Instruction instruction, ref Dictionary<Instruction, int> stackSizes, int stackSize)
		{
			if (stackSize == 0)
				return;

		    // ReSharper disable once SwitchStatementMissingSomeCases
			switch (instruction.OpCode.OperandType)
			{
				case OperandType.ShortInlineBrTarget:
				case OperandType.InlineBrTarget:
					CopyBranchStackSize(ref stackSizes, (Instruction)instruction.Operand, stackSize);
					break;
				case OperandType.InlineSwitch:
					var targets = (Instruction[])instruction.Operand;
					foreach (var t in targets)
						CopyBranchStackSize(ref stackSizes, t, stackSize);
					break;
			}
		}

		private static void CopyBranchStackSize(ref Dictionary<Instruction, int> stackSizes, Instruction target, int stackSize)
		{
			if (stackSizes == null)
				stackSizes = new Dictionary<Instruction, int>(); // ncrunch: no coverage
            var branchStackSize = stackSize;
			int computedSize;
			if (stackSizes.TryGetValue(target, out computedSize))
				branchStackSize = System.Math.Max(branchStackSize, computedSize);
			stackSizes[target] = branchStackSize;
		}

		private static void ComputeStackSize(Instruction instruction, ref int stack_size)
		{
			if (instruction.OpCode.FlowControl == FlowControl.Branch || 
			    instruction.OpCode.FlowControl == FlowControl.Throw || instruction.OpCode.FlowControl == FlowControl.Return)
			{
				stack_size = 0;
			}
		}

		private static void ComputePopDelta(StackBehaviour popBehavior, ref int stackSize)
		{
		    // ReSharper disable once SwitchStatementMissingSomeCases
			switch (popBehavior)
			{
				case StackBehaviour.Popi:
				case StackBehaviour.Popref:
				case StackBehaviour.Pop1:
					stackSize--;
					break;
				case StackBehaviour.Pop1_pop1:
				case StackBehaviour.Popi_pop1:
				case StackBehaviour.Popi_popi:
				case StackBehaviour.Popi_popi8:
				case StackBehaviour.Popi_popr4:
				case StackBehaviour.Popi_popr8:
				case StackBehaviour.Popref_pop1:
				case StackBehaviour.Popref_popi:
					stackSize -= 2;
					break;
				case StackBehaviour.Popi_popi_popi:
				case StackBehaviour.Popref_popi_popi:
				case StackBehaviour.Popref_popi_popi8:
				case StackBehaviour.Popref_popi_popr4:
				case StackBehaviour.Popref_popi_popr8:
				case StackBehaviour.Popref_popi_popref:
					stackSize -= 3;
					break;
				case StackBehaviour.PopAll:
					stackSize = 0;
					break;
			}
		}

		private static void ComputePushDelta(StackBehaviour pushBehaviour, ref int stackSize)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (pushBehaviour)
			{
				case StackBehaviour.Push1:
				case StackBehaviour.Pushi:
				case StackBehaviour.Pushi8:
				case StackBehaviour.Pushr4:
				case StackBehaviour.Pushr8:
				case StackBehaviour.Pushref:
					stackSize++;
					break;
				case StackBehaviour.Push1_push1:
					stackSize += 2;
					break;
			}
		}
	}
}
