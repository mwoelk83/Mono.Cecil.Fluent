using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    partial class FluentEmitter
    {
        public FluentEmitter Call(MethodReference m)
        {
            return Emit(OpCodes.Call, m);
        }
    }
}
