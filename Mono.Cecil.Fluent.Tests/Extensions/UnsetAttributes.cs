using Machine.Specifications;
using Mono.Cecil.Fluent.Attributes;
using Mono.Cecil.Fluent.Utils;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Mono.Cecil.Fluent.Tests.Extensions
{
	public class Extensions_UnsetAttributes : TestsBase
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

		It should_unset_attributes_for_method = () =>
			TestMethod
				.SetMethodAttributes(MethodAttributes.Abstract, MethodAttributes.Family, MethodAttributes.Final)
				.UnsetMethodAttributes(MethodAttributes.Abstract, MethodAttributes.Final)
				.Attributes.Should().Equal(MethodAttributes.Family);

		It should_unset_attributes_for_type = () =>
			TestType
				.SetTypeAttributes(TypeAttributes.Abstract, TypeAttributes.Class, TypeAttributes.AutoClass)
				.UnsetTypeAttributes(TypeAttributes.Class, TypeAttributes.AutoClass)
				.Attributes.Should().Equal(TypeAttributes.Abstract);

		It should_unset_attributes_for_event = () =>
			CreateEvent()
				.SetEventAttributes(EventAttributes.SpecialName | EventAttributes.RTSpecialName)
				.UnsetEventAttributes(EventAttributes.SpecialName | EventAttributes.RTSpecialName)
				.Attributes.Should().Equal(EventAttributes.None);

		It should_unset_attributes_for_property = () =>
			CreateProperty()
				.SetPropertyAttributes(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName | PropertyAttributes.HasDefault)
				.UnsetPropertyAttributes(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName)
				.Attributes.Should().Equal(PropertyAttributes.HasDefault);

		It should_unset_attributes_for_field = () =>
			CreateField()
				.SetFieldAttributes(FieldAttributes.Assembly | FieldAttributes.Family | FieldAttributes.HasDefault)
				.UnsetAllFieldAttributes()
				.Attributes.Should().Equal((FieldAttributes) 0);

		It should_unset_all_attributes_for_field = () =>
			CreateField()
				.SetFieldAttributes(FieldAttributes.Assembly | FieldAttributes.Family | FieldAttributes.HasDefault)
				.UnsetFieldAttributes(FieldAttributes.Assembly | FieldAttributes.Family)
				.Attributes.Should().Equal(FieldAttributes.HasDefault);

		It should_unset_all_attributes_for_method = () =>
			TestMethod
				.SetMethodAttributes(MethodAttributes.Abstract, MethodAttributes.Family, MethodAttributes.Final)
				.UnsetAllMethodAttributes()
				.Attributes.Should().Equal((MethodAttributes) 0);

		It should_unset_all_attributes_for_type = () =>
			TestType
				.SetTypeAttributes(TypeAttributes.Abstract, TypeAttributes.Class, TypeAttributes.AutoClass)
				.UnsetAllTypeAttributes()
				.Attributes.Should().Equal((TypeAttributes) 0);

		It should_unset_all_attributes_for_event = () =>
			CreateEvent()
				.SetEventAttributes(EventAttributes.SpecialName | EventAttributes.RTSpecialName)
				.UnsetAllEventAttributes()
				.Attributes.Should().Equal(EventAttributes.None);

		It should_unset_all_attributes_for_property = () =>
			CreateProperty()
				.SetPropertyAttributes(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName | PropertyAttributes.HasDefault)
				.UnsetAllPropertyAttributes()
				.Attributes.Should().Equal(PropertyAttributes.None);
	}
}
