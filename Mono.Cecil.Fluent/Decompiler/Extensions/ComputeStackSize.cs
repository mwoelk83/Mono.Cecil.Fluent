using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	public static partial class MethodBodyExtensions
	{
		public static int ComputeMaxStackSize(this MethodBody body)
		{
			var stack_size = 0;
			var max_stack = 0;
			var stack_sizes = new Dictionary<Instruction, int>();

			if (body.HasExceptionHandlers)
			{
				foreach (var eh in body.ExceptionHandlers)
				{
					if (eh.HandlerType != ExceptionHandlerType.Catch && eh.HandlerType != ExceptionHandlerType.Filter)
						continue;
					
					if (eh.HandlerStart != null)
						stack_sizes[eh.HandlerStart] = 1;
					if (eh.FilterStart != null && eh.HandlerType == ExceptionHandlerType.Filter)
						stack_sizes[eh.FilterStart] = 1;
				}
			}
			
			foreach (var instruction in body.Instructions)
			{
				int computed_size;
				if (stack_sizes != null && stack_sizes.TryGetValue(instruction, out computed_size))
					stack_size = computed_size;
				max_stack = System.Math.Max(max_stack, stack_size);
				ComputeStackDelta(instruction, ref stack_size);
				max_stack = System.Math.Max(max_stack, stack_size);
				CopyBranchStackSize(instruction, ref stack_sizes, stack_size);
				ComputeStackSize(instruction, ref stack_size);
			}

			return max_stack;
		}

		private static void ComputeStackDelta(Instruction instruction, ref int stack_size)
		{
			switch (instruction.OpCode.FlowControl)
			{
				case FlowControl.Call:
					{
						var method = (IMethodSignature)instruction.Operand;
						// pop 'this' argument
						if (method.HasImplicitThis() && instruction.OpCode.Code != Code.Newobj)
							stack_size--;
						// pop normal arguments
						if (method.HasParameters)
							stack_size -= method.Parameters.Count;
						// pop function pointer
						if (instruction.OpCode.Code == Code.Calli)
							stack_size--;
						// push return value
						if (method.ReturnType.IsVoid() || instruction.OpCode.Code == Code.Newobj)
							stack_size++;
						break;
					}
				default:
					ComputePopDelta(instruction.OpCode.StackBehaviourPop, ref stack_size);
					ComputePushDelta(instruction.OpCode.StackBehaviourPush, ref stack_size);
					break;
			}
		}

		private static void CopyBranchStackSize(Instruction instruction, ref Dictionary<Instruction, int> stack_sizes, int stack_size)
		{
			if (stack_size == 0)
				return;
			switch (instruction.OpCode.OperandType)
			{
				case OperandType.ShortInlineBrTarget:
				case OperandType.InlineBrTarget:
					CopyBranchStackSize(ref stack_sizes, (Instruction)instruction.Operand, stack_size);
					break;
				case OperandType.InlineSwitch:
					var targets = (Instruction[])instruction.Operand;
					foreach (var t in targets)
						CopyBranchStackSize(ref stack_sizes, t, stack_size);
					break;
			}
		}

		private static void CopyBranchStackSize(ref Dictionary<Instruction, int> stack_sizes, Instruction target, int stack_size)
		{
			if (stack_sizes == null)
				stack_sizes = new Dictionary<Instruction, int>();
			var branch_stack_size = stack_size;
			int computed_size;
			if (stack_sizes.TryGetValue(target, out computed_size))
				branch_stack_size = System.Math.Max(branch_stack_size, computed_size);
			stack_sizes[target] = branch_stack_size;
		}

		private static void ComputeStackSize(Instruction instruction, ref int stack_size)
		{
			if (instruction.OpCode.FlowControl == FlowControl.Branch || instruction.OpCode.FlowControl == FlowControl.Break ||
			    instruction.OpCode.FlowControl == FlowControl.Throw || instruction.OpCode.FlowControl == FlowControl.Return)
			{
				stack_size = 0;
			}
		}

		private static void ComputePopDelta(StackBehaviour pop_behavior, ref int stack_size)
		{
			switch (pop_behavior)
			{
				case StackBehaviour.Popi:
				case StackBehaviour.Popref:
				case StackBehaviour.Pop1:
					stack_size--;
					break;
				case StackBehaviour.Pop1_pop1:
				case StackBehaviour.Popi_pop1:
				case StackBehaviour.Popi_popi:
				case StackBehaviour.Popi_popi8:
				case StackBehaviour.Popi_popr4:
				case StackBehaviour.Popi_popr8:
				case StackBehaviour.Popref_pop1:
				case StackBehaviour.Popref_popi:
					stack_size -= 2;
					break;
				case StackBehaviour.Popi_popi_popi:
				case StackBehaviour.Popref_popi_popi:
				case StackBehaviour.Popref_popi_popi8:
				case StackBehaviour.Popref_popi_popr4:
				case StackBehaviour.Popref_popi_popr8:
				case StackBehaviour.Popref_popi_popref:
					stack_size -= 3;
					break;
				case StackBehaviour.PopAll:
					stack_size = 0;
					break;
			}
		}

		private static void ComputePushDelta(StackBehaviour push_behaviour, ref int stack_size)
		{
			switch (push_behaviour)
			{
				case StackBehaviour.Push1:
				case StackBehaviour.Pushi:
				case StackBehaviour.Pushi8:
				case StackBehaviour.Pushr4:
				case StackBehaviour.Pushr8:
				case StackBehaviour.Pushref:
					stack_size++;
					break;
				case StackBehaviour.Push1_push1:
					stack_size += 2;
					break;
			}
		}
	}
}
