using System;
using System.Linq;
using System.Reflection;
using Machine.Specifications;
using Mono.Cecil.Cil;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
    public class FluentMethodBody_NewObj : TestsBase
    {
        static readonly TypeDefinition TestType = CreateType();

        static FluentMethodBody NewTestMethod => new FluentMethodBody(CreateMethod());

        It should_emit_newobj_instruction = () =>
            NewTestMethod
                .NewObj<object>()
            .Body.Instructions.First().OpCode.Should().Equal(OpCodes.Newobj);

        It should_throw_exception_newobj_primitive_valuetype = () =>
        {
            var exceptionThrown = false;

            try
            {
                NewTestMethod
                    .Returns<bool>()
                        .Ldc(true)
                        .NewObj<bool>()
                        .Ret();
            } //ncrunch: no coverage
            catch { exceptionThrown = true; }

            if (!exceptionThrown)
                throw new Exception(); //ncrunch: no coverage
        };

        It should_emit_newobj_valuetype_with_arg_and_return_new_object = () =>
            CreateStaticMethod()
                .Returns<DateTime>()
                    .Ldc(DateTime.Today.Ticks)
                    .NewObj<DateTime>(typeof(long))
                    .Ret()
                .Compile<Func<DateTime>>()().Should().Equal(DateTime.Today);
    }
}
