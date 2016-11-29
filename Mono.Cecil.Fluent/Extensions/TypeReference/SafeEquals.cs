 // ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class TypeReferenceExtensions
	{
		public static bool SafeEquals<T>(this TypeReference that)
        {
            if (that == null)
                return false;
            var b = that.Module.SafeImport(typeof(T));
            return that.GetILType() == b.GetILType() && that.FullName == b.FullName;
        }

		/// <summary>
		/// Determines if two TypeReferences are equivalent.
		/// Because sadly, this feature is not implemented in TypeReference
		/// </summary>
		public static bool SafeEquals(this TypeReference a, TypeReference b)
		{
			if (a == null || b == null)
				return (a == null) == (b == null);
			return a.GetILType() == b.GetILType() && a.FullName == b.FullName;
		}
	}
}