using System.Linq;
using Machine.Specifications;
using Mono.Cecil.Cil;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class FluentMethodBodyLdArg : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static FluentMethodBody NewTestMethod => new FluentMethodBody(CreateMethod());

		It should_load_this_parameter = () =>
			NewTestMethod.Ldthis()
				.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Ldarg_0);

		It should_load_arg_and_return_parameter = () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter<string>()
				.Ldarg(0)
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] { "teststring" }).Should().Equal("teststring");

		It should_load_arg_and_return_parameter_use_ld_param = () =>
			CreateStaticMethod()
				.Returns<double>()
				.WithParameter<double>()
				.LdParam(0)
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] { (object) 10.01d }).Should().Equal(10.01d);

		It should_load_named_arg_and_return_parameter = () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter<string>("arg1")
				.Ldarg("arg1")
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] { "teststring1" }).Should().Equal("teststring1");

		It should_load_named_arg_and_return_parameter_use_ldparam = () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter<string>("arg1")
				.LdParam("arg1")
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] { "test100" }).Should().Equal("test100");

		static readonly ParameterDefinition testparam  = new ParameterDefinition(TestModule.TypeSystem.String);

		It should_load_arg_and_return_with_parameterdefinition = () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter(testparam)
				.Ldarg(testparam)
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] { "teststring2" }).Should().Equal("teststring2");

		It should_store_arg_with_parameterdefinition = () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter(testparam)
				.LdStr("otherstring")	
				.Starg(testparam)
				.Ldarg(testparam)
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] { "teststring" }).Should().Equal("otherstring");

		It should_load_many_args = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.Ldarg(0, 1, 2, 3, 4)
				.Add()
				.Add()
				.Add()
				.Add()
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] {(object) 1, 2, 3, 4, 5}).Should().Equal(1 + 2 + 3 + 4 + 5);

		It should_store_values_in_many_args = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.Ldc(6, 7, 8, 9, 10)
				.Starg(0, 1, 2, 3, 4)
				.Ldarg(0, 1, 2, 3, 4)
				.Add()
				.Add()
				.Add()
				.Add()
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] { (object)1, 2, 3, 4, 5 }).Should().Equal(6 + 7 + 8 + 9 + 10);

		It should_store_numberparameters_in_args = () =>
			CreateStaticMethod()
				.Returns<long>().DebuggerBreak()
				.WithParameter<long>("arg1")
				.WithParameter<long>("arg2")
				.Starg(100L, "arg1", "arg2")
				.Ldarg(0, 1)
				.Add()
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new [] {(object) 10, 10}).Should().Equal(200L);

		It should_store_value_in_named_arg = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithParameter<int>("arg1")
				.Ldc(10)
				.Starg("arg1")
				.Ldarg("arg1")
				.Ret()
				.ToDynamicMethod()
				.Invoke(null, new[] { (object) 1 }).Should().Equal(10);
	}
}
