using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Cil;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Emit
{
    [TestClass]
    public class NewObj : TestsBase
    {
        [TestMethod]
        public void emit_newobj_instruction () =>
            CreateMethod()
                .AppendIL()
                    .NewObj<object>()
            .Body.Instructions.First().OpCode.Should().Equal(OpCodes.Newobj);

        [TestMethod]
        public void throw_exception_newobj_primitive_valuetype ()
        {
            var exceptionThrown = false;

            try
            {
                CreateMethod()
                    .Returns<bool>()
                    .AppendIL()
                        .Ldc(true)
                        .NewObj<bool>()
                        .Ret();
            }
            catch { exceptionThrown = true; }

            if (!exceptionThrown)
                throw new Exception();
        }

        [TestMethod]
        public void emit_newobj_valuetype_with_arg_and_return_new_object () =>
            CreateStaticMethod()
                .Returns<DateTime>()
                .AppendIL()
                    .Ldc(DateTime.Today.Ticks)
                    .NewObj<DateTime>(typeof(long))
                    .Ret()
                .Compile<Func<DateTime>>()().Should().Equal(DateTime.Today);
    }
}
