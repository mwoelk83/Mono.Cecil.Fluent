using System;
using Machine.Specifications;
using Mono.Cecil.Cil;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Emit
{
	public class Locals : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static FluentEmitter NewTestMethod => new FluentEmitter(CreateMethod());

		It should_load_and_store_local = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithVariable<int>()
                .AppendIL()
                    .Ldc(1)
				    .Stloc(0)
				    .Ldc(2)
				    .Ldloc(0)
				    .Add()
				    .Ret()
			    .Compile<Func<int>>()
			    ().Should().Equal(3);

		It should_load_and_store_named_local = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithVariable<int>("var1")
                .AppendIL()
                    .Ldc(1)
				    .Stloc("var1")
				    .Ldc(2)
				    .Ldloc("var1")
				    .Add()
				    .Ret()
			    .Compile<Func<int>>()
			    ().Should().Equal(3);

		It should_store_locals_with_value = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithVariable<int>("var1")
				.WithVariable<int>("var2")
                .AppendIL()
                    .Stloc(1, "var1", "var2")
				    .Ldloc("var1", "var2")
				    .Add()
				    .Ret()
			    .Compile<Func<int>>()
			    ().Should().Equal(2);

		It should_store_and_load_many_locals_with_value = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithVariable<int>("var1")
				.WithVariable<int>("var2")
				.WithVariable<int>("var3")
				.WithVariable<int>("var4")
				.WithVariable<int>("var5")
				.WithVariable<int>("var6")//.DebuggerBreak()
                .AppendIL()
                    .Stloc(1, "var1", "var2", "var3", "var4", "var5", "var6")
				    .Ldloc("var1", "var2", "var3", "var4", "var5", "var6")
				    .Add()
				    .Add()
				    .Add()
				    .Add()
				    .Add()
				    .Ret()
			    .Compile<Func<int>>()
			    ().Should().Equal(6);

		static VariableDefinition vardef = new VariableDefinition(TestModule.SafeImport(typeof(bool)));

		It should_store_and_load_local_using_variabledefinition = () =>
			CreateStaticMethod()
				.Returns<bool>()
				.WithVariable(vardef)
                .AppendIL()
                    .Ldc(1)
				    .Stloc(vardef)
				    .Ldloc(vardef)
				    .Ret()
			    .Compile<Func<bool>>()
			    ().Should().Equal(true);
		
		It should_store_and_load_locals_with_differnt_types_with_manual_convert_instructions = () =>
			CreateStaticMethod()
				.Returns<double>()
				.WithVariable<long>("var1")
				.WithVariable<int>("var2")
				.WithVariable<float>("var3")
				.WithVariable<double>("var4")
                .AppendIL()
                    .Stloc(10, "var1", "var2", "var3", "var4")
				    .Ldloc(0)
				    .Ldloc(1)
				    .ConvI8()
				    .Add()
				    .ConvR4()
				    .Ldloc(2)
				    .Add()
				    .ConvR8()
				    .Ldloc(3)
				    .Add()
				    .Ret()
			    .Compile<Func<double>>()
			    ().Should().Equal(40.0d);
	}
}
