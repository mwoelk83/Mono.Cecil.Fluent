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
		private readonly PlainTextOutput _o;

		public MethodBodyDisassembler(PlainTextOutput output)
		{
			if (output == null)
				throw new ArgumentNullException(nameof(output)); // ncrunch: no coverage
			_o = output;
		}

		public void Disassemble(MethodBody body)
		{
			// start writing IL code
			var method = body.Method;
			var codesize = body.ComputeCodeSize();
			var maxstacksize = body.ComputeMaxStackSize();
			body.ComputeOffsets();
			
			_o.WriteLine("// Code size {0} (0x{0:x})", codesize);
			_o.WriteLine(".maxstack {0}", maxstacksize);
			if (method.DeclaringType.Module.Assembly != null && method.DeclaringType.Module.Assembly.EntryPoint == method)
				_o.WriteLine(".entrypoint");

			if (method.Body.HasVariables)
			{
				_o.Write(".locals ");
				if (body.InitLocals)
					_o.Write("init ");
				_o.WriteLine("(");
				_o.Indent();
				foreach (var v in body.Variables)
				{
					_o.Write("[" + v.Index + "] ");
					v.VariableType.WriteTo(_o);
					if (!string.IsNullOrEmpty(v.Name))
					{
						_o.Write(' ');
						_o.Write(DisassemblerHelpers.Escape(v.Name));
					}
					if (v.Index + 1 < body.Variables.Count)
						_o.Write(',');
					_o.WriteLine();
				}
				_o.Unindent();
				_o.WriteLine(")");
			}
			_o.WriteLine();

			if (body.Instructions.Count > 0)
			{
				var inst = body.Instructions[0];
				var branchTargets = GetBranchTargets(body.Instructions);
				WriteStructureBody(new IlStructure(body, codesize), branchTargets, ref inst, codesize);
			}
			else
			{
				// we ignore method without instructions (but it can have exception handlers)
				_o.WriteLine();
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

		private void WriteStructureHeader(IlStructure s)
		{
			switch (s.Type)
			{
				case IlStructureType.Loop:
					_o.Write("// loop start");
					if (s.LoopEntryPoint != null)
					{
						_o.Write(" (head: ");
						DisassemblerHelpers.WriteOffsetReference(_o, s.LoopEntryPoint);
						_o.Write(')');
					}
					_o.WriteLine();
					break;
				case IlStructureType.Try:
					_o.WriteLine(".try");
					_o.WriteLine("{");
					break;
				case IlStructureType.Handler:
					switch (s.ExceptionHandler.HandlerType)
					{
						case ExceptionHandlerType.Catch:
						case ExceptionHandlerType.Filter:
							_o.Write("catch");
							if (s.ExceptionHandler.CatchType != null)
							{
								_o.Write(' ');
								s.ExceptionHandler.CatchType.WriteTo(_o, IlNameSyntax.TypeName);
							}
							_o.WriteLine();
							break;
						case ExceptionHandlerType.Finally:
							_o.WriteLine("finally");
							break;
						case ExceptionHandlerType.Fault:
							_o.WriteLine("fault");
							break;
						default:
							throw new NotSupportedException(); // ncrunch: no coverage
					}
					_o.WriteLine("{");
					break;
				case IlStructureType.Filter:
					_o.WriteLine("filter");
					_o.WriteLine("{");
					break;
				default:
					throw new NotSupportedException(); // ncrunch: no coverage
			}
			_o.Indent();
		}

		private void WriteStructureBody(IlStructure s, HashSet<int> branchTargets, ref Instruction inst, int codeSize)
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
						_o.WriteLine(); // put an empty line after branches, and in front of branch targets
					}
					inst.WriteTo(_o);

					_o.WriteLine();

					prevInstructionWasBranch = inst.OpCode.FlowControl == FlowControl.Branch
						|| inst.OpCode.FlowControl == FlowControl.Cond_Branch
						|| inst.OpCode.FlowControl == FlowControl.Return
						|| inst.OpCode.FlowControl == FlowControl.Throw;

					inst = inst.Next;
				}
				isFirstInstructionInStructure = false;
			}
		}

		private void WriteStructureFooter(IlStructure s)
		{
			_o.Unindent();
			switch (s.Type)
			{
				case IlStructureType.Loop:
					_o.WriteLine("// end loop");
					break;
				case IlStructureType.Try:
					_o.WriteLine("} // end .try");
					break;
				case IlStructureType.Handler:
					_o.WriteLine("} // end handler");
					break;
				case IlStructureType.Filter:
					_o.WriteLine("} // end filter");
					break;
				default:
					throw new NotSupportedException(); // ncrunch: no coverage
			}
		}
	}
}
