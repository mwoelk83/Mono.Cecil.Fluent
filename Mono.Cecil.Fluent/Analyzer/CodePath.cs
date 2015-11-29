using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent.Analyzer
{
	internal sealed class CodePath
	{
		public readonly Instruction StartInstruction;
		public readonly Instruction EndInstruction;

		public readonly List<CodePath> IncomingPaths;

		public readonly Instruction Next;           // start instruction of next code path
		public readonly Instruction[] Alternatives; // alternative start instructions of next code path

		public readonly MethodBody MethodBody;

		private int _stackSizeOnEnter = int.MinValue;

		public int StackSizeOnLeave => InternalGetStackSizeOnLeave(new HashSet<CodePath>());

		public int StackSizOneEnter
		{
			get
			{
				if (_stackSizeOnEnter != int.MinValue)
					return _stackSizeOnEnter;
				return _stackSizeOnEnter = InternalGetStackSizeOnEnter(new HashSet<CodePath>());
			}
		}

		private int _stackDelta = int.MinValue;

		public int StackDelta
		{
			get
			{
				if (_stackDelta != int.MinValue)
					return _stackDelta;

				var delta = 0;
				var ins = StartInstruction;

				if (MethodBody.HasExceptionHandlers && MethodBody.ExceptionHandlers.Any(eh => eh.HandlerStart == ins))
					delta++;

				while (true)
				{
					delta -= ins.GetPopCount(MethodBody.Method, delta) - ins.GetPushCount();
					if (ins == EndInstruction)
						break;
					ins = ins.Next;
				};
				return _stackDelta = delta;
			}
		}

		internal CodePath(Instruction start, Instruction end, Instruction next, Instruction alternative, MethodBody body)
		{
			IncomingPaths = new List<CodePath>();
			StartInstruction = start;
			EndInstruction = end;
			MethodBody = body;
			Next = next;
			if (alternative == null)
				Alternatives = new Instruction[0];
			else
				Alternatives = new[] { alternative };
		}

		internal CodePath(Instruction start, Instruction end, Instruction next, Instruction[] alternatives, MethodBody body)
			: this(start, end, next, (Instruction)null, body)
		{
			Alternatives = alternatives;
		}

		public void ValidateStackOrThrow()
		{
			var stacksize = StackSizOneEnter;
			var startstacksize = stacksize;

			var ins = StartInstruction;
			while (true)
			{
				var popcount = ins.GetPopCount(MethodBody.Method, stacksize);
				stacksize -= popcount;

				// exception handlers pushes one value to stack
				if (MethodBody.HasExceptionHandlers && MethodBody.ExceptionHandlers.Any(eh => eh.HandlerStart == ins))
					stacksize++;

				if (stacksize < 0)
					throw new Exception(
						$"instruction {ins} pops {popcount} values from stack." + Environment.NewLine +
						$"but there are only {stacksize + popcount} values on stack. at:" + Environment.NewLine +
						(ins.Previous?.ToString() ?? "" + Environment.NewLine) +
						ins + Environment.NewLine);  // todo: print full instruction list


				stacksize += ins.GetPushCount();

				if (stacksize != 0 && ins.OpCode.FlowControl == FlowControl.Return && ins.OpCode != OpCodes.Endfinally)
					throw new Exception("stacksize on return is not zero. at: " + Environment.NewLine +
						(ins.Previous?.ToString() ?? "" + Environment.NewLine) +
						ins + Environment.NewLine);  // todo: print full instruction list

				if (ins == EndInstruction)
					break;
				ins = ins.Next;
			};

			_stackDelta = stacksize - startstacksize;
		}

		public void AddIncomingPath(CodePath previous)
		{
			if (!IncomingPaths.Contains(previous))
				IncomingPaths.Add(previous);
		}

		private int InternalGetStackSizeOnLeave(HashSet<CodePath> checkedPaths /* avoid recursive calculation */)
		{
			if (checkedPaths.Contains(this))
				return 0;

			checkedPaths.Add(this);

			var size = StackDelta;
			if (IncomingPaths.Count > 0)
				size += IncomingPaths[0].InternalGetStackSizeOnLeave(checkedPaths);

			return size;
		}

		private int InternalGetStackSizeOnEnter(HashSet<CodePath> checkedPaths /* avoid recursive calculation */)
		{
			if (checkedPaths.Contains(this))
				return 0;

			checkedPaths.Add(this);

			var size = 0;
			if (IncomingPaths.Count > 0)
				size += IncomingPaths[0].InternalGetStackSizeOnLeave(checkedPaths);

			return size;
		}
	}
}
