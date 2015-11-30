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

		FluentMethodBody ISystemEmitter.FluentMethodBody => this;

		ISystemMathEmitter ISystemEmitter.Math => this;
	}

	public interface ISystemEmitter
	{
		FluentMethodBody FluentMethodBody { get; }

		ISystemMathEmitter Math { get; }
	}
}
