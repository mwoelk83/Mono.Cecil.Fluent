using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent.Analyzer
{
	// todo: check that only leave instructins leave try blocks (no branches!)
	// todo: validate types on stack
	// todo: validate that different incoming code paths have the same end stack size
	internal sealed class FlowControlAnalyzer
	{
		public readonly Dictionary<Instruction, CodePath> CodePaths;
		public readonly HashSet<Instruction> JumpTargets;

		public FlowControlAnalyzer(MethodBody body)
		{
			if (body == null)
				return;

			JumpTargets = GetJumpTargets(body);
			CodePaths = GetCodePaths(body, JumpTargets);

			foreach (var codePath in CodePaths.Values)
			{
				if (codePath.Next != null)
					CodePaths[codePath.Next].AddIncomingPath(codePath);
				if (codePath.Alternatives.Length > 0)
				{
					foreach (var alternative in codePath.Alternatives)
					{
						CodePaths[alternative].AddIncomingPath(codePath);
					}
				}
			}
		}

		public void ValidateFullStackOrThrow()
		{
			if (CodePaths == null)
				return;

			foreach (var codepath in CodePaths.Values)
				codepath.ValidateStackOrThrow();
		}

		private static Dictionary<Instruction, CodePath> GetCodePaths(MethodBody body, HashSet<Instruction> jumptargets)
		{
			var codePaths = new Dictionary<Instruction, CodePath>();

			if (body.Instructions.Count == 0)
				return codePaths; // ncrunch: no coverage

            var current = body.Instructions[0];
			var pathstart = current;

			do
			{
				var flowcontrol = current.OpCode.FlowControl;

			    // ReSharper disable once SwitchStatementMissingSomeCases
				switch (flowcontrol)
				{
					case FlowControl.Branch:
						codePaths.Add(pathstart, new CodePath(pathstart, current, (Instruction)current.Operand, (Instruction)null, body));
						pathstart = current.Next;
						break;
					case FlowControl.Cond_Branch:
				        codePaths.Add(pathstart,
				            current.OpCode.OperandType == OperandType.InlineSwitch
				                ? new CodePath(pathstart, current, current.Next, (Instruction[]) current.Operand, body)
				                : new CodePath(pathstart, current, (Instruction) current.Operand, current.Next, body));
				        pathstart = current.Next;
						break;
					case FlowControl.Return:
					case FlowControl.Throw:
						codePaths.Add(pathstart, new CodePath(pathstart, current, current.Next, (Instruction)null, body));
						pathstart = current.Next;
						break;
					default:
						if (jumptargets.Contains(current.Next))
						{
							codePaths.Add(pathstart, new CodePath(pathstart, current, current.Next, (Instruction)null, body));
							pathstart = current.Next;
						}
						else if (current.Next == null)
							codePaths.Add(pathstart, new CodePath(pathstart, current, null, current.Operand as Instruction, body));
						break;
				}
				current = current.Next;
			} while (current != null);

			return codePaths;
		}

		private static HashSet<Instruction> GetJumpTargets(MethodBody body)
		{
			var jumptargets = new HashSet<Instruction>();

			if (body.Instructions.Count == 0)
				return jumptargets; // ncrunch: no coverage

			var current = body.Instructions[0];
			jumptargets.Add(current);

			do
			{
				var flowcontrol = current.OpCode.FlowControl;
				if (current.OpCode.OperandType == OperandType.InlineSwitch)
				{
					foreach (var ins in (Instruction[])current.Operand)
						jumptargets.Add(ins);
				}
				else if (flowcontrol == FlowControl.Branch || flowcontrol == FlowControl.Cond_Branch)
					jumptargets.Add((Instruction)current.Operand);
				current = current.Next;
			} while (current != null);

			return jumptargets;
		}
	}
}
