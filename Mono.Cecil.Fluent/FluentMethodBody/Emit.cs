using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		private Func<Instruction, FluentMethodBody> _emitAction;

		public Func<Instruction, FluentMethodBody> Emit => _emitAction ??
				      (_emitAction = instruction =>
				      {
					      MethodDefinition.Body.Instructions.Add(instruction);
					      return this;
				      });


	}
}
