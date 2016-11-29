using System.Linq;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class CecilExtensions
    {
		public static bool IsIndexer(this PropertyDefinition property)
		{
			CustomAttribute attr;
			return property.IsIndexer(out attr);
		}

		public static bool IsIndexer(this PropertyDefinition property, out CustomAttribute defaultMemberAttribute)
		{
			defaultMemberAttribute = null;

			if (!property.HasParameters)
				return false;

			var accessor = property.GetMethod ?? property.SetMethod;
			var basePropDef = property;
			if (accessor.HasOverrides)
			{
				// if the property is explicitly implementing an interface, look up the property in the interface:
				var baseAccessor = accessor.Overrides.First().Resolve();
				if (baseAccessor != null)
				{
					foreach (var baseProp in baseAccessor.DeclaringType.Properties.Where(baseProp => baseProp.GetMethod == baseAccessor || baseProp.SetMethod == baseAccessor))
					{
						basePropDef = baseProp;
						break;
					}
				}
				else
					return false;
			}
			CustomAttribute attr;
			var defaultMemberName = basePropDef.DeclaringType.GetDefaultMemberName(out attr);
			if (defaultMemberName != basePropDef.Name)
				return false;

			defaultMemberAttribute = attr;
			return true;
		}
	}
}
