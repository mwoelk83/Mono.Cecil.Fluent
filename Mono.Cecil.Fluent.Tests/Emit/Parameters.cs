﻿using System;
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
    public class LdArg : TestsBase
	{
        [TestMethod]
        public void load_this_parameter () =>
            CreateMethod()
                .AppendIL()
                    .LdThis()
				.Body.Instructions.First().OpCode.Should().Equal(OpCodes.Ldarg_0);

        [TestMethod]
        public void load_arg_and_return_parameter () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter<string>()
                .AppendIL()
				    .Ldarg(0)
				    .Ret()
			    .Compile<Func<string,string>>()
			    ("teststring").Should().Equal("teststring");

        [TestMethod]
        public void load_arg_and_return_parameter_use_ld_param () =>
			CreateStaticMethod()
				.Returns<double>()
				.WithParameter<double>()
                .AppendIL()
                    .LdParam(0)
				    .Ret()
			    .Compile<Func<double,double>>()
			    (10.01d).Should().Equal(10.01d);

        [TestMethod]
        public void load_named_arg_and_return_parameter () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter<string>("arg1")
                .AppendIL()
                    .Ldarg("arg1")
				    .Ret()
			    .Compile<Func<string, string>>()
			    ("teststring1").Should().Equal("teststring1");

        [TestMethod]
        public void load_named_arg_and_return_parameter_use_ldparam () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter<string>("arg1")
                .AppendIL()
                    .LdParam("arg1")
				    .Ret()
			    .Compile<Func<string, string>>()
			    ("test100").Should().Equal("test100");

		static readonly ParameterDefinition testparam  = new ParameterDefinition(TestModule.TypeSystem.String);

        [TestMethod]
        public void load_arg_and_return_with_parameterdefinition () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter(testparam)
                .AppendIL()
                    .Ldarg(testparam)
				    .Ret()
			    .Compile<Func<string, string>>()
			    ("teststring2").Should().Equal("teststring2");

        [TestMethod]
        public void store_arg_with_parameterdefinition () =>
			CreateStaticMethod()
				.Returns<string>()
				.WithParameter(testparam)
                .AppendIL()
                    .LdStr("otherstring")	
				    .Starg(testparam)
				    .Ldarg(testparam)
				    .Ret()
			    .Compile<Func<string, string>>()
			    ("otherstring").Should().Equal("otherstring");

        [TestMethod]
        public void load_many_args () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
                .AppendIL()
                    .Ldarg(0, 1, 2, 3, 4)
				    .Add()
				    .Add()
				    .Add()
				    .Add()
				    .Ret()
			    .Compile<Func<int,int,int,int,int,int>>()
			    (1,2,3,4,5).Should().Equal(1 + 2 + 3 + 4 + 5);

        [TestMethod]
        public void store_values_in_many_args () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
				.WithParameter<int>()
                .AppendIL()
                    .Ldc(6, 7, 8, 9, 10)
				    .Starg(0, 1, 2, 3, 4)
				    .Ldarg(0, 1, 2, 3, 4)
				    .Add()
				    .Add()
				    .Add()
				    .Add()
				    .Ret()
			    .Compile<Func<int, int, int, int, int, int>>()
			    (1, 2, 3, 4, 5).Should().Equal(6 + 7 + 8 + 9 + 10);

        [TestMethod]
        public void store_numberparameters_in_args () =>
			CreateStaticMethod()
				.Returns<long>()//.DebuggerBreak()
				.WithParameter<long>("arg1")
				.WithParameter<long>("arg2")
                .AppendIL()
                    .Starg(100L, "arg1", "arg2")
				    .Ldarg(0, 1)
				    .Add()
				    .Ret()
			    .Compile<Func<long,long,long>>()
			    (10,10).Should().Equal(200L);

        [TestMethod]
        public void store_value_in_named_arg () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithParameter<int>("arg1")
                .AppendIL()
                    .Ldc(10)
				    .Starg("arg1")
				    .Ldarg("arg1")
				    .Ret()
			    .Compile<Func<int, int>>()
			    (100).Should().Equal(10);

        [TestMethod]
        public void store_and_load_params_with_differnt_types () =>
			CreateStaticMethod()
				.Returns<double>()
				.WithParameter<long>("arg1")
				.WithParameter<int>("arg2")
				.WithParameter<float>("arg3")
				.WithParameter<double>("arg4")
                .AppendIL()
                    .Starg(10, "arg1", "arg2", "arg3", "arg4")
				    .Ldarg(0)
				    .Ldarg(1)
				    .ConvI8()
				    .Add()
				    .ConvR4()
				    .Ldarg(2)
				    .Add()
				    .ConvR8()
				    .Ldarg(3)
				    .Add()
				    .Ret()
			    .Compile<Func<long,int,float,double,double>>()
			    (10,10,10,10).Should().Equal(40.0d);
	}
}
