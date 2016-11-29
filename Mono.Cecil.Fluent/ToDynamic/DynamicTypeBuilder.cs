using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	internal class DynamicTypeBuilder
	{
		private readonly TypeDefinition _type;

		internal DynamicTypeBuilder(TypeDefinition type)
		{
			_type = type;
		}

		/// <summary>
		/// todo: properties, events, custom attributes, subtypes
		/// </summary>
		public Type Process()
		{
			var parent = typeof(object);
			var interfaces = new List<Type>();
			var importer = TypeLoader.Instance;

			if (_type.BaseType != null)
			{
				parent = importer.Load(_type.BaseType);
				if (parent == null)
					throw new Exception($"can not resolve base type '{_type.BaseType.FullName}' in current app domain"); // ncrunch: no coverage
			}
			if (_type.Interfaces.Count != 0)
			{
				foreach (var @interface in _type.Interfaces)
				{
					var iface = importer.Load(@interface);
					if (iface == null)
						throw new Exception($"can not resolve interface type '{@interface.FullName}' in current app domain"); // ncrunch: no coverage
                    interfaces.Add(iface);
				}
			}

			var typebuilder = CreateTypeBuilder(_type.Name, (System.Reflection.TypeAttributes) _type.Attributes, parent, interfaces);

			foreach (var field in _type.Fields)
			{
				var fieldtype = importer.Load(field.FieldType);
				if (fieldtype == null)
					throw new Exception($"can not resolve type '{field.FieldType.FullName}' for field '{field.Name}' in current app domain"); // ncrunch: no coverage
                typebuilder.DefineField(field.Name, fieldtype, (System.Reflection.FieldAttributes) field.Attributes);
			}

			foreach (var method in _type.Methods)
			{
				var returntype = importer.Load(method.ReturnType);
				if (returntype == null)
					throw new Exception($"can not resolve type '{method.ReturnType.FullName}' for method '{method.Name}' in current app domain"); // ncrunch: no coverage
                var paramtypes = new List<Type>();
				foreach (var param in method.Parameters)
				{
					var t = importer.Load(param.ParameterType);
					if (t == null)
						throw new Exception($"can not resolve type '{method.ReturnType.FullName}' for method '{method.Name}' parameter '{param.Name}' in current app domain"); // ncrunch: no coverage
                    paramtypes.Add(t);
				}
				var mb = typebuilder.DefineMethod(method.Name, (System.Reflection.MethodAttributes) method.Attributes, returntype, paramtypes.ToArray());
				var body = new FluentMethodBody(method);
				body.ToDynamicMethod(typebuilder, mb);
			}

			return typebuilder.CreateType();
		}

		private TypeBuilder CreateTypeBuilder(string name, System.Reflection.TypeAttributes attributes, Type parent, List<Type> interfaces )
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
