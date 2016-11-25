using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Mono.Cecil;
using Mono.Cecil.Fluent;
using FieldAttributes = System.Reflection.FieldAttributes;
using MethodAttributes = System.Reflection.MethodAttributes;
using TypeAttributes = System.Reflection.TypeAttributes;

namespace FluentIL.Infos
{
	internal class DynamicTypeBuilder
	{
		private TypeDefinition type;

		internal DynamicTypeBuilder(TypeDefinition type)
		{
			this.type = type;
		}

		/// <summary>
		/// todo: properties, events, custom attributes, subtypes
		/// </summary>
		public Type Process()
		{
			var parent = typeof(object);
			var interfaces = new List<Type>();
			var importer = TypeLoader.Instance;

			if (type.BaseType != null)
			{
				parent = importer.Load(type.BaseType);
				if (parent == null)
					throw new Exception($"can not resolve base type '{type.BaseType.FullName}' in current app domain");
			}
			if (type.Interfaces.Count != 0)
			{
				foreach (var @interface in type.Interfaces)
				{
					var iface = importer.Load(@interface);
					if (iface == null)
						throw new Exception($"can not resolve interface type '{iface}' in current app domain");
					interfaces.Add(iface);
				}
			}

			var typebuilder = CreateTypeBuilder(type.Name, (TypeAttributes) type.Attributes, parent, interfaces);

			foreach (var field in type.Fields)
			{
				var fieldtype = importer.Load(field.FieldType);
				if (fieldtype == null)
					throw new Exception($"can not resolve type '{field.FieldType.FullName}' for field '{field.Name}' in current app domain");
				typebuilder.DefineField(field.Name, fieldtype, (FieldAttributes) field.Attributes);
			}

			foreach (var method in type.Methods)
			{
				var returntype = importer.Load(method.ReturnType);
				if (returntype == null)
					throw new Exception($"can not resolve type '{method.ReturnType.FullName}' for method '{method.Name}' in current app domain");
				var paramtypes = new List<Type>();
				foreach (var param in method.Parameters)
				{
					var t = importer.Load(param.ParameterType);
					if (t == null)
						throw new Exception($"can not resolve type '{method.ReturnType.FullName}' for method '{method.Name}' parameter '{param.Name}' in current app domain");
					paramtypes.Add(t);
				}
				var mb = typebuilder.DefineMethod(method.Name, (MethodAttributes) method.Attributes, returntype, paramtypes.ToArray());
				var body = new FluentMethodBody(method);
				body.ToDynamicMethod(typebuilder, mb);
			}

			return typebuilder.CreateType();
		}

		private TypeBuilder CreateTypeBuilder(string name, TypeAttributes attributes, Type parent, List<Type> interfaces )
		{
			var assemblyName = new AssemblyName(
				$"__assembly__{DateTime.Now.Millisecond}"
				);

			var assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(
				assemblyName,
				AssemblyBuilderAccess.RunAndSave
				);

			var moduleBuilder = assemblyBuilder.DefineDynamicModule(
				assemblyBuilder.GetName().Name,
				false
				);

			return moduleBuilder.DefineType(name,
				attributes,
				parent,
				interfaces.ToArray()
				);
		}
	}
}