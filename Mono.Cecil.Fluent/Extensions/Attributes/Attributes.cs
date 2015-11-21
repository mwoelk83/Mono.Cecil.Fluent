
// see extensions SetAttribute(s)<TAttr[,TAttr1[TAttr2[,Tattr3[,..]]]]>

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

namespace Mono.Cecil.Fluent.Attributes
{
	public struct FieldAccessMask : IFieldAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.FieldAccessMask;
	}
	public struct Forwarder : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.Forwarder;
	}

	public struct BeforeFieldInit : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.BeforeFieldInit;
	}

	public struct Import : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.Import;
	}

	public struct Serializable : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.Serializable;
	}

	public struct WindowsRuntime : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.WindowsRuntime;
	}

	public struct StringFormatMask : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.StringFormatMask;
	}

	public struct AnsiClass : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.AnsiClass;
	}

	public struct UnicodeClass : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.UnicodeClass;
	}

	public struct AutoClass : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.AutoClass;
	}

	public struct Sealed : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.Sealed;
	}

	public struct ClassSemanticMask : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.ClassSemanticMask;
	}

	public struct Class : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.Class;
	}

	public struct Interface : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.Interface;
	}

	public struct LayoutMask : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.LayoutMask;
	}

	public struct AutoLayout : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.AutoLayout;
	}

	public struct SequentialLayout : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.SequentialLayout;
	}

	public struct ExplicitLayout : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.ExplicitLayout;
	}

	public struct NestedPublic : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.NestedPublic;
	}

	public struct NestedPrivate : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.NestedPrivate;
	}

	public struct NestedFamily : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.NestedFamily;
	}

	public struct NestedAssembly : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.NestedAssembly;
	}

	public struct NestedFamANDAssem : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.NestedFamANDAssem;
	}

	public struct NestedFamORAssem : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.NestedFamORAssem;
	}

	public struct NotPublic : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.NotPublic;
	}

	public struct VisibilityMask : ITypeAttribute
	{
		public TypeAttributes TypeAttributesValue => TypeAttributes.VisibilityMask;
	}

	public struct MemberAccessMask : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.MemberAccessMask;
	}

	public struct CompilerControlled : IFieldAttribute, IMethodAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.CompilerControlled;
		public MethodAttributes MethodAttributesValue => MethodAttributes.CompilerControlled;
	}

	public struct ReuseSlot : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.ReuseSlot;
	}

	public struct Private : IFieldAttribute, IMethodAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.Private;
		public MethodAttributes MethodAttributesValue => MethodAttributes.Private;
	}

	public struct FamANDAssem : IFieldAttribute, IMethodAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.FamANDAssem;
		public MethodAttributes MethodAttributesValue => MethodAttributes.FamANDAssem;
	}

	public struct Assembly : IFieldAttribute, IMethodAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.Assembly;
		public MethodAttributes MethodAttributesValue => MethodAttributes.Assembly;
	}

	public struct Family : IFieldAttribute, IMethodAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.Family;
		public MethodAttributes MethodAttributesValue => MethodAttributes.Family;
	}

	public struct FamORAssem : IFieldAttribute, IMethodAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.FamORAssem;
		public MethodAttributes MethodAttributesValue => MethodAttributes.FamORAssem;
	}

	public struct Public : IFieldAttribute, IMethodAttribute, ITypeAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.Public;
		public MethodAttributes MethodAttributesValue => MethodAttributes.Public;
		public TypeAttributes TypeAttributesValue => TypeAttributes.Public;
	}

	public struct UnmanagedExport : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.UnmanagedExport;
	}


	public struct Static : IFieldAttribute, IMethodAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.Static;
		public MethodAttributes MethodAttributesValue => MethodAttributes.Static;
	}

	public struct InitOnly : IFieldAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.InitOnly;
	}

	public struct Final : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.Final;
	}

	public struct Literal : IFieldAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.Literal;
	}

	public struct Virtual : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.Virtual;
	}

	public struct NotSerialized : IFieldAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.NotSerialized;
	}

	public struct HideBySig : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.HideBySig;
	}

	public struct VtableLayoutMask : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.VtableLayoutMask;
	}

	public struct NewSlot : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.NewSlot;
	}

	public struct CheckAccessOnOverride : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.CheckAccessOnOverride;
	}

	public struct SpecialName : IFieldAttribute, IEventAttribute, IMethodAttribute, IPropertyAttribute, ITypeAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.SpecialName;
		public EventAttributes EventAttributesValue => EventAttributes.SpecialName;
		public MethodAttributes MethodAttributesValue => MethodAttributes.SpecialName;
		public PropertyAttributes PropertydAttributesValue => PropertyAttributes.SpecialName;
		public TypeAttributes TypeAttributesValue => TypeAttributes.SpecialName;
	}

	public struct Abstract : IMethodAttribute, ITypeAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.Abstract;
		public TypeAttributes TypeAttributesValue => TypeAttributes.Abstract;
	}

	public struct PInvokeImpl : IFieldAttribute, IMethodAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.PInvokeImpl;
		public MethodAttributes MethodAttributesValue => MethodAttributes.PInvokeImpl;
	}

	public struct RTSpecialName : IFieldAttribute, IEventAttribute, IMethodAttribute, IPropertyAttribute, ITypeAttribute
	{
		public EventAttributes EventAttributesValue => EventAttributes.RTSpecialName;
		public FieldAttributes FieldAttributesValue => FieldAttributes.RTSpecialName;
		public MethodAttributes MethodAttributesValue => MethodAttributes.RTSpecialName;
		public PropertyAttributes PropertydAttributesValue => PropertyAttributes.RTSpecialName;
		public TypeAttributes TypeAttributesValue => TypeAttributes.RTSpecialName;
	}

	public struct HasFieldMarshal : IFieldAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.HasFieldMarshal;
	}

	public struct HasDefault : IFieldAttribute, IPropertyAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.HasDefault;
		public PropertyAttributes PropertydAttributesValue => PropertyAttributes.HasDefault;
	}

	public struct HasSecurity : IMethodAttribute, ITypeAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.HasSecurity;
		public TypeAttributes TypeAttributesValue => TypeAttributes.HasSecurity;
	}

	public struct RequireSecObject : IMethodAttribute
	{
		public MethodAttributes MethodAttributesValue => MethodAttributes.RequireSecObject;
	}

	public struct HasFieldRVA : IFieldAttribute
	{
		public FieldAttributes FieldAttributesValue => FieldAttributes.HasFieldRVA;
	}
}