 // ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	/// <summary>
	/// Extensions to TypeReference
	/// </summary>
	public static partial class TypeReferenceExtensions
	{
		public static bool Is<T>(this TypeReference that)
		{
			return SafeEquals(that, that.Module.SafeImport(typeof(T)));
		}

		/// <summary>
		/// Determines if two TypeReferences are equivalent.
		/// Because sadly, this feature is not implemented in TypeReference
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public static bool SafeEquals(this TypeReference a, TypeReference b)
		{
			if (a == null || b == null)
				return (a == null) == (b == null);
			return a.GetILType() == b.GetILType() && a.FullName == b.FullName;
		}

		/// <summary>
		/// Determines if two <see cref="MethodReference"/> are equivalent.
		/// Because sadly, this feature is not implemented in TypeReference
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <param name="fullCompare">if set to <c>true</c> [full compare].</param>
		/// <returns></returns>
		public static bool SafeEquals(this MethodReference a, MethodReference b, bool fullCompare = false)
		{
			if (a == null || b == null)
				return (a == null) == (b == null);
			if (fullCompare && a.GenericParameters.Count != b.GenericParameters.Count)
				return false;
			return a.DeclaringType.GetILType() == b.DeclaringType.GetILType() && a.DeclaringType.FullName == b.DeclaringType.FullName && a.Name == b.Name;
		}
	}
}