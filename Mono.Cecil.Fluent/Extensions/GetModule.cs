using System;
using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent
{
	static partial class IMemberDefinitionExtensions
	{
		public static ModuleDefinition GetModule(this IMemberDefinition member)
		{
			return member is TypeReference ? ((TypeReference)member).Resolve().Module : member.DeclaringType.Module;
		}
	}
}
