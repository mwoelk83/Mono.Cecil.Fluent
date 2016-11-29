
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent.Attributes
{
    public static partial class MethodDefinitionExtensions
    {
        public static FluentMethodBody UnsetMethodAttributes(this MethodDefinition method, params MethodAttributes[] attributes)
        {
            return new FluentMethodBody(method).UnsetAttributes(attributes);
        }
        public static FluentMethodBody UnsetAllMethodAttributes(this MethodDefinition method)
        {
            return new FluentMethodBody(method).UnsetAllAttributes();
        }
    }

    public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody SetMethodAttributes(this MethodDefinition method, params MethodAttributes[] attributes)
		{
			return new FluentMethodBody(method).SetAttributes(attributes);
		}

		public static FluentMethodBody SetMethodAttributes<TAttr>(this MethodDefinition method)
			where TAttr : struct, IMethodAttribute
		{
			method.Attributes |= default(TAttr).MethodAttributesValue;
			return new FluentMethodBody(method);
		}
		public static FluentMethodBody SetMethodAttributes<TAttr1, TAttr2>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
		{
			return method.SetMethodAttributes<TAttr1>()
				.SetAttributes<TAttr2>();
		}
		public static FluentMethodBody SetMethodAttributes<TAttr1, TAttr2, TAttr3>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
		{
			return method.SetMethodAttributes<TAttr2, TAttr3>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
		{
			return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
		{
			return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4, TAttr5>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
		{
			return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
			where TAttr7 : struct, IMethodAttribute
		{
			return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>()
				.SetAttributes<TAttr1>();
		}
		public static FluentMethodBody SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>(this MethodDefinition method)
			where TAttr1 : struct, IMethodAttribute
			where TAttr2 : struct, IMethodAttribute
			where TAttr3 : struct, IMethodAttribute
			where TAttr4 : struct, IMethodAttribute
			where TAttr5 : struct, IMethodAttribute
			where TAttr6 : struct, IMethodAttribute
			where TAttr7 : struct, IMethodAttribute
			where TAttr8 : struct, IMethodAttribute
		{
			return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>()
				.SetAttributes<TAttr1>();
		}
	}
}
