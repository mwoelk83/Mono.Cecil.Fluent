using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Emit
{
    [TestClass]
    public class Basics : TestsBase
    {
        [TestMethod]
        public void dup_value () =>
			CreateStaticMethod()
				.Returns<int>()
                .AppendIL()
				    .Ldc(10)
				    .Dup()
				    .Add()
				    .Ret()
			    .Compile<Func<int>>()
			    ().Should().Equal(20);

        [TestMethod]
        public void add_nops () =>
			CreateStaticMethod()
				.ReturnsVoid()
                .AppendIL()
                    .Nop()
				    .Nop()
				    .Nop()
			    .Body.Instructions.Count.Should().Equal(3);

        [TestMethod]
        public void pop_value () =>
			CreateStaticMethod()
				.ReturnsVoid()
                .AppendIL()
                    .Ldc(0)
				    .Pop()
				    .Ret()
			    .Compile<Action>()();

        [TestMethod]
        public void invert_bitwise () =>
			CreateStaticMethod()
				.Returns<ushort>()
                .AppendIL()
                    .Ldc((ushort)0x00FF)
				    .Not()
				    .Ret()
			    .Compile<Func<ushort>>()
			    ().Should().Equal((ushort)0xFF00);

        [TestMethod]
        public void ldnull () =>
			CreateStaticMethod()
				.Returns<object>()
                .AppendIL()
                    .LdNull()
				    .Ret()
			    .Compile<Func<object>>()
			    ().Should().Equal(null);

        [TestMethod]
        public void ret_int () =>
			CreateStaticMethod()
				.Returns<int>()
                .AppendIL()
                    .Ret(10d)
			    .Compile<Func<int>>()
			    ().Should().Equal(10);

        [TestMethod]
        public void ret_long () =>
			CreateStaticMethod()
				.Returns<long>()
                .AppendIL()
                    .Ret(1f)
			    .Compile<Func<long>>()
			    ().Should().Equal(1L);

        [TestMethod]
        public void ret_float () =>
			CreateStaticMethod()
				.Returns<float>()
                .AppendIL()
                    .Ret(10d)
			    .Compile<Func<float>>()
			    ().Should().Equal(10f);

        [TestMethod]
        public void ret_double () =>
			CreateStaticMethod()
				.Returns<double>()
                .AppendIL()
                    .Ret(10f)
			    .ToDynamicMethod()
			    .Invoke(null, null).Should().Equal(10d);

        [TestMethod]
        public void ret_loc_with_name () =>
			CreateStaticMethod()
				.WithVariable<int>("ret")
				.Returns<int>()
                .AppendIL()
                    .Stloc(100, "ret")
				    .RetLoc("ret")
			    .Compile<Func<int>>()
			    ().Should().Equal(100);

        [TestMethod]
        public void ret_loc_with_index () =>
			CreateStaticMethod()
				.WithVariable<int>("ret")
				.Returns<int>()
                .AppendIL()
                    .Stloc(1100, "ret")
				    .RetLoc(0)
			    .Compile<Func<int>>()
			    ().Should().Equal(1100);

        [TestMethod]
        public void ret_arg_with_name () =>
			CreateStaticMethod()
				.WithParameter<int>("ret")
				.Returns<int>()
                .AppendIL()
                    .RetArg("ret")
			    .Compile<Func<int,int>>()
			    (100).Should().Equal(100);
	}
}
