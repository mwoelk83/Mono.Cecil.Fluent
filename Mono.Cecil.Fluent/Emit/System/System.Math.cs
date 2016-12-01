using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using Mono.Cecil.Fluent.Utils;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable ExplicitCallerInfoArgument

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public interface ISystemMathEmitter
	{
		FluentEmitter Abs<T>() where T : struct, IConvertible;
		FluentEmitter Acos();
		FluentEmitter Asin();
		FluentEmitter Atan();
		FluentEmitter Atan2();
		FluentEmitter BigMul(int? b = null);
		FluentEmitter Ceiling();
		FluentEmitter CeilingDecimal();
		FluentEmitter Cos();
		FluentEmitter Cosh();
		FluentEmitter DivRemInt32();
		FluentEmitter DivRemInt64();
		FluentEmitter Exp();
		FluentEmitter Floor();
		FluentEmitter FloorDecimal();
		FluentEmitter IEEERemainder();
		FluentEmitter Log(double? basis = null);
		FluentEmitter Log10();
		FluentEmitter Max<T>(T? b = null) where T: struct, IConvertible;
		FluentEmitter Min<T>(T? b = null) where T : struct, IConvertible;
		FluentEmitter Pow(double? exponent = null);
		FluentEmitter Round(int decimals = 0, MidpointRounding mode = MidpointRounding.AwayFromZero);
		FluentEmitter RoundDecimal(int decimals = 0, MidpointRounding mode = MidpointRounding.AwayFromZero);
		FluentEmitter Sign<T>() where T : struct, IConvertible;
		FluentEmitter Sin();
		FluentEmitter Sinh();
		FluentEmitter Sqrt();
		FluentEmitter Tan();
		FluentEmitter Tanh();
		FluentEmitter TruncateDecimal();
		FluentEmitter Truncate();
	}

	partial class FluentEmitter : ISystemMathEmitter
	{
		#region GetMathMethod
		private MethodReference GetMathMethod(ref MethodReference cache, string name)
		{
			if (cache != null)
				return cache;
			cache = Module.SafeImport(typeof(Math).GetMethod(name, BindingFlags.Public | BindingFlags.Static));
			if(cache == null)
				throw new Exception($"can not resolve Method {name} in class System.Math");
			return cache;
		}
		private MethodReference GetMathMethod(ref MethodReference cache, Type[] types, string name)
		{
			if (cache != null)
				return cache;
			cache = Module.SafeImport(typeof(Math).GetMethod(name, types));
			if (cache == null)
				throw new Exception($"can not resolve Method {name} in class System.Math");
			return cache;
		}
		private MethodReference GetMathMethod<T>(ref MethodReference cache, string name)
		{
			return GetMathMethod(ref cache, new [] {typeof(T)}, name);
		}
		private MethodReference GetMathMethod<T1, T2>(ref MethodReference cache, string name)
		{
			return GetMathMethod(ref cache, new[] { typeof(T1), typeof(T2) }, name);
		}
		private MethodReference GetMathMethod<T1, T2, T3>(ref MethodReference cache, string name)
		{
			return GetMathMethod(ref cache, new[] { typeof(T1), typeof(T2), typeof(T3) }, name);
		}
		private MethodReference GetMathMethod<T>(string name)
		{
			MethodReference dummy = null;
			return GetMathMethod<T>(ref dummy, name);
		}
		private MethodReference GetMathMethod<T1, T2>(string name)
		{
			MethodReference dummy = null; 
			return GetMathMethod<T1, T2>(ref dummy, name);
		}
		#endregion

		public FluentEmitter Abs<T>() where T : struct, IConvertible
		{
			return Call(GetMathMethod<T>("Abs"));
		}

		private static MethodReference acosmethod;

		FluentEmitter ISystemMathEmitter.Acos()
		{
			return Call(GetMathMethod(ref acosmethod, "Acos"));
		}

		private static MethodReference asinmethod;

		FluentEmitter ISystemMathEmitter.Asin()
		{
			return Call(GetMathMethod(ref asinmethod, "Asin"));
		}

		private static MethodReference atanmethod;

		FluentEmitter ISystemMathEmitter.Atan()
		{
			return Call(GetMathMethod(ref atanmethod, "Atan"));
		}

		private static MethodReference atan2method;

		FluentEmitter ISystemMathEmitter.Atan2()
		{
			return Call(GetMathMethod(ref atan2method, "Atan2"));
		}

		private static MethodReference bigmulmethod;

		FluentEmitter ISystemMathEmitter.BigMul(int? i)
		{
			if (i != null)
				Ldc(i);
			return Call(GetMathMethod(ref bigmulmethod, "BigMul"));
		}

		private static MethodReference ceilingmethod;

		FluentEmitter ISystemMathEmitter.Ceiling()
		{
			return Call(GetMathMethod<double>(ref ceilingmethod, "Ceiling"));
		}

		private static MethodReference ceilingdecimalmethod;

		FluentEmitter ISystemMathEmitter.CeilingDecimal()
		{
			return Call(GetMathMethod<decimal>(ref ceilingdecimalmethod, "Ceiling"));
		}

		private static MethodReference cosmethod;

		FluentEmitter ISystemMathEmitter.Cos()
		{
			return Call(GetMathMethod(ref cosmethod, "Cos"));
		}

		private static MethodReference coshmethod;

		FluentEmitter ISystemMathEmitter.Cosh()
		{
			return Call(GetMathMethod(ref coshmethod, "Cosh"));
		}

		private static MethodReference divremi32method;

		FluentEmitter ISystemMathEmitter.DivRemInt32()
		{
			return Call(GetMathMethod(ref divremi32method, new[] { typeof(int), typeof(int), typeof(int).MakeByRefType() }, "DivRem"));
		}
		
		private static MethodReference divremi64method;

		FluentEmitter ISystemMathEmitter.DivRemInt64()
		{
			return Call(GetMathMethod(ref divremi64method, new[] { typeof(long), typeof(long), typeof(long).MakeByRefType() }, "DivRem"));
		}

		private static MethodReference expmethod;

		FluentEmitter ISystemMathEmitter.Exp()
		{
			return Call(GetMathMethod(ref expmethod, "Exp"));
		}

		private static MethodReference floormethod;

		FluentEmitter ISystemMathEmitter.Floor()
		{
			return Call(GetMathMethod<double>(ref floormethod, "Floor"));
		}

		private static MethodReference floordecimalmethod;

		FluentEmitter ISystemMathEmitter.FloorDecimal()
		{ 
			return Call(GetMathMethod<decimal>(ref floordecimalmethod, "Floor"));
		}

		private static MethodReference ieeeremaindermethod;

		FluentEmitter ISystemMathEmitter.IEEERemainder()
		{
			return Call(GetMathMethod(ref ieeeremaindermethod, "IEEERemainder"));
		}

		private static MethodReference logemethod;

		private static MethodReference logmethod;

		FluentEmitter ISystemMathEmitter.Log(double? basis)
		{
			if(basis == null)
				return Call(GetMathMethod<double, double>(ref logemethod, "Log"));
			Ldc(basis);
			return Call(GetMathMethod<double, double>(ref logmethod, "Log"));
		}

		private static MethodReference log10method;

		FluentEmitter ISystemMathEmitter.Log10()
		{
			return Call(GetMathMethod(ref log10method, "Log10"));
		}

		public FluentEmitter Max<T>(T? b) where T : struct, IConvertible
		{
			return Call(GetMathMethod<T, T>("Max"));
		}

		public FluentEmitter Min<T>(T? b) where T : struct, IConvertible
		{
			return Call(GetMathMethod<T, T>("Min"));
		}

		private static MethodReference powmethod;

		FluentEmitter ISystemMathEmitter.Pow(double? exponent)
		{
			if (exponent == null)
				return Call(GetMathMethod(ref powmethod, "Pow"));
			Ldc(exponent);
			return Call(GetMathMethod(ref powmethod, "Pow"));
		}

		private static MethodReference roundmethod;

		FluentEmitter ISystemMathEmitter.Round(int decimals, MidpointRounding mode)
		{
			Ldc(decimals);
			Ldc((int) mode);
			return Call(GetMathMethod<double, int, MidpointRounding>(ref roundmethod, "Round"));
		}

		private static MethodReference rounddecimalmethod;

		FluentEmitter ISystemMathEmitter.RoundDecimal(int decimals, MidpointRounding mode)
		{
			Ldc(decimals);
			Ldc((int)mode);
			return Call(GetMathMethod<decimal, int, MidpointRounding>(ref rounddecimalmethod, "Round"));
		}

		private static MethodReference signmethod;

		public FluentEmitter Sign<T>() where T : struct, IConvertible
		{
			return Call(GetMathMethod<T>("Sign"));
		}

		private static MethodReference sinmethod;

		FluentEmitter ISystemMathEmitter.Sin()
		{
			return Call(GetMathMethod(ref sinmethod, "Sin"));
		}

		private static MethodReference sinhmethod;

		FluentEmitter ISystemMathEmitter.Sinh()
		{
			return Call(GetMathMethod(ref sinhmethod, "Sinh"));
		}

		private static MethodReference sqrtmethod;

		FluentEmitter ISystemMathEmitter.Sqrt()
		{
			return Call(GetMathMethod(ref sqrtmethod, "Sqrt"));
		}

		private static MethodReference tanmethod;

		FluentEmitter ISystemMathEmitter.Tan()
		{
			return Call(GetMathMethod(ref tanmethod, "Tan"));
		}

		private static MethodReference tanhmethod;

		FluentEmitter ISystemMathEmitter.Tanh()
		{
			return Call(GetMathMethod(ref tanhmethod, "Tanh"));
		}

		private static MethodReference truncatemethod;

		FluentEmitter ISystemMathEmitter.Truncate()
		{
			return Call(GetMathMethod<double>(ref truncatemethod, "Truncate"));
		}

		private static MethodReference truncatedecimalmethod;

		FluentEmitter ISystemMathEmitter.TruncateDecimal()
		{
			return Call(GetMathMethod<decimal>(ref truncatedecimalmethod, "Truncate"));
		}
	}
}
