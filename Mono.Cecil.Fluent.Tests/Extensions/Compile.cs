using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Extensions
{
    [TestClass]
    public class ToDynamicMethod : TestsBase
    {
        [TestMethod]
		public void create_simple_dynamic_method_that_returns_a_string() =>
			CreateStaticMethod()
			.Returns<string>()
            .AppendIL()
                .LdStr("teststring")
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal("teststring");

        [TestMethod]
        public void compile_method_to_function () => 
			CreateStaticMethod()
			.WithParameter<int>()
			.Returns<float>()
            .AppendIL()
                .LdParam(0)
				.ConvR4()
				.Ldc(10f)
				.Div()
				.Ret()
			.Compile<Func<int,float>>()(100).Should().Equal(10f);
	}
}
