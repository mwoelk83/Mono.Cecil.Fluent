using System;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class TypeDefinitionExtensions
	{
		public static Type ToDynamicType(this TypeDefinition that)
		{
			var ret = new DynamicTypeBuilder(that).Process();
			return ret;
		}
	}
}
