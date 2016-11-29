 // ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodReferenceExtensions
	{
		/// <summary>
		/// Determines if two <see cref="MethodReference"/> are equivalent.
		/// Because sadly, this feature is not implemented in TypeReference
		/// </summary>
		public static bool SafeEquals(this MethodReference a, MethodReference b, bool fullCompare = false)
		{
			if (a == null || b == null)
				return (a == null) == (b == null);
			if (fullCompare && a.GenericParameters.Count != b.GenericParameters.Count) // todo: check if this is the right way ..
				return false;
			return a.DeclaringType.GetILType() == b.DeclaringType.GetILType() && a.DeclaringType.FullName == b.DeclaringType.FullName && a.Name == b.Name;
		}
	}
}