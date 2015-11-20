using System;

namespace Mono.Cecil.Fluent
{
	public static partial class FieldDefinitionExtensions
	{
		public static FieldDefinition SetAttributes(this FieldDefinition field, params FieldAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				field.Attributes |= attribute;
			return field;
		}
	}
	public static partial class PropertyDefinitionExtensions
	{
		public static PropertyDefinition SetAttributes(this PropertyDefinition property, params PropertyAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				property.Attributes |= attribute;
			return property;
		}
	}
	public static partial class EventDefinitionExtensions
	{
		public static EventDefinition SetAttributes(this EventDefinition @event, params EventAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				@event.Attributes |= attribute;
			return @event;
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
	}

	public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody SetAttributes(this MethodDefinition method, params MethodAttributes[] attributes)
		{
			return new FluentMethodBody(method).SetAttributes(attributes);
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
	}
}
