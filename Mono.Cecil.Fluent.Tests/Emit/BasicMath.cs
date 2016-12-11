using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Emit
{
    [TestClass]
    public class BasicMath : TestsBase
    {
        [TestMethod]
        public void return_remainder_of_ten_by_four () =>
			CreateStaticMethod()
			.Returns<byte>()
            .AppendIL()
				.Ldc((byte)10)
				.Rem(4)
				.Ret()
			.Compile<Func<byte>>()
			().Should().Equal((byte)2);

        [TestMethod]
        public void return_remainder_of_twenty_by_seven () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc((ushort)20)
				.Ldc((short)7)
				.Rem()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(6);

        [TestMethod]
        public void return_remainder_of_twenty_by_seven_unsigned () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(20)
				.RemUn(7)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(6);

        [TestMethod]
        public void return_remainder_of_tenmillion_by_1303 () =>
			CreateStaticMethod()
			.Returns<uint>()
            .AppendIL()
                .Ldc(10000000U)
				.Ldc(1303U)
				.RemUn()
				.Ret()
			.Compile<Func<uint>>()
			().Should().Equal(778U);

        [TestMethod]
        public void return_remainder_of_tenmillion_by_1303_ulong () =>
			CreateStaticMethod()
			.Returns<ulong>()
            .AppendIL()
                .Ldc(10000000UL)
				.Ldc(1303UL)
				.RemUn()
				.Ret()
			.Compile<Func<ulong>>()
			().Should().Equal(778UL);

        [TestMethod]
        public void add_3 () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(0)
				.Ldc(3)
				.Add()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(3);

		public void add_3_with_arg () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(0)
				.Add(3u)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(3);

        [TestMethod]
        public void sub_3 () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(0)
				.Ldc(3)
				.Sub()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(-3);

        [TestMethod]
        public void sub_3_with_arg () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(0U)
				.Sub(3)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(-3);

        [TestMethod]
        public void mul_by_3 () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(5)
				.Ldc(3)
				.Mul()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(15);

        [TestMethod]
        public void mul_by_3_with_arg () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(5)
				.Mul(3U)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(15);

        [TestMethod]
        public void div_by_3 () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(15)
				.Ldc(3)
				.Div()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(5);

        [TestMethod]
        public void div_by_3_with_arg () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(15)
				.Div(3U)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(5);

        [TestMethod]
        public void div_by_3_un () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(15)
				.Ldc(3)
				.DivUn()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(5);

        [TestMethod]
        public void div_by_3_with_arg_un () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(15)
				.DivUn(3U)
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(5);

        [TestMethod]
        // improves code coverage
        public void add_a_bool_and_a_sbyte () => 
			CreateStaticMethod()
			.Returns<byte>()
            .AppendIL()
                .Ldc(true)
			    .Ldc((sbyte) 2)
			    .Add()
			    .Ret()
			.Compile<Func<byte>>()
			().Should().Equal((byte) 3);
	}
}
