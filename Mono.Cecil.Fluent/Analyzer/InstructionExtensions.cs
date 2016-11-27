using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent.Analyzer
{
	internal static class InstructionExtensions
	{
		private static readonly int[] StackBehaviourCache;

		static InstructionExtensions()
		{
			StackBehaviourCache = new[]
			{
				0, // Pop0,
				1, // Pop1,
				2, // Pop1_pop1,
				1, // Popi,
				2, // Popi_pop1,
				2, // Popi_popi,
				2, // Popi_popi8,
				3, // Popi_popi_popi,
				2, // Popi_popr4,
				2, // Popi_popr8,
				1, // Popref,
				2, // Popref_pop1,
				2, // Popref_popi,
				3, // Popref_popi_popi,
				3, // Popref_popi_popi8,
				3, // Popref_popi_popr4,
				3, // Popref_popi_popr8,
				3, // Popref_popi_popref,
				0, // PopAll,
				0, // Push0,
				1, // Push1,
				2, // Push1_push1,
				1, // Pushi,
				1, // Pushi8,
				1, // Pushr4,
				1, // Pushr8,
				1, // Pushref,
				0, // Varpop,
				0, // Varpush,
			};
		}

        /// <summary>
        /// Get the number of values removed on the stack for this instruction.
        /// </summary>
        /// <param name="self">The Instruction on which the extension method can be called.</param>
        /// <param name="method">The method inside which the instruction comes from (needed for StackBehaviour.Varpop).</param>
        /// <param name="currentstacksize">This method returns this value when stack behaviour is StackBehaviour.PopAll.</param>
        /// <returns>The number of values removed (pop) from the stack for this instruction.</returns>
        public static int GetPopCount(this Instruction self, MethodReference method, int currentstacksize = 0)
		{
			if (self == null)
				throw new ArgumentException("self"); // ncrunch: no coverage
            if (method == null)
				throw new ArgumentException("method"); // ncrunch: no coverage

            var sbp = self.OpCode.StackBehaviourPop;

			if (sbp != StackBehaviour.Varpop)
				return sbp != StackBehaviour.PopAll ? StackBehaviourCache[(int)sbp] : currentstacksize;

			if (self.OpCode.FlowControl == FlowControl.Return)
				return method.ReturnType.FullName == "System.Void" ? 0 : 1;

			var calledMethod = self.Operand as MethodReference;

			// avoid allocating empty ParameterDefinitionCollection
			var n = calledMethod.HasParameters ? calledMethod.Parameters.Count : 0;
		    if (self.OpCode.Code == Code.Newobj)
                return n;
		    if (calledMethod.HasThis)
		        n++;
		    return n;
		}

		/// <summary>
		/// Get the number of values placed on the stack by this instruction.
		/// </summary>
		/// <param name="self">The Instruction on which the extension method can be called.</param>
		/// <returns>The number of value added (push) to the stack by this instruction.</returns>
		public static int GetPushCount(this Instruction self)
		{
			if (self == null)
				throw new ArgumentNullException(nameof(self)); // ncrunch: no coverage

			var stackbehaviourpush = self.OpCode.StackBehaviourPush;

			if (stackbehaviourpush != StackBehaviour.Varpush)
				return StackBehaviourCache[(int)stackbehaviourpush];

			return (self.Operand as MethodReference)
				.ReturnType.FullName == "System.Void" ? 0 : 1;
		}
	}
}
