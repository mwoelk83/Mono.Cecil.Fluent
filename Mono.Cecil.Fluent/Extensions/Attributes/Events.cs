using Mono.Cecil.Fluent.Attributes;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	namespace Attributes
	{
		public interface IEventAttribute
		{
			EventAttributes EventAttributesValue { get; }
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

		public static EventDefinition SetAttributes<TAttr>(this EventDefinition @event) 
			where TAttr : struct, IEventAttribute
		{
			@event.Attributes |= default(TAttr).EventAttributesValue;
			return @event;
		}

		public static EventDefinition SetAttributes<TAttr1, TAttr2>(this EventDefinition field)
			where TAttr1 : struct, IEventAttribute
			where TAttr2 : struct, IEventAttribute
		{
			return field.SetAttributes<TAttr1>()
				.SetAttributes<TAttr2>();
		}
	}
}
