using System;
using System.Reflection;

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

		public static FieldReference SafeImport(this ModuleDefinition module, FieldReference field)
		{
			lock (SyncRoot)
				return module.Import(field);
		}

		public static MethodReference SafeImport(this ModuleDefinition module, MethodReference method)
		{
			lock (SyncRoot)
				return module.Import(method);
		}

		public static FieldReference SafeImport(this ModuleDefinition module, FieldInfo field)
		{
			lock (SyncRoot)
				return module.Import(field);
		}

		public static MethodReference SafeImport(this ModuleDefinition module, MethodInfo method)
		{
			lock (SyncRoot)
				return module.Import(method);
		}

		public static TypeReference SafeImport(this ModuleDefinition module, TypeInfo type)
		{
			lock (SyncRoot)
				return module.Import(type);
		}

		public static MethodReference SafeImport(this ModuleDefinition module, ConstructorInfo constructor)
		{
			lock (SyncRoot)
				return module.Import(constructor);
		}
	}
}
