
namespace Mono.Cecil.Fluent
{
	partial class MethodDefinitionExtensions
	{
        /// <summary>
        /// Compilation of MethodDefinition and bind to given to Func or Action signature.
        /// </summary>
		public static T Compile<T>(this MethodDefinition method)
			where T : class
		{
			return new FluentMethodBody(method).Compile<T>();
		}
	}
}
