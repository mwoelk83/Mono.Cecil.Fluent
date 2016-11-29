
// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class IMethodSignatureExtensions
    {
		public static bool HasImplicitThis(this IMethodSignature self)
		{
			return self.HasThis && !self.ExplicitThis;
		}
	}
}
