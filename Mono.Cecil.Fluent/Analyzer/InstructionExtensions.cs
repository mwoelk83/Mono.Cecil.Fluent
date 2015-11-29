using System;
using System.Globalization;
using Mono.Cecil.Cil;
using static System.String;

namespace Mono.Cecil.Fluent.StackValidation
{
	internal static class InstructionExtensions
	{
		/// <summary>
		/// Get the number of values removed on the stack for this instruction.
		/// </summary>
		/// <param name="self">The Instruction on which the extension method can be called.</param>
		/// <param name="method">The method inside which the instruction comes from 
		/// (needed for StackBehaviour.Varpop).</param>
		/// <returns>The number of value removed (pop) from the stack for this instruction.</returns>
		public static int GetPopCount(this Instruction self, IMethodSignature method)
		{
			if (self == null)
				throw new ArgumentException("self");
			if (method == null)
				throw new ArgumentException("method");

			switch (self.OpCode.StackBehaviourPop)
			{
				case StackBehaviour.Pop0:
					return 0;

				case StackBehaviour.Pop1:
				case StackBehaviour.Popi:
				case StackBehaviour.Popref:
					return 1;

				case StackBehaviour.Pop1_pop1:
				case StackBehaviour.Popi_pop1:
				case StackBehaviour.Popi_popi8:
				case StackBehaviour.Popi_popr4:
				case StackBehaviour.Popi_popr8:
				case StackBehaviour.Popref_pop1:
				case StackBehaviour.Popref_popi:
				case StackBehaviour.Popi_popi:
					return 2;

				case StackBehaviour.Popi_popi_popi:
				case StackBehaviour.Popref_popi_popi:
				case StackBehaviour.Popref_popi_popi8:
				case StackBehaviour.Popref_popi_popr4:
				case StackBehaviour.Popref_popi_popr8:
				case StackBehaviour.Popref_popi_popref:
					return 3;

				case StackBehaviour.Varpop:
					switch (self.OpCode.FlowControl)
					{
						case FlowControl.Return:
							return method.ReturnType.FullName == "System.Void" ? 0 : 1;

						case FlowControl.Call:
							var calledMethod = (IMethodSignature)self.Operand;
							// avoid allocating empty ParameterDefinitionCollection
							var n = calledMethod.HasParameters ? calledMethod.Parameters.Count : 0;
							if (self.OpCode.Code != Code.Newobj)
							{
								if (calledMethod.HasThis)
									n++;
							}
							return n;

						default:
							throw new NotImplementedException("Varpop not supported for this Instruction.");
					}

				case StackBehaviour.PopAll:
					return -1;
				default:
					var unknown = Format(CultureInfo.InvariantCulture,
						"'{0}' is not a valid value for instruction '{1}'.",
						self.OpCode.StackBehaviourPush, self.OpCode);
					throw new InvalidOperationException(unknown);
			}
		}

		/// <summary>
		/// Get the number of values placed on the stack by this instruction.
		/// </summary>
		/// <param name="self">The Instruction on which the extension method can be called.</param>
		/// <returns>The number of value added (push) to the stack by this instruction.</returns>
		public static int GetPushCount(this Instruction self)
		{
			if (self == null)
				throw new ArgumentException("self");

			switch (self.OpCode.StackBehaviourPush)
			{
				case StackBehaviour.Push0:
					return 0;

				case StackBehaviour.Push1:
				case StackBehaviour.Pushi:
				case StackBehaviour.Pushi8:
				case StackBehaviour.Pushr4:
				case StackBehaviour.Pushr8:
				case StackBehaviour.Pushref:
					return 1;

				case StackBehaviour.Push1_push1:
					return 2;

				case StackBehaviour.Varpush:
					var calledMethod = (IMethodSignature)self.Operand;
					if (calledMethod != null)
						return calledMethod.ReturnType.FullName == "System.Void" ? 0 : 1;

					throw new NotImplementedException("Varpush not supported for this Instruction.");
				default:
					var unknown = Format(CultureInfo.InvariantCulture,
						"'{0}' is not a valid value for instruction '{1}'.",
						self.OpCode.StackBehaviourPush, self.OpCode);
					throw new InvalidOperationException(unknown);
			}
		} 
	}
}
