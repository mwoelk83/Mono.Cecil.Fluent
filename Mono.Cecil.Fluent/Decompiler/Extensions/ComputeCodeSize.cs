using System.Linq;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodBodyExtensions
	{
		public static int ComputeCodeSize(this MethodBody body)
		{
			return body.Instructions.Sum(instruction => instruction.GetSize());
		}

		public static int ComputeCodeSize(this FluentEmitter method)
		{
			return method.Body.ComputeCodeSize();
		}
	}
}
