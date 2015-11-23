using Mono.Cecil.Fluent.Attributes;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	namespace Attributes
	{
		public interface IFieldAttribute
		{
			FieldAttributes FieldAttributesValue { get; }
		}
	}

	public static partial class FieldDefinitionExtensions
	{
		public static FieldDefinition SetAttributes(this FieldDefinition field, params FieldAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				field.Attributes |= attribute;
			return field;
		}
		public static FieldDefinition SetAttributes<TAttr>(this FieldDefinition field) 
			where TAttr : struct, IFieldAttribute
		{
			field.Attributes |= default(TAttr).FieldAttributesValue;
			return field;
		}
		public static FieldDefinition SetAttributes<TAttr1, TAttr2>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
		{
			return field.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2>();
		}
		public static FieldDefinition SetAttributes<TAttr1, TAttr2, TAttr3>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
		{
			return field.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3>();
		}
		public static FieldDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4>(this FieldDefinition field) 
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
		{
			return field.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4>();
		}
		public static FieldDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
			where TAttr5 : struct, IFieldAttribute
		{
			return field.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5>();
		}
		public static FieldDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>(this FieldDefinition field) 
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
			where TAttr5 : struct, IFieldAttribute
			where TAttr6 : struct, IFieldAttribute
		{
			return field.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>();
		}
		public static FieldDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
			where TAttr5 : struct, IFieldAttribute
			where TAttr6 : struct, IFieldAttribute
			where TAttr7 : struct, IFieldAttribute
		{
			return field.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>();
		}
		public static FieldDefinition SetAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
			where TAttr5 : struct, IFieldAttribute
			where TAttr6 : struct, IFieldAttribute
			where TAttr7 : struct, IFieldAttribute
			where TAttr8 : struct, IFieldAttribute
		{
			return field.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>();
		}
	}
}
