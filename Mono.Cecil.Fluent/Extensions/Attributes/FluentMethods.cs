using System;
using Mono.Cecil.Fluent.Attributes;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	namespace Attributes
	{
		public interface IMethodAttribute
		{
			MethodAttributes MethodAttributesValue { get; }
		}
	}

	public static partial class FluentMethodBodyExtensions
	{
		public static FluentMethodBody SetAttributes(this FluentMethodBody method, params MethodAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				method.Attributes |= attribute;
			return method;
		}
		public static FluentMethodBody SetAttributes<TAttr>(this FluentMethodBody method) where TAttr : struct, IMethodAttribute
		{
			method.Attributes |= default(TAttr).MethodAttributesValue;
			return method;
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2>(this FluentMethodBody method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3>(this FluentMethodBody method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4>(this FluentMethodBody method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5>(this FluentMethodBody method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>(this FluentMethodBody method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>(this FluentMethodBody method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
			where TAttr7 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>(this FluentMethodBody method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
			where TAttr7 : struct, IMethodAttribute
			where TAttr8 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>();
		}
	}
}
