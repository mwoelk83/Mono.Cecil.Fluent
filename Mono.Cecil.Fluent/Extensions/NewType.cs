using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent
{
	public static class NewTypeExtensions
	{
		public static TypeDefinition NewType(this ModuleDefinition that, string name = null, TypeAttributes? attributes = null)
		{
			if (string.IsNullOrEmpty(name))
				name = Generate.Name.ForClass();

			var type = new TypeDefinition("", name, attributes ?? TypeAttributes.Class);
			that.Types.Add(type);

			return type;
		}
	}
}
