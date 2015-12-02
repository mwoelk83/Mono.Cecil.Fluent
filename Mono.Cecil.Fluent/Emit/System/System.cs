using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody : ISystemEmitter
	{
		public ISystemEmitter System => this;

		ISystemMathEmitter ISystemEmitter.Math => this;
	}

	public interface ISystemEmitter
	{
		ISystemMathEmitter Math { get; }
	}
}
