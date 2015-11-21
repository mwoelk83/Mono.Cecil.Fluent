using Mono.Cecil.Fluent.Attributes;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{

	public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody SetAttributes(this MethodDefinition method, params MethodAttributes[] attributes)
		{
			return new FluentMethodBody(method).SetAttributes(attributes);
		}

		public static FluentMethodBody SetAttributes<TAttr>(this MethodDefinition method)
			where TAttr : struct, IMethodAttribute
		{
			method.Attributes |= default(TAttr).MethodAttributesValue;
			return new FluentMethodBody(method);
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr2, TAttr3>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr2, TAttr3, TAttr4>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
			where TAttr7 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
			where TAttr7 : struct, IMethodAttribute
			where TAttr8 : struct, IMethodAttribute
		{
			return method.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>()
				.SetAttributes<TAttr1>();
		}
	}
}
