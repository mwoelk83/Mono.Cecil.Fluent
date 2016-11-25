using Mono.Cecil.Fluent.Attributes;

// ReSharper disable once CheckNamespace

namespace Mono.Cecil.Fluent
{
	namespace Attributes
	{
		public interface IPropertyAttribute
		{
			PropertyAttributes PropertydAttributesValue { get; }
		}
	}

	public static partial class PropertyDefinitionExtensions
	{
		public static PropertyDefinition SetAttributes(this PropertyDefinition prop, params PropertyAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				prop.Attributes |= attribute;
			return prop;
		}

		public static PropertyDefinition SetAttributes<TAttr>(this PropertyDefinition prop)
			where TAttr : struct, IPropertyAttribute
		{
			prop.Attributes |= default(TAttr).PropertydAttributesValue;
			return prop;
		}

		public static PropertyDefinition SetAttributes<TAttr1, TAttr2>(this PropertyDefinition prop)
			where TAttr1 : struct, IPropertyAttribute
			where TAttr2 : struct, IPropertyAttribute
		{
			return prop.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2>();
		}

		public static PropertyDefinition SetAttributes<TAttr1, TAttr2, TAttr3>(this PropertyDefinition prop)
			where TAttr1 : struct, IPropertyAttribute
			where TAttr2 : struct, IPropertyAttribute
			where TAttr3 : struct, IPropertyAttribute
		{
			return prop.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3>();
		}
	}
}