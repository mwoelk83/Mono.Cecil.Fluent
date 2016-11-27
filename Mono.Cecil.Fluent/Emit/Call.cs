using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    partial class FluentMethodBody
    {
        public FluentMethodBody Call(MethodReference m)
        {
            return Emit(OpCodes.Call, m);
        }
    }
}
