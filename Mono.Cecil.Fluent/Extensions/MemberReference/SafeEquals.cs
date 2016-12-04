 // ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class IMemberReferenceExtensions
	{
		/// <summary>
		/// Determines if two <see cref="MemberReference"/>s are equivalent.
		/// Because sadly, this feature is not implemented in TypeReference
		/// </summary>
		public static bool SafeEquals<T>(this T a, T b)
            where T : MemberReference
		{
			if (a == null || b == null)
				return (a == null) == (b == null);

            // for Types
            if(typeof(T) == typeof(TypeReference) || typeof(T).IsSubclassOf(typeof(TypeReference)))
                return (a as TypeReference)?.GetILType() == (b as TypeReference)?.GetILType() && a.FullName == b.FullName;

            return a.DeclaringType.GetILType() == b.DeclaringType.GetILType() && a.DeclaringType.FullName == b.DeclaringType.FullName && a.Name == b.Name;
		}
	}
}