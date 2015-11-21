using Machine.Specifications;
using Mono.Cecil.Fluent.Attributes;
using Mono.Cecil.Fluent.Utils;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_SetAttributes : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();
		static readonly MethodDefinition TestMethod = CreateMethod();

		static PropertyDefinition CreateProperty()
		{
			var prop = new PropertyDefinition(Generate.Name.ForMethod(), PropertyAttributes.None, TestModule.TypeSystem.Boolean);
			TestType.Properties.Add(prop);
			prop.DeclaringType = TestType;
			return prop;
		}

		It should_set_attributes_for_method = () =>
			TestMethod
				.SetAttributes(MethodAttributes.Abstract, MethodAttributes.Final)
				.Attributes.Should().Equal(MethodAttributes.Family | MethodAttributes.Abstract | MethodAttributes.Final);

		It should_set_attributes_for_type = () =>
			TestType
				.SetAttributes(TypeAttributes.Abstract, TypeAttributes.Class)
				.Attributes.Should().Equal(TypeAttributes.Abstract | TypeAttributes.Class);

		It should_set_attributes_for_event = () =>
			CreateEvent()
				.SetAttributes(EventAttributes.SpecialName | EventAttributes.RTSpecialName)
				.Attributes.Should().Equal(EventAttributes.SpecialName | EventAttributes.RTSpecialName);

		It should_set_attributes_for_property = () =>
			CreateProperty()
				.SetAttributes(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName)
				.Attributes.Should().Equal(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName);

		It should_set_attributes_for_field = () =>
			CreateField()
				.SetAttributes(FieldAttributes.Assembly | FieldAttributes.Family)
				.Attributes.Should().Equal(FieldAttributes.Assembly | FieldAttributes.Family);

		It should_set_attributes_for_method_generic_part1 = () =>
			TestMethod
				.UnsetAllAttributes()
				.SetAttributes<MemberAccessMask, Private, FamANDAssem, HideBySig, NewSlot, Public, Final, Virtual>()
				.Attributes.Should().Equal(MethodAttributes.MemberAccessMask | MethodAttributes.Private | MethodAttributes.FamANDAssem | MethodAttributes.HideBySig | MethodAttributes.NewSlot 
					| MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual);

		It should_set_attributes_for_method_generic_part2 = () =>
			TestMethod
				.UnsetAllAttributes()
				.SetAttributes<Assembly, Family, Static, VtableLayoutMask, CheckAccessOnOverride, Abstract, SpecialName, PInvokeImpl>()
				.Attributes.Should().Equal(MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.VtableLayoutMask | MethodAttributes.CheckAccessOnOverride
				| MethodAttributes.Family | MethodAttributes.Abstract | MethodAttributes.SpecialName | MethodAttributes.PInvokeImpl);

		It should_set_attributes_for_method_generic_part3 = () =>
			TestMethod
				.UnsetAllAttributes()
				.SetAttributes<FamORAssem, UnmanagedExport, RTSpecialName, HasSecurity, RequireSecObject, ReuseSlot, CompilerControlled>() // last two are 0
				.Attributes.Should().Equal(MethodAttributes.FamORAssem | MethodAttributes.UnmanagedExport | MethodAttributes.RTSpecialName | MethodAttributes.HasSecurity | MethodAttributes.RequireSecObject);
		
		// improve code coverage
		It should_set_attributes_for_method_generic_part4 = () =>
			TestMethod
				.UnsetAllAttributes()
				.MethodDefinition
				.SetAttributes<MemberAccessMask, Private, FamANDAssem, HideBySig, NewSlot, Public, Final, Virtual>()
				.Attributes.Should().Equal(MethodAttributes.MemberAccessMask | MethodAttributes.Private | MethodAttributes.FamANDAssem | MethodAttributes.HideBySig | MethodAttributes.NewSlot
					| MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual);

		It should_set_attributes_for_type_generic_part1 = () =>
			TestType
				.UnsetAllAttributes()
				.SetAttributes<VisibilityMask, Public, NestedPublic, NestedFamily, LayoutMask, AutoLayout, SequentialLayout, ExplicitLayout>()
				.Attributes.Should().Equal(TypeAttributes.VisibilityMask | TypeAttributes.Public | TypeAttributes.NestedPublic | TypeAttributes.NestedFamily | TypeAttributes.LayoutMask
					| TypeAttributes.AutoLayout | TypeAttributes.SequentialLayout | TypeAttributes.ExplicitLayout);

		It should_set_attributes_for_type_generic_part2 = () =>
			TestType
				.UnsetAllAttributes()
				.SetAttributes<NestedPrivate, ClassSemanticMask, Interface, Abstract, Sealed, SpecialName, Import, Serializable>()
				.Attributes.Should().Equal(TypeAttributes.NestedPrivate | TypeAttributes.ClassSemanticMask | TypeAttributes.Interface | TypeAttributes.Abstract
					| TypeAttributes.Sealed | TypeAttributes.SpecialName | TypeAttributes.Import | TypeAttributes.Serializable);

		It should_set_attributes_for_type_generic_part3 = () =>
			TestType
				.UnsetAllAttributes()
				.SetAttributes<WindowsRuntime, NestedAssembly, StringFormatMask, AnsiClass, UnicodeClass, AutoClass, BeforeFieldInit, RTSpecialName>()
				.Attributes.Should().Equal(TypeAttributes.WindowsRuntime | TypeAttributes.NestedAssembly | TypeAttributes.StringFormatMask | TypeAttributes.AnsiClass
					| TypeAttributes.UnicodeClass | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit | TypeAttributes.RTSpecialName);

		It should_set_attributes_for_type_generic_part4 = () =>
			TestType
				.UnsetAllAttributes()
				.SetAttributes<NestedFamANDAssem, NestedFamORAssem, HasSecurity, Forwarder, Class, NotPublic>() // last two are 0
				.Attributes.Should().Equal(TypeAttributes.NestedFamANDAssem | TypeAttributes.NestedFamORAssem | TypeAttributes.HasSecurity | TypeAttributes.Forwarder);

		It should_set_attributes_for_event_generic = () =>
			CreateEvent()
				.SetAttributes<SpecialName, RTSpecialName>()
				.Attributes.Should().Equal(EventAttributes.SpecialName | EventAttributes.RTSpecialName);

		It should_set_attributes_for_property_generic = () =>
			CreateProperty()
				.SetAttributes<SpecialName, RTSpecialName, HasDefault> ()
				.Attributes.Should().Equal(PropertyAttributes.SpecialName | PropertyAttributes.RTSpecialName | PropertyAttributes.HasDefault);

		It should_set_attributes_for_field_generic_part1 = () =>
			CreateField()
				.SetAttributes<FieldAccessMask, Private, FamANDAssem, Family, Static, InitOnly, Literal, NotSerialized>()
				.Attributes.Should().Equal(FieldAttributes.FieldAccessMask | FieldAttributes.Private | FieldAttributes.FamANDAssem | FieldAttributes.Family
					| FieldAttributes.Static | FieldAttributes.InitOnly | FieldAttributes.Literal | FieldAttributes.NotSerialized);

		It should_set_attributes_for_field_generic_part2 = () =>
			CreateField()
				.SetAttributes<Assembly, FamORAssem, SpecialName, PInvokeImpl, RTSpecialName, HasFieldMarshal, HasDefault, HasFieldRVA>()
				.Attributes.Should().Equal(FieldAttributes.Assembly | FieldAttributes.FamORAssem | FieldAttributes.SpecialName | FieldAttributes.PInvokeImpl
					| FieldAttributes.RTSpecialName | FieldAttributes.HasFieldMarshal | FieldAttributes.HasDefault | FieldAttributes.HasFieldRVA);

		It should_set_attributes_for_field_generic_part3 = () =>
			CreateField()
				.SetAttributes<Public, CompilerControlled>() // last one is 0
				.Attributes.Should().Equal(FieldAttributes.Public);
	}
}
