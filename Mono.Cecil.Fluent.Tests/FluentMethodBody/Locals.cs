using Machine.Specifications;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.FluentMethodBody
{
	public class FluentMethodBody_Locals : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();

		static Fluent.FluentMethodBody NewTestMethod => new Fluent.FluentMethodBody(CreateMethod());

		It shoud_load_and_store_local = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithVariable<int>()
				.Ldc(1)
				.Stloc(0)
				.Ldc(2)
				.Ldloc(0)
				.Add()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(3);

		It shoud_load_and_store_named_local = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithVariable<int>("var1")
				.Ldc(1)
				.Stloc("var1")
				.Ldc(2)
				.Ldloc("var1")
				.Add()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(3);

		It shoud_store_locals_with_value = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithVariable<int>("var1")
				.WithVariable<int>("var2")
				.Stloc(1, "var1", "var2")
				.Ldloc("var1", "var2")
				.Add()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(2);

		It shoud_store_and_load_many_locals_with_value = () =>
			CreateStaticMethod()
				.Returns<int>()
				.WithVariable<int>("var1")
				.WithVariable<int>("var2")
				.WithVariable<int>("var3")
				.WithVariable<int>("var4")
				.WithVariable<int>("var5")
				.WithVariable<int>("var6")
				.Stloc(1, "var1", "var2", "var3", "var4", "var5", "var6")
				.Ldloc("var1", "var2", "var3", "var4", "var5", "var6")
				.Add()
				.Add()
				.Add()
				.Add()
				.Add()
				.Ret()
			.ToDynamicMethod()
			.Invoke(null, null).Should().Equal(6);
	}
}
