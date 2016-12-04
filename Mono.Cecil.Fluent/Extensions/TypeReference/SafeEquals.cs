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
	}
}