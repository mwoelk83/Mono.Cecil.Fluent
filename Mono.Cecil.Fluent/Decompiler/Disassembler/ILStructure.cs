// Copyright (c) 2011 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ICSharpCode.Decompiler.FlowAnalysis;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace ICSharpCode.Decompiler.Disassembler
{
	internal enum IlStructureType
	{
		Root,
		Loop,
		Try,
		Handler,
		Filter
	}
	
	internal sealed class IlStructure
	{
		public readonly IlStructureType Type;
		public readonly int StartOffset;
		public readonly int EndOffset;
		public readonly ExceptionHandler ExceptionHandler;
		public readonly Instruction LoopEntryPoint;
		public readonly List<IlStructure> Children = new List<IlStructure>();

		public IlStructure(MethodBody body, int codesize) : this(IlStructureType.Root, 0, codesize)
		{
			// Build the tree of exception structures:
			for (var i = 0; i < body.ExceptionHandlers.Count; i++)
			{
				var eh = body.ExceptionHandlers[i];
				if (!body.ExceptionHandlers.Take(i).Any(oldEh => oldEh.TryStart == eh.TryStart && oldEh.TryEnd == eh.TryEnd))
					AddNestedStructure(new IlStructure(IlStructureType.Try, eh.TryStart.Offset, eh.TryEnd.Offset, eh));
				if (eh.HandlerType == ExceptionHandlerType.Filter)
					AddNestedStructure(new IlStructure(IlStructureType.Filter, eh.FilterStart.Offset, eh.HandlerStart.Offset, eh));
				AddNestedStructure(new IlStructure(IlStructureType.Handler, eh.HandlerStart.Offset, eh.HandlerEnd?.Offset ?? codesize, eh));
			}
			// Very simple loop detection: look for backward branches
			var allBranches = FindAllBranches(body);
			// We go through the branches in reverse so that we find the biggest possible loop boundary first (think loops with "continue;")
			for (var i = allBranches.Count - 1; i >= 0; i--)
			{
				var loopEnd = allBranches[i].Key.Offset + allBranches[i].Key.GetSize();
				var loopStart = allBranches[i].Value.Offset;

				if (loopStart >= loopEnd)
					continue;

				// We found a backward branch. This is a potential loop.
				// Check that is has only one entry point:
				Instruction entryPoint = null;

				// entry point is first instruction in loop if prev inst isn't an unconditional branch
				var prev = allBranches[i].Value.Previous;
				if (prev != null && !OpCodeInfo.IsUnconditionalBranch(prev.OpCode))
					entryPoint = allBranches[i].Value;

				var multipleEntryPoints = false;
				foreach (var pair in allBranches)
				{
					if (pair.Key.Offset >= loopStart && pair.Key.Offset < loopEnd)
						continue;
					if (loopStart > pair.Value.Offset || pair.Value.Offset >= loopEnd)
						continue;

					// jump from outside the loop into the loop
					if (entryPoint == null)
						entryPoint = pair.Value;
					else if (pair.Value != entryPoint)
						multipleEntryPoints = true;
				}
				if (!multipleEntryPoints)
				{
					AddNestedStructure(new IlStructure(IlStructureType.Loop, loopStart, loopEnd, entryPoint));
				}
			}
			SortChildren();
		}

		public IlStructure(IlStructureType type, int startOffset, int endOffset, ExceptionHandler handler = null)
		{
			Debug.Assert(startOffset < endOffset);
			Type = type;
			StartOffset = startOffset;
			EndOffset = endOffset;
			ExceptionHandler = handler;
		}

		public IlStructure(IlStructureType type, int startOffset, int endOffset, Instruction loopEntryPoint)
		{
			Debug.Assert(startOffset < endOffset);
			Type = type;
			StartOffset = startOffset;
			EndOffset = endOffset;
			LoopEntryPoint = loopEntryPoint;
		}

		private bool AddNestedStructure(IlStructure newStructure)
		{
			// special case: don't consider the loop-like structure of "continue;" statements to be nested loops
			if (Type == IlStructureType.Loop && newStructure.Type == IlStructureType.Loop && newStructure.StartOffset == StartOffset)
				return false;

			// use <= for end-offset comparisons because both end and EndOffset are exclusive
			Debug.Assert(StartOffset <= newStructure.StartOffset && newStructure.EndOffset <= EndOffset);
			foreach (var child in Children)
			{
				if (child.StartOffset <= newStructure.StartOffset && newStructure.EndOffset <= child.EndOffset)
					return child.AddNestedStructure(newStructure);

				if (child.EndOffset <= newStructure.StartOffset || newStructure.EndOffset <= child.StartOffset)
					continue;

				// child and newStructure overlap
				if (!(newStructure.StartOffset <= child.StartOffset && child.EndOffset <= newStructure.EndOffset))
					// Invalid nesting, can't build a tree. -> Don't add the new structure.
					return false;
			}
			// Move existing structures into the new structure:
			for (var i = 0; i < Children.Count; i++)
			{
				var child = Children[i];
				if (newStructure.StartOffset > child.StartOffset || child.EndOffset > newStructure.EndOffset)
					continue;

				Children.RemoveAt(i--);
				newStructure.Children.Add(child);
			}
			// Add the structure here:
			Children.Add(newStructure);
			return true;
		}

		/// <summary>
		/// Finds all branches. Returns list of source offset->target offset mapping.
		/// Multiple entries for the same source offset are possible (switch statements).
		/// The result is sorted by source offset.
		/// </summary>
		private static List<KeyValuePair<Instruction, Instruction>> FindAllBranches(MethodBody body)
		{
			var result = new List<KeyValuePair<Instruction, Instruction>>();
			foreach (var inst in body.Instructions)
			{
			    // ReSharper disable once SwitchStatementMissingSomeCases
				switch (inst.OpCode.OperandType)
				{
					case OperandType.InlineBrTarget:
					case OperandType.ShortInlineBrTarget:
						result.Add(new KeyValuePair<Instruction, Instruction>(inst, (Instruction)inst.Operand));
						break;
					case OperandType.InlineSwitch:
						result.AddRange(from target in (Instruction[]) inst.Operand
							select new KeyValuePair<Instruction, Instruction>(inst, target));
						break;
				}
			}
			return result;
		}

		private void SortChildren()
		{
			Children.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));
			foreach (var child in Children)
				child.SortChildren();
		}
	}
}
