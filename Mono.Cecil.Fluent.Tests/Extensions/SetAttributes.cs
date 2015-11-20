using Machine.Specifications;
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
	}
}
