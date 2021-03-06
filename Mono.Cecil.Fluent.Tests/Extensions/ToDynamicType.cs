﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Fluent.Attributes;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Extensions
{
    [TestClass]
    public class Extensions_ToDynamicType : TestsBase
	{
        [TestMethod]
		public void invoke_instance_method_of_created_dynamic_type ()
		{
			var newtype = TestModule
				.CreateType()
				.WithField<string>("testfield1", FieldAttributes.FamANDAssem | FieldAttributes.Public)
				.CreateMethod<int>("newmethod")
                .SetMethodAttributes<FamANDAssem, Public>()
				.WithParameter<int>()
                .AppendIL()
					.LdParam(0)
					.Ret()
				.DeclaringType
				.ToDynamicType();

			newtype
				.GetMethod("newmethod")
				.Invoke(Activator.CreateInstance(newtype), new object[] {20}).Should().Equal(20);
		}
	}
}
