using Machine.Specifications;
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
				.SetAttributes(MethodAttributes.Abstract, MethodAttributes.Family, MethodAttributes.Final)
				.MethodDefinition.UnsetAttributes(MethodAttributes.Abstract, MethodAttributes.Final)
				.Attributes.Should().Equal(MethodAttributes.Family);

		It should_unset_attributes_for_type = () =>
			TestType
				.SetAttributes(TypeAttributes.Abstract, TypeAttributes.Class, TypeAttributes.AutoClass)
				.UnsetAttributes(TypeAttributes.Class, TypeAttributes.AutoClass)
				.Attributes.Should().Equal(TypeAttributes.Abstract);

		It should_unset_attributes_for_event = () =>
			CreateEvent()
				.SetAttributes(EventAttributes.SpecialName | EventAttributes.RTSpecialName)
				.UnsetAttributes(EventAttributes.SpecialName | EventAttributes.RTSpecialName)
				.Attributes.Should().Equal(EventAttributes.None);

		It should_unset_attributes_for_property = () =>
			CreateProperty()
				.SetAttributes(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName | PropertyAttributes.HasDefault)
				.UnsetAttributes(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName)
				.Attributes.Should().Equal(PropertyAttributes.HasDefault);

		It should_unset_attributes_for_field = () =>
			CreateField()
				.SetAttributes(FieldAttributes.Assembly | FieldAttributes.Family | FieldAttributes.HasDefault)
				.UnsetAllAttributes()
				.Attributes.Should().Equal((FieldAttributes) 0);

		It should_unset_all_attributes_for_field = () =>
			CreateField()
				.SetAttributes(FieldAttributes.Assembly | FieldAttributes.Family | FieldAttributes.HasDefault)
				.UnsetAttributes(FieldAttributes.Assembly | FieldAttributes.Family)
				.Attributes.Should().Equal(FieldAttributes.HasDefault);

		It should_unset_all_attributes_for_method = () =>
			TestMethod
				.SetAttributes(MethodAttributes.Abstract, MethodAttributes.Family, MethodAttributes.Final)
				.MethodDefinition.UnsetAllAttributes()
				.Attributes.Should().Equal((MethodAttributes) 0);

		It should_unset_all_attributes_for_type = () =>
			TestType
				.SetAttributes(TypeAttributes.Abstract, TypeAttributes.Class, TypeAttributes.AutoClass)
				.UnsetAllAttributes()
				.Attributes.Should().Equal((TypeAttributes) 0);

		It should_unset_all_attributes_for_event = () =>
			CreateEvent()
				.SetAttributes(EventAttributes.SpecialName | EventAttributes.RTSpecialName)
				.UnsetAllAttributes()
				.Attributes.Should().Equal(EventAttributes.None);

		It should_unset_all_attributes_for_property = () =>
			CreateProperty()
				.SetAttributes(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName | PropertyAttributes.HasDefault)
				.UnsetAllAttributes()
				.Attributes.Should().Equal(PropertyAttributes.None);
	}
}
