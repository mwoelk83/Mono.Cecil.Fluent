using Mono.Cecil.Fluent.Attributes;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{

	namespace Attributes
	{
		public interface ITypeAttribute
		{
			TypeAttributes TypeAttributesValue { get; }
		}
	}

	public static partial class TypeDefinitionExtensions
	{
		public static TypeDefinition SetAttributes(this TypeDefinition type, params TypeAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				type.Attributes |= attribute;
			return type;
		}
		public static TypeDefinition SetAttributes<TAttr>(this TypeDefinition type) where TAttr : struct, ITypeAttribute
		{
			type.Attributes |= default(TAttr).TypeAttributesValue;
			return type;
		}
		public static TypeDefinition SetAttributes<TAttr1, TAttr2>(this TypeDefinition type)
			where TAttr1 : struct, ITypeAttribute
			where TAttr2 : struct, ITypeAttribute
		{
			return type.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2>();
		}
		public static TypeDefinition SetAttributes<TAttr1, TAttr2, TAttr3>(this TypeDefinition type)
			where TAttr1 : struct, ITypeAttribute
			where TAttr2 : struct, ITypeAttribute
			where TAttr3 : struct, ITypeAttribute
		{
			return type.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3>();
		}
		public static TypeDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4>(this TypeDefinition type)
			where TAttr1 : struct, ITypeAttribute
			where TAttr2 : struct, ITypeAttribute
			where TAttr3 : struct, ITypeAttribute
			where TAttr4 : struct, ITypeAttribute
		{
			return type.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4>();
		}
		public static TypeDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5>(this TypeDefinition type)
			where TAttr1 : struct, ITypeAttribute
			where TAttr2 : struct, ITypeAttribute
			where TAttr3 : struct, ITypeAttribute
			where TAttr4 : struct, ITypeAttribute
			where TAttr5 : struct, ITypeAttribute
		{
			return type.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5>();
		}
		public static TypeDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>(this TypeDefinition type)
			where TAttr1 : struct, ITypeAttribute
			where TAttr2 : struct, ITypeAttribute
			where TAttr3 : struct, ITypeAttribute
			where TAttr4 : struct, ITypeAttribute
			where TAttr5 : struct, ITypeAttribute
			where TAttr6 : struct, ITypeAttribute
		{
			return type.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>();
		}
		public static TypeDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>(this TypeDefinition type)
			where TAttr1 : struct, ITypeAttribute
			where TAttr2 : struct, ITypeAttribute
			where TAttr3 : struct, ITypeAttribute
			where TAttr4 : struct, ITypeAttribute
			where TAttr5 : struct, ITypeAttribute
			where TAttr6 : struct, ITypeAttribute
			where TAttr7 : struct, ITypeAttribute
		{
			return type.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>();
		}
		public static TypeDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>(this TypeDefinition type)
			where TAttr1 : struct, ITypeAttribute
			where TAttr2 : struct, ITypeAttribute
			where TAttr3 : struct, ITypeAttribute
			where TAttr4 : struct, ITypeAttribute
			where TAttr5 : struct, ITypeAttribute
			where TAttr6 : struct, ITypeAttribute
			where TAttr7 : struct, ITypeAttribute
			where TAttr8 : struct, ITypeAttribute
		{
			return type.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>();
		}
	}
}
