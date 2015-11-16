using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

public class TypeLoader
{
	private readonly object _syncRoot = new object();
	private readonly IDictionary<string, Type> _typesByName = new Dictionary<string, Type>();

	public static TypeLoader Instance { get; private set; }

	static TypeLoader()
	{
		Instance = new TypeLoader();
	}

	/// <summary>
	/// Liefert den System.Type einer TypeReference.
	/// </summary>
	public Type Load(TypeReference typeReference)
	{
		var fullName = typeReference.FullName.Replace('/', '+');
		Type type;

		lock (_syncRoot)
			if (_typesByName.TryGetValue(fullName, out type))
				return type;

		type = AppDomain.CurrentDomain.GetAssemblies()
			.Select(asm => asm.GetType(fullName))
			.FirstOrDefault(t => t != null);

		lock (_syncRoot)
			if (!_typesByName.ContainsKey(fullName))
				_typesByName[fullName] = type;

		return type;
	}

	/// <summary>
	/// Liefert den System.Type einer TypeReference.
	/// </summary>
	public FieldInfo Load(FieldReference fieldReference)
	{
		return Load(fieldReference.DeclaringType)?
			.GetField(fieldReference.Name);
	}

	/// <summary>
	/// Liefert den System.Type einer TypeReference.
	/// </summary>
	public MethodInfo Load(MethodReference methodReference)
	{
		Type[] paramTypes = methodReference.Parameters.Select(p => Load(p.ParameterType)).ToArray();
		if (paramTypes.Any(t => t == null))
			return null;

		return Load(methodReference.DeclaringType)?
			.GetMethod(methodReference.Name, paramTypes);
	}
}