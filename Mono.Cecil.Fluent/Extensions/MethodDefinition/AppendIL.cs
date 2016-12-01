
// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    partial class MethodDefinitionExtensions
    {
        /// <summary>
        /// Compilation of MethodDefinition and bind to given to Func or Action signature.
        /// </summary>
		public static FluentEmitter AppendIL(this MethodDefinition method)
        {
            return new FluentEmitter(method);
        }
    }
}
