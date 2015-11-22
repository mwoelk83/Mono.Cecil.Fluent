using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class FluentMethodBody_Conv : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static FluentMethodBody NewTestMethod => new FluentMethodBody(CreateMethod());

		It should_conv_I8_to_I = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc((long)int.MaxValue + 1)
				.ConvI()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(int.MinValue);

		It should_conv_I8_to_I1 = () =>
			CreateStaticMethod()
			.Returns<sbyte>()
				.Ldc((long)sbyte.MaxValue + 1)
				.ConvI1()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(sbyte.MinValue);

		It should_conv_I8_to_I2 = () =>
			CreateStaticMethod()
			.Returns<short>()
				.Ldc((long)short.MaxValue + 1)
				.ConvI2()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(short.MinValue);

		It should_conv_I8_to_I4 = () =>
			CreateStaticMethod()
			.Returns<int>()
				.Ldc((long) int.MaxValue + 1)
				.ConvI4()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(int.MinValue);

		It should_conv_I4_to_I8 = () =>
			CreateStaticMethod()
			.Returns<long>()
				.Ldc(100)
				.ConvI8()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(100L);

		It should_conv_I8_to_U = () =>
			CreateStaticMethod()
			.Returns<uint>()
				.Ldc((long)int.MaxValue + 1)
				.ConvU()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal((uint.MaxValue >> 1) + 1);

		It should_conv_I8_to_U1 = () =>
			CreateStaticMethod()
			.Returns<byte>()
				.Ldc((long)sbyte.MaxValue + 1)
				.ConvU1()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal((byte)128);

		It should_conv_I8_to_U2 = () =>
			CreateStaticMethod()
			.Returns<ushort>()
				.Ldc((long)short.MaxValue + 1)
				.ConvU2()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal((ushort)((ushort.MaxValue >> 1) + 1));

		It should_conv_I8_to_U4 = () =>
			CreateStaticMethod()
			.Returns<uint>()
				.Ldc((long)int.MaxValue + 1)
				.ConvU4()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal((uint.MaxValue >> 1) + 1);

		It should_conv_I4_to_U8 = () =>
			CreateStaticMethod()
			.Returns<ulong>().DebuggerBreak()
				.Ldc(int.MinValue)
				.ConvU8()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal((ulong.MaxValue >> 33) + 1);

		It should_conv_R4_to_R8 = () =>
			CreateStaticMethod()
				.Returns<double>()
				.Ldc(1.01f)
				.ConvR8()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal((double)1.01f);

		It should_conv_R8_to_R4 = () =>
			CreateStaticMethod()
			.Returns<float>()
				.Ldc(1.01d)
				.ConvR4()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(1.01f);
	}
}
