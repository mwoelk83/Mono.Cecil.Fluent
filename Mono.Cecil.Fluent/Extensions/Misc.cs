using System;
using System.Globalization;
using System.Linq;

namespace Mono.Cecil.Fluent
{
	public static partial class CecilExtensions
	{
		public static bool IsVoid(this TypeReference type)
		{
			while (type is OptionalModifierType || type is RequiredModifierType)
				type = ((TypeSpecification)type).ElementType;
			return type.MetadataType == MetadataType.Void;
		}

		public static bool HasImplicitThis(this IMethodSignature self)
		{
			return self.HasThis && !self.ExplicitThis;
		}

		/// <summary>
		/// checks if the given value is a numeric zero-value.
		/// NOTE that this only works for types: [sbyte, short, int, long, IntPtr, byte, ushort, uint, ulong, float, double, decimal and IConvertible]
		/// </summary>
		public static bool IsZero(this object value)
		{
			return value.Equals((sbyte)0) ||
				   value.Equals((short)0) ||
				   value.Equals(0) ||
				   value.Equals(0L) ||
				   value.Equals(IntPtr.Zero) ||
				   value.Equals((byte)0) ||
				   value.Equals((ushort)0) ||
				   value.Equals(0u) ||
				   value.Equals(0UL) ||
				   value.Equals(0.0f) ||
				   value.Equals(0.0) ||
				   value.Equals((decimal)0) ||
				   (value as IConvertible)?.ToInt32(CultureInfo.InvariantCulture) == 0;

		}

		public static bool IsValueTypeOrVoid(this TypeReference type)
		{
			while (type is OptionalModifierType || type is RequiredModifierType)
				type = ((TypeSpecification)type).ElementType;
			if (type is ArrayType)
				return false;
			return type.IsValueType || type.IsVoid();
		}

		public static bool IsCompilerGenerated(this ICustomAttributeProvider provider)
		{
			if (provider != null && provider.HasCustomAttributes)
			{
				return provider.CustomAttributes.Any(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute");
			}
			return false;
		}

		public static bool IsCompilerGeneratedOrIsInCompilerGeneratedClass(this IMemberDefinition member)
		{
			if (member == null)
				return false;
			return member.IsCompilerGenerated() || IsCompilerGeneratedOrIsInCompilerGeneratedClass(member.DeclaringType);
		}

		public static TypeReference GetEnumUnderlyingType(this TypeDefinition type)
		{
			if (!type.IsEnum)
				throw new ArgumentException("Type must be an enum", nameof(type));

			var fields = type.Fields;

			var field = fields.FirstOrDefault(f => !f.IsStatic);

			if (field != null)
				return field.FieldType;

			throw new NotSupportedException();
		}

		public static bool IsAnonymousType(this TypeReference type)
		{
			if (type == null)
				return false;
			if (string.IsNullOrEmpty(type.Namespace) && type.HasGeneratedName() && (type.Name.Contains("AnonType") || type.Name.Contains("AnonymousType")))
			{
				var td = type.Resolve();
				return td != null && td.IsCompilerGenerated();
			}
			return false;
		}

		public static bool HasGeneratedName(this MemberReference member)
		{
			return member.Name.StartsWith("<", StringComparison.Ordinal);
		}

		public static bool ContainsAnonymousType(this TypeReference type)
		{
			var git = type as GenericInstanceType;
			if (git != null)
			{
				return IsAnonymousType(git) || git.GenericArguments.Any(t => t.ContainsAnonymousType());
			}
			var typeSpec = type as TypeSpecification;
			return typeSpec != null && typeSpec.ElementType.ContainsAnonymousType();
		}

		public static string GetDefaultMemberName(this TypeDefinition type)
		{
			CustomAttribute attr;
			return type.GetDefaultMemberName(out attr);
		}

		public static string GetDefaultMemberName(this TypeDefinition type, out CustomAttribute defaultMemberAttribute)
		{
			if (type.HasCustomAttributes)
				foreach (var ca in type.CustomAttributes)
					if (ca.Constructor.DeclaringType.Name == "DefaultMemberAttribute" && ca.Constructor.DeclaringType.Namespace == "System.Reflection"
						&& ca.Constructor.FullName == @"System.Void System.Reflection.DefaultMemberAttribute::.ctor(System.String)")
					{
						defaultMemberAttribute = ca;
						return ca.ConstructorArguments[0].Value as string;
					}
			defaultMemberAttribute = null;
			return null;
		}

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

		public static bool IsDelegate(this TypeDefinition type)
		{
			if (type.BaseType == null || type.BaseType.Namespace != "System")
				return false;

			if (type.BaseType.Name == "MulticastDelegate")
				return true;

			return type.BaseType.Name == "Delegate" && type.Name != "MulticastDelegate";
		}

		///// <summary>
		///// checks if the given TypeReference is one of the following types:
		///// [sbyte, short, int, long, IntPtr]
		///// </summary>
		//public static bool IsSignedIntegralType(this TypeReference type)
		//{
		//	return type.MetadataType == MetadataType.SByte ||
		//		   type.MetadataType == MetadataType.Int16 ||
		//		   type.MetadataType == MetadataType.Int32 ||
		//		   type.MetadataType == MetadataType.Int64 ||
		//		   type.MetadataType == MetadataType.IntPtr;
		//}

		//public static HashSet<MethodDefinition> GetAccessorMethods(this TypeDefinition type)
		//{
		//	var accessorMethods = new HashSet<MethodDefinition>();
		//	foreach (var property in type.Properties)
		//	{
		//		accessorMethods.Add(property.GetMethod);
		//		accessorMethods.Add(property.SetMethod);
		//		if (property.HasOtherMethods)
		//		{
		//			foreach (var m in property.OtherMethods)
		//				accessorMethods.Add(m);
		//		}
		//	}
		//	foreach (var ev in type.Events)
		//	{
		//		accessorMethods.Add(ev.AddMethod);
		//		accessorMethods.Add(ev.RemoveMethod);
		//		accessorMethods.Add(ev.InvokeMethod);
		//		if (ev.HasOtherMethods)
		//		{
		//			foreach (var m in ev.OtherMethods)
		//				accessorMethods.Add(m);
		//		}
		//	}
		//	return accessorMethods;
		//}
	}
}
