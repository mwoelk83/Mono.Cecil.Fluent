using System;
using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class Branches : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		It should_emit_if_block = () =>
			CreateStaticMethod()
                .Returns<int>()
				.WithParameter<bool>()
                .AppendIL()
				    .Ldarg(0)
				    .IfTrue()
					    .Ret(10)
				    .EndIf()
				    .Ret(100)
			    .Compile<Func<bool,int>>()
			    (false).Should().Equal(100);

		It should_emit_ifnot_block = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithParameter<bool>()
                .AppendIL()
                    .Ldarg(0)
				    .IfNot()
					    .Ret(10)
				    .EndIf()
				    .Ret(100)
			    .Compile<Func<bool,int>>()
			    (false).Should().Equal(10);

		static Func<long, long, bool> LessThenMethod => CreateStaticMethod()
			.Returns<bool>()
			.WithParameter<long>()
			.WithParameter<long>()
            .AppendIL()
                .Ldarg(0)
				.Ldarg(1)
				.Iflt()
					.Ret(true)
				.EndIf()
				.Ret(false)
			.Compile<Func<long,long,bool>>();

		It should_be_less_than = () => LessThenMethod(50, 100).Should().Equal(true);
		It should_not_be_less_than = () => LessThenMethod(500, 100).Should().Equal(false);

		static Func<int, int, bool> GreaterThanMethod => CreateStaticMethod()
			.Returns<bool>()
			.WithParameter<int>()
			.WithParameter<int>()
            .AppendIL()
                .Ldarg(0)
				.Ldarg(1)
				.Ifgt()
					.Ret(true)
				.EndIf()
				.Ret(false)
			.Compile<Func<int,int,bool>>();

		It should_be_greater_than = () => GreaterThanMethod(500, 100).Should().Equal(true);
		It should_not_be_greater_than = () => GreaterThanMethod(50, 100).Should().Equal(false);

		static Func<long, long, bool> LessThenOrEqualMethod => CreateStaticMethod()
			.Returns<bool>()
			.WithParameter<long>()
			.WithParameter<long>()
            .AppendIL()
                .Ldarg(0)
				.Ldarg(1)
				.Iflte()
					.Ret(true)
				.EndIf()
				.Ret(false)
			.Compile<Func<long, long, bool>>();

		It should_be_less_than_or_equal = () => LessThenOrEqualMethod(100, 100).Should().Equal(true);
		It should_be_less_than_or_equal_2 = () => LessThenOrEqualMethod(7, 100).Should().Equal(true);
		It should_not_be_less_than_or_equal = () => LessThenOrEqualMethod(500, 100).Should().Equal(false);

		static  Func<int, int, bool> GreaterThanOrEqualMethod => CreateStaticMethod()
			.Returns<bool>()
			.WithParameter<int>()
			.WithParameter<int>()
            .AppendIL()
                .Ldarg(0)
				.Ldarg(1)
				.Ifgte()
					.Ret(true)
				.EndIf()
				.Ret(false)
			.Compile<Func<int, int, bool>>();

		It should_be_greater_than_or_equal = () => GreaterThanOrEqualMethod(100, 100).Should().Equal(true);
		It should_be_greater_than_or_equal_2 = () => GreaterThanOrEqualMethod(1000, 100).Should().Equal(true);
		It should_not_be_greater_than_or_equal = () => GreaterThanOrEqualMethod(5, 100).Should().Equal(false);

		static Func<int, int, int> NestedNumberComparisonFunction => CreateStaticMethod()
			.Returns<int>()
			.WithParameter<int>()
			.WithParameter<int>()
            .AppendIL()
                .Ldarg(0)
				.Ldarg(1)
				.Ifgt()
					.Ret(1)
				.Else()
					.Ldarg(0)
					.Ldarg(1)
					.Iflt()
						.Ret(-1)
					.Else()
						.Ret(0)
					.EndIf()
				.EndIf()
			.Compile<Func<int, int, int>>();

		It should_compare_numbers = () => NestedNumberComparisonFunction(100, 100).Should().Equal(0);
		It should_compare_numbers_2 = () => NestedNumberComparisonFunction(1000, 100).Should().Equal(1);
		It should_compare_numbers_3 = () => NestedNumberComparisonFunction(5, 100).Should().Equal(-1);

		static Func<int, int, int> NestedNumberComparisonFunctionWithLocal => CreateStaticMethod()
			.Returns<int>()
			.WithParameter<int>()
			.WithParameter<int>()
			.WithVariable<int>("ret")
            .AppendIL()
                .Ldarg(0)
				.Ldarg(1)
				.Ifgt()
					.Stloc(1, "ret")
				.Else()
					.Ldarg(0)
					.Ldarg(1)
					.Iflt()
						.Stloc(-1, "ret")
					.Else()
						.Stloc(0, "ret")
					.EndIf()
				.EndIf()
				.RetLoc("ret")
			.Compile<Func<int, int, int>>();

		It should_compare_numbers_with_locals = () => NestedNumberComparisonFunctionWithLocal(100, 100).Should().Equal(0);
		It should_compare_numbers_with_locals_2 = () => NestedNumberComparisonFunctionWithLocal(1000, 100).Should().Equal(1);
		It should_compare_numbers_with_locals_3 = () => NestedNumberComparisonFunctionWithLocal(5, 100).Should().Equal(-1);
	}
}
