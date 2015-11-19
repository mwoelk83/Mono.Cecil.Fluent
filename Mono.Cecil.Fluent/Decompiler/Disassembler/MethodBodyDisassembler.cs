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

using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Cecil.Fluent;

namespace ICSharpCode.Decompiler.Disassembler
{
	/// <summary>
	/// Disassembles a method body.
	/// </summary>
	internal sealed class MethodBodyDisassembler
	{
		private readonly PlainTextOutput o;

		public MethodBodyDisassembler(PlainTextOutput output)
		{
			if (output == null)
				throw new ArgumentNullException(nameof(output));
			o = output;
		}

		public void Disassemble(MethodBody body)
		{
			// start writing IL code
			var method = body.Method;
			var codesize = body.ComputeCodeSize();
			var maxstacksize = body.ComputeMaxStackSize();
			body.ComputeOffsets();
			
			o.WriteLine("// Code size {0} (0x{0:x})", codesize);
			o.WriteLine(".maxstack {0}", maxstacksize);
			if (method.DeclaringType.Module.Assembly != null && method.DeclaringType.Module.Assembly.EntryPoint == method)
				o.WriteLine(".entrypoint");

			if (method.Body.HasVariables)
			{
				o.Write(".locals ");
				if (body.InitLocals)
					o.Write("init ");
				o.WriteLine("(");
				o.Indent();
				foreach (var v in body.Variables)
				{
					o.Write("[" + v.Index + "] ");
					v.VariableType.WriteTo(o);
					if (!string.IsNullOrEmpty(v.Name))
					{
						o.Write(' ');
						o.Write(DisassemblerHelpers.Escape(v.Name));
					}
					if (v.Index + 1 < body.Variables.Count)
						o.Write(',');
					o.WriteLine();
				}
				o.Unindent();
				o.WriteLine(")");
			}
			o.WriteLine();

			if (body.Instructions.Count > 0)
			{
				var inst = body.Instructions[0];
				var branchTargets = GetBranchTargets(body.Instructions);
				WriteStructureBody(new ILStructure(body, codesize), branchTargets, ref inst, codesize);
			}
			else
			{
				if (!method.Body.HasExceptionHandlers)
					return;
				o.WriteLine();
				foreach (var eh in method.Body.ExceptionHandlers)
				{
					eh.WriteTo(o);
					o.WriteLine();
				}
			}
		}

		private static HashSet<int> GetBranchTargets(IEnumerable<Instruction> instructions)
		{
			var branchTargets = new HashSet<int>();
			foreach (var inst in instructions)
			{
				var target = inst.Operand as Instruction;
				if (target != null)
					branchTargets.Add(target.Offset);
				var targets = inst.Operand as Instruction[];
				if (targets != null)
					foreach (var t in targets)
						branchTargets.Add(t.Offset);
			}
			return branchTargets;
		}

		private void WriteStructureHeader(ILStructure s)
		{
			switch (s.Type)
			{
				case ILStructureType.Loop:
					o.Write("// loop start");
					if (s.LoopEntryPoint != null)
					{
						o.Write(" (head: ");
						DisassemblerHelpers.WriteOffsetReference(o, s.LoopEntryPoint);
						o.Write(')');
					}
					o.WriteLine();
					break;
				case ILStructureType.Try:
					o.WriteLine(".try");
					o.WriteLine("{");
					break;
				case ILStructureType.Handler:
					switch (s.ExceptionHandler.HandlerType)
					{
						case ExceptionHandlerType.Catch:
						case ExceptionHandlerType.Filter:
							o.Write("catch");
							if (s.ExceptionHandler.CatchType != null)
							{
								o.Write(' ');
								s.ExceptionHandler.CatchType.WriteTo(o, ILNameSyntax.TypeName);
							}
							o.WriteLine();
							break;
						case ExceptionHandlerType.Finally:
							o.WriteLine("finally");
							break;
						case ExceptionHandlerType.Fault:
							o.WriteLine("fault");
							break;
						default:
							throw new NotSupportedException();
					}
					o.WriteLine("{");
					break;
				case ILStructureType.Filter:
					o.WriteLine("filter");
					o.WriteLine("{");
					break;
				default:
					throw new NotSupportedException();
			}
			o.Indent();
		}

		private void WriteStructureBody(ILStructure s, HashSet<int> branchTargets, ref Instruction inst, int codeSize)
		{
			var isFirstInstructionInStructure = true;
			var prevInstructionWasBranch = false;
			var childIndex = 0;
			while (inst != null && inst.Offset < s.EndOffset)
			{
				var offset = inst.Offset;
				if (childIndex < s.Children.Count && s.Children[childIndex].StartOffset <= offset && offset < s.Children[childIndex].EndOffset)
				{
					var child = s.Children[childIndex++];
					WriteStructureHeader(child);
					WriteStructureBody(child, branchTargets, ref inst, codeSize);
					WriteStructureFooter(child);
				}
				else
				{
					if (!isFirstInstructionInStructure && (prevInstructionWasBranch || branchTargets.Contains(offset)))
					{
						o.WriteLine(); // put an empty line after branches, and in front of branch targets
					}
					inst.WriteTo(o);

					o.WriteLine();

					prevInstructionWasBranch = inst.OpCode.FlowControl == FlowControl.Branch
						|| inst.OpCode.FlowControl == FlowControl.Cond_Branch
						|| inst.OpCode.FlowControl == FlowControl.Return
						|| inst.OpCode.FlowControl == FlowControl.Throw;

					inst = inst.Next;
				}
				isFirstInstructionInStructure = false;
			}
		}

		private void WriteStructureFooter(ILStructure s)
		{
			o.Unindent();
			switch (s.Type)
			{
				case ILStructureType.Loop:
					o.WriteLine("// end loop");
					break;
				case ILStructureType.Try:
					o.WriteLine("} // end .try");
					break;
				case ILStructureType.Handler:
					o.WriteLine("} // end handler");
					break;
				case ILStructureType.Filter:
					o.WriteLine("} // end filter");
					break;
				default:
					throw new NotSupportedException();
			}
		}
	}
}
