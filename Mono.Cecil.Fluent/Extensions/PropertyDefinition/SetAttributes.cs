
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent.Attributes
{
    public interface IPropertyAttribute
    {
        PropertyAttributes PropertydAttributesValue { get; }
    }

    public static partial class PropertyDefinitionExtensions
    {
        public static PropertyDefinition UnsetPropertyAttributes(this PropertyDefinition property, params PropertyAttributes[] attributes)
        {
            foreach (var attribute in attributes)
                property.Attributes &= ~attribute;
            return property;
        }
        public static PropertyDefinition UnsetAllPropertyAttributes(this PropertyDefinition property)
        {
            property.Attributes = 0;
            return property;
        }
    }

    public static partial class PropertyDefinitionExtensions
	{
		public static PropertyDefinition SetPropertyAttributes(this PropertyDefinition prop, params PropertyAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				prop.Attributes |= attribute;
			return prop;
		}

		public static PropertyDefinition SetPropertyAttributes<TAttr>(this PropertyDefinition prop)
			where TAttr : struct, IPropertyAttribute
		{
			prop.Attributes |= default(TAttr).PropertydAttributesValue;
			return prop;
		}

		public static PropertyDefinition SetPropertyAttributes<TAttr1, TAttr2>(this PropertyDefinition prop)
			where TAttr1 : struct, IPropertyAttribute
			where TAttr2 : struct, IPropertyAttribute
		{
			return prop.SetPropertyAttributes<TAttr1>()
				.SetPropertyAttributes<TAttr2>();
		}

		public static PropertyDefinition SetPropertyAttributes<TAttr1, TAttr2, TAttr3>(this PropertyDefinition prop)
			where TAttr1 : struct, IPropertyAttribute
			where TAttr2 : struct, IPropertyAttribute
			where TAttr3 : struct, IPropertyAttribute
		{
			return prop.SetPropertyAttributes<TAttr1>()
				.SetPropertyAttributes<TAttr2, TAttr3>();
		}
	}
}