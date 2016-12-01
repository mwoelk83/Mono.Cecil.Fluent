using System;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable UnusedMember.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming

namespace Mono.Cecil.Fluent.Tests.Emit.System
{
	public class System_Math : TestsBase
	{
		It should_return_abs_of_positive_I4 = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(int.MaxValue)
				.System.Math.Abs<int>()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(int.MaxValue);

		It should_return_abs_of_negative_I4 = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(int.MinValue + 1)
				.System.Math.Abs<int>()
				.Ret()
			.Compile<Func<int>>()
			().Should().Equal(int.MaxValue);

		It should_return_abs_of_positive_I8 = () =>
			CreateStaticMethod()
			.Returns<long>()
            .AppendIL()
                .Ldc(100L)
				.System.Math.Abs<long>()
				.Ret()
			.Compile<Func<long>>()
			().Should().Equal(100L);

		It should_return_abs_of_negative_I8 = () =>
			CreateStaticMethod()
			.Returns<long>()
            .AppendIL()
                .Ldc(long.MinValue + 1)
				.System.Math.Abs<long>()
				.Ret()
			.Compile<Func<long>>()
			().Should().Equal(long.MaxValue);

		It should_return_abs_of_positive_R4 = () =>
			CreateStaticMethod()
			.Returns<float>()
            .AppendIL()
                .Ldc(100f)
				.System.Math.Abs<float>()
				.Ret()
			.Compile<Func<float>>()
			().Should().Equal(100f);

		It should_return_abs_of_negative_R4 = () =>
			CreateStaticMethod()
			.Returns<float>()
            .AppendIL()
                .Ldc(-100f)
				.System.Math.Abs<float>()
				.Ret()
			.Compile<Func<float>>()
			().Should().Equal(100f);

		It should_return_abs_of_positive_R8 = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.System.Math.Abs<double>()
				.Ret()
			.Compile<Func<double>>()
			()
			.Should().Equal(100d);

		It should_return_abs_of_negative_R8 = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(-100d)
				.System.Math.Abs<double>()
				.Ret()
			.Compile<Func<double>>()
			().Should().Equal(100d);

		It should_return_sqrt_for_double = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.System.Math.Sqrt()
				.Ret()
			.Compile<Func<double>>()
			().Should().Equal(10d);

		It should_return_sqrt_for_float = () =>
			CreateStaticMethod()
			.Returns<float>()
            .AppendIL()
                .Ldc(100f)
				.System.Math.Sqrt()
				.Ret()
			.Compile<Func<float>>()
			().Should().Equal(10f);

		It should_return_acost = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(0.56d)
				.System.Math.Acos()
				.Ret()
			.Compile<Func<double>>()
			().Should().Equal(Math.Acos(.56d));

		It should_return_asin = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(0.56d)
				.System.Math.Asin()
				.Ret()
			.Compile<Func<double>>()
			().Should().Equal(Math.Asin(.56d));

		It should_return_atan = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(0.56d)
				.System.Math.Atan()
				.Ret()
			.Compile<Func<double>>()
			().Should().Equal(Math.Atan(.56d));

		It should_return_atan2 = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(0.56d)
				.Ldc(2.34d)
				.System.Math.Atan2()
				.Ret()
			.Compile<Func<double>>()
			().Should().Equal(Math.Atan2(.56d, 2.34d));

		It should_return_bigmul = () =>
			CreateStaticMethod()
			.Returns<long>()
            .AppendIL()
                .Ldc(int.MaxValue)
				.Ldc(58324708)
				.System.Math.BigMul()
				.Ret()
			.Compile<Func<long>>()
			().Should().Equal(Math.BigMul(int.MaxValue, 58324708));

		It should_return_bigmul_with_arg = () =>
			CreateStaticMethod()
			.Returns<long>()
            .AppendIL()
                .Ldc(int.MaxValue)
				.System.Math.BigMul(58324708)
				.Ret()
			.Compile<Func<long>>()
			().Should().Equal(Math.BigMul(int.MaxValue, 58324708));

		It should_return_ceiling_of_double = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(9.32644)
				.System.Math.Ceiling()
				.Ret()
			.Compile<Func<double>>()
			().Should().Equal(Math.Ceiling(9.32644d));

		// todo: fix #9
		//It should_return_ceiling_of_decimal = () =>
		//	CreateStaticMethod()
		//	.Returns<decimal>()
		//		.Ldc((decimal) 9.32644)
		//		.System.Math.Ceiling()
		//		.Ret()
		//	.Compile<Func<double>>()
		//	.Should().Equal(Math.Ceiling((decimal) 9.32644));

		It should_return_cos = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(0.56d)
				.System.Math.Cos()
				.Ret()
			.Compile<Func<double>>()
			().Should().Equal(Math.Cos(.56d));


		It should_return_cosh = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(0.56d)
				.System.Math.Cosh()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Cosh(.56d));

		static int _divrem32out;
		It should_return_divrem_of_i32 = () =>
			CreateStaticMethod()
			.Returns<int>()
			.WithVariable<int>("refvar")
            .AppendIL()
                .Ldc(4345)
				.Ldc(534678)
				.Ldloca("refvar")
				.System.Math.DivRemInt32()
				.Ret()
			.Compile<Func<int>>()
			 ().Should().Equal(Math.DivRem(4345, 534678, out _divrem32out));

		static long _divrem64out;
		It should_return_divrem_of_i64 = () =>
			CreateStaticMethod()
			.WithVariable<long>("refvar")
			.Returns<long>()
            .AppendIL()
                .Ldc(4345L)
				.Ldc(534678L)
				.Ldloca("refvar")
				.System.Math.DivRemInt64()
				.Ret()
			.Compile<Func<long>>()
			 ().Should().Equal(Math.DivRem(4345L, 534678L, out _divrem64out));

		It should_return_exp = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(0.56d)
				.System.Math.Exp()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Exp(.56d));

		It should_return_floor_double = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(7.56d)
				.System.Math.Floor()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Floor(7.56d));

		// todo: fix #9
		//It should_return_floor_decimal = () =>
		//	CreateStaticMethod()
		//	.Returns<decimal>()
		//		.Ldc((decimal)7.56d)
		//		.System.Math.Floor()
		//		.Ret()
		//	.Compile<Func<double>>()
		//	 ().Should().Equal(Math.Floor((decimal)7.56d));
		
		It should_return_ieeeremainder = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(.235d)
				.Ldc(.3254d)
				.System.Math.IEEERemainder()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.IEEERemainder(.235d, .3254d));
		
		It should_return_log = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(.235d)
				.Ldc(.3254d)
				.System.Math.Log()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Log(.235d, .3254d));
		
		It should_return_log_with_arg = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(.235d)
				.System.Math.Log(.3254d)
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Log(.235d, .3254d));
		
		It should_return_log10 = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(.235d)
				.System.Math.Log10()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Log10(.235d));
		
		It should_return_max_int = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(100)
				.Ldc(33)
				.System.Math.Max<int>()
				.Ret()
			.Compile<Func<int>>()
			 ().Should().Equal(Math.Max(100, 33));
		
		It should_return_max_long = () =>
			CreateStaticMethod()
			.Returns<long>()
            .AppendIL()
                .Ldc(100L)
				.Ldc(33L)
				.System.Math.Max<long>()
				.Ret()
			.Compile<Func<long>>()
			 ().Should().Equal(Math.Max(100L, 33L));

		It should_return_max_float = () =>
			CreateStaticMethod()
			.Returns<float>()
            .AppendIL()
                .Ldc(100f)
				.Ldc(33f)
				.System.Math.Max<float>()
				.Ret()
			.Compile<Func<float>>()
			 ().Should().Equal(Math.Max(100f, 33f));
		
		It should_return_max_double = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.Ldc(33d)
				.System.Math.Max<double>()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Max(100d, 33d));
		
		It should_return_min_int = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(100)
				.Ldc(33)
				.System.Math.Min<int>()
				.Ret()
			.Compile<Func<int>>()
			 ().Should().Equal(Math.Min(100, 33));
		
		It should_return_min_long = () =>
			CreateStaticMethod()
			.Returns<long>()
            .AppendIL()
                .Ldc(100L)
				.Ldc(33L)
				.System.Math.Min<long>()
				.Ret()
			.Compile<Func<long>>()
			 ().Should().Equal(Math.Min(100L, 33L));

		It should_return_min_float = () =>
			CreateStaticMethod()
			.Returns<float>()
            .AppendIL()
                .Ldc(100f)
				.Ldc(33f)
				.System.Math.Min<float>()
				.Ret()
			.Compile<Func<float>>()
			 ().Should().Equal(Math.Min(100f, 33f));
		
		It should_return_min_double = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.Ldc(33d)
				.System.Math.Min<double>()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Min(100d, 33d));
		
		It should_return_pow = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.Ldc(33d)
				.System.Math.Pow()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Pow(100d, 33d));
		
		It should_return_pow_witharg = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.System.Math.Pow(33d)
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Pow(100d, 33d));

		It should_return_round = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(1.5d)
				.System.Math.Round()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Round(1.5d));
		
		It should_return_round_withprecision = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(1.5d)
				.System.Math.Round(4)
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Round(1.5d, 4));
		
		It should_return_sign_int = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(-100)
				.System.Math.Sign<int>()
				.Ret()
			.Compile<Func<int>>()
			 ().Should().Equal(Math.Sign(-100));
		
		It should_return_sign_long = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(100L)
				.System.Math.Sign<long>()
				.Ret()
			.Compile<Func<int>>()
			 ().Should().Equal(Math.Sign(100L));


		It should_return_sign_float = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(-100f)
				.System.Math.Sign<float>()
				.Ret()
			.Compile<Func<int>>()
			 ().Should().Equal(Math.Sign(-100f));
		
		It should_return_sign_double = () =>
			CreateStaticMethod()
			.Returns<int>()
            .AppendIL()
                .Ldc(-100d)
				.System.Math.Sign<double>()
				.Ret()
			.Compile<Func<int>>()
			 ().Should().Equal(Math.Sign(-100d));

		It should_return_sin = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.System.Math.Sin()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Sin(100d));

		It should_return_sinh = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.System.Math.Sinh()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Sinh(100d));

		It should_return_tan = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.System.Math.Tan()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Tan(100d));

		It should_return_tanh = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.System.Math.Tanh()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Tanh(100d));

		It should_return_truncate = () =>
			CreateStaticMethod()
			.Returns<double>()
            .AppendIL()
                .Ldc(100d)
				.System.Math.Truncate()
				.Ret()
			.Compile<Func<double>>()
			 ().Should().Equal(Math.Truncate(100d));

  //      //todo: fix #9
		//It should_return_truncatedecimal = () =>
  //          CreateStaticMethod()
		//	.Returns<double>()
		//		.Ldc((decimal)100.51d)
		//		.System.Math.TruncateDecimal()
		//		.Ret()
		//	.Compile<Func<decimal>>()
		//	 ().Should().Equal(Math.Truncate((decimal)100.51d));
	}
}
