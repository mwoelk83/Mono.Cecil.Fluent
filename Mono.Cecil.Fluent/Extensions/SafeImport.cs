using System;

namespace Mono.Cecil.Fluent
{
	partial class ModuleDefinitionExtensions
	{
		private static readonly object SyncRoot = new object();

		public static TypeReference SafeImport(this ModuleDefinition module, Type type)
		{
			lock (SyncRoot)
				return module.Import(type);
		}

		public static TypeReference SafeImport<T>(this ModuleDefinition module)
		{
			lock (SyncRoot)
				return module.Import(typeof(T));
		}

		public static TypeReference SafeImport(this ModuleDefinition module, TypeReference type)
		{
			lock (SyncRoot)
				return module.Import(type);
		}
	}
}
