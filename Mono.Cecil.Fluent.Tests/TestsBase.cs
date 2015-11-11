using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Fluent.Utils;

namespace Mono.Cecil.Fluent.Tests
{
	public class TestsBase
	{
		internal static readonly ModuleDefinition TestModule = ModuleDefinition.CreateModule(Generate.Name.ForClass(), ModuleKind.Dll);
	}
}
