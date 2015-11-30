using System;
using System.Reflection;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	public interface ISystemMathEmitter
	{
		FluentMethodBody FluentMethodBody { get; }
		FluentMethodBody Abs<T>();
		FluentMethodBody Sqrt();
	}

	partial class FluentMethodBody : ISystemMathEmitter
	{ 
		FluentMethodBody ISystemMathEmitter.FluentMethodBody => this;

		FluentMethodBody ISystemMathEmitter.Abs<T>()
		{
			// todo: caching?
			var absmethod = typeof(System.Math).GetMethod("Abs", new[] { typeof(T) });
			if (absmethod == null || !absmethod.ReturnType.IsPrimitive)
				throw new Exception($"no System.Math.Abs function found for type {typeof(T)}");

			return Emit(OpCodes.Call, absmethod);
		}

		static MethodReference sqrtmethod;
		public FluentMethodBody Sqrt()
		{
			if (sqrtmethod == null)
				sqrtmethod = Module.SafeImport(typeof (Math).GetMethod("Sqrt", BindingFlags.Public | BindingFlags.Static));

			return Emit(OpCodes.Call, sqrtmethod);
		}
	}
}
