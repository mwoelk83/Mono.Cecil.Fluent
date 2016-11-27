// Copyright (c) 2011 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;

// ReSharper disable CheckNamespace
namespace ICSharpCode.Decompiler.Disassembler
{
	/// <summary>
	/// Disassembles type and member definitions.
	/// </summary>
	internal sealed class ReflectionDisassembler
	{
		private readonly PlainTextOutput _o;

		private bool _isInType; // whether we are currently disassembling a whole type (-> defaultCollapsed for foldings)

		public ReflectionDisassembler(PlainTextOutput output)
		{
			if (output == null)
				throw new ArgumentNullException(nameof(output));
			_o = output;
		}

		#region Disassemble Method

		private readonly EnumNameCollection<MethodAttributes> _methodAttributeFlags = new EnumNameCollection<MethodAttributes>() {
			{ MethodAttributes.Final, "final" },
			{ MethodAttributes.HideBySig, "hidebysig" },
			{ MethodAttributes.SpecialName, "specialname" },
			{ MethodAttributes.PInvokeImpl, null }, // handled separately
			{ MethodAttributes.UnmanagedExport, "export" },
			{ MethodAttributes.RTSpecialName, "rtspecialname" },
			{ MethodAttributes.RequireSecObject, "reqsecobj" },
			{ MethodAttributes.NewSlot, "newslot" },
			{ MethodAttributes.CheckAccessOnOverride, "strict" },
			{ MethodAttributes.Abstract, "abstract" },
			{ MethodAttributes.Virtual, "virtual" },
			{ MethodAttributes.Static, "static" },
			{ MethodAttributes.HasSecurity, null }, // ?? also invisible in ILDasm
		};

		private readonly EnumNameCollection<MethodAttributes> _methodVisibility = new EnumNameCollection<MethodAttributes>() {
			{ MethodAttributes.Private, "private" },
			{ MethodAttributes.FamANDAssem, "famandassem" },
			{ MethodAttributes.Assembly, "assembly" },
			{ MethodAttributes.Family, "family" },
			{ MethodAttributes.FamORAssem, "famorassem" },
			{ MethodAttributes.Public, "public" },
		};

		private readonly EnumNameCollection<MethodCallingConvention> _callingConvention = new EnumNameCollection<MethodCallingConvention>() {
			{ MethodCallingConvention.C, "unmanaged cdecl" },
			{ MethodCallingConvention.StdCall, "unmanaged stdcall" },
			{ MethodCallingConvention.ThisCall, "unmanaged thiscall" },
			{ MethodCallingConvention.FastCall, "unmanaged fastcall" },
			{ MethodCallingConvention.VarArg, "vararg" },
			{ MethodCallingConvention.Generic, null },
		};

		private readonly EnumNameCollection<MethodImplAttributes> _methodCodeType = new EnumNameCollection<MethodImplAttributes>() {
			{ MethodImplAttributes.IL, "cil" },
			{ MethodImplAttributes.Native, "native" },
			{ MethodImplAttributes.OPTIL, "optil" },
			{ MethodImplAttributes.Runtime, "runtime" },
		};

		private readonly EnumNameCollection<MethodImplAttributes> _methodImpl = new EnumNameCollection<MethodImplAttributes>() {
			{ MethodImplAttributes.Synchronized, "synchronized" },
			{ MethodImplAttributes.NoInlining, "noinlining" },
			{ MethodImplAttributes.NoOptimization, "nooptimization" },
			{ MethodImplAttributes.PreserveSig, "preservesig" },
			{ MethodImplAttributes.InternalCall, "internalcall" },
			{ MethodImplAttributes.ForwardRef, "forwardref" },
		};

		public void DisassembleMethod(MethodDefinition method)
		{
			// write method header
			_o.WriteDefinition(".method ", method);
			DisassembleMethodInternal(method);
		}

		private void DisassembleMethodInternal(MethodDefinition method)
		{
			//    .method public hidebysig  specialname
			//               instance default class [mscorlib]System.IO.TextWriter get_BaseWriter ()  cil managed
			//

			//emit flags
			WriteEnum(method.Attributes & MethodAttributes.MemberAccessMask, _methodVisibility);
			WriteFlags(method.Attributes & ~MethodAttributes.MemberAccessMask, _methodAttributeFlags);
			if (method.IsCompilerControlled) _o.Write("privatescope ");

			if ((method.Attributes & MethodAttributes.PInvokeImpl) == MethodAttributes.PInvokeImpl)
			{
				_o.Write("pinvokeimpl");
				if (method.HasPInvokeInfo && method.PInvokeInfo != null)
				{
					var info = method.PInvokeInfo;
					_o.Write("(\"" + DisassemblerHelpers.ConvertString(info.Module.Name) + "\"");

					if (!string.IsNullOrEmpty(info.EntryPoint) && info.EntryPoint != method.Name)
						_o.Write(" as \"" + DisassemblerHelpers.ConvertString(info.EntryPoint) + "\"");

					if (info.IsNoMangle)
						_o.Write(" nomangle");

					if (info.IsCharSetAnsi)
						_o.Write(" ansi");
					else if (info.IsCharSetAuto)
						_o.Write(" autochar");
					else if (info.IsCharSetUnicode)
						_o.Write(" unicode");

					if (info.SupportsLastError)
						_o.Write(" lasterr");

					if (info.IsCallConvCdecl)
						_o.Write(" cdecl");
					else if (info.IsCallConvFastcall)
						_o.Write(" fastcall");
					else if (info.IsCallConvStdCall)
						_o.Write(" stdcall");
					else if (info.IsCallConvThiscall)
						_o.Write(" thiscall");
					else if (info.IsCallConvWinapi)
						_o.Write(" winapi");

					_o.Write(')');
				}
				_o.Write(' ');
			}

			_o.WriteLine();
			_o.Indent();
			if (method.ExplicitThis)
				_o.Write("instance explicit ");
			else if (method.HasThis)
				_o.Write("instance ");

			//call convention
		    // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
			WriteEnum(method.CallingConvention & (MethodCallingConvention)0x1f, _callingConvention);

			//return type
			method.ReturnType.WriteTo(_o);
			_o.Write(' ');
			if (method.MethodReturnType.HasMarshalInfo)
			{
				WriteMarshalInfo(method.MethodReturnType.MarshalInfo);
			}

		    _o.Write(method.IsCompilerControlled
		        ? DisassemblerHelpers.Escape(method.Name + "$PST" + method.MetadataToken.ToInt32().ToString("X8"))
		        : DisassemblerHelpers.Escape(method.Name));

		    WriteTypeParameters(_o, method);

			//( params )
			_o.Write(" (");
			if (method.HasParameters)
			{
				_o.WriteLine();
				_o.Indent();
				WriteParameters(method.Parameters);
				_o.Unindent();
			}
			_o.Write(") ");
			//cil managed
			WriteEnum(method.ImplAttributes & MethodImplAttributes.CodeTypeMask, _methodCodeType);
		    _o.Write((method.ImplAttributes & MethodImplAttributes.ManagedMask) == MethodImplAttributes.Managed ? "managed " : "unmanaged ");
		    WriteFlags(method.ImplAttributes & ~(MethodImplAttributes.CodeTypeMask | MethodImplAttributes.ManagedMask), _methodImpl);

			_o.Unindent();
			OpenBlock();
			WriteAttributes(method.CustomAttributes);
			if (method.HasOverrides)
			{
				foreach (var methodOverride in method.Overrides)
				{
					_o.Write(".override method ");
					methodOverride.WriteTo(_o);
					_o.WriteLine();
				}
			}
			WriteParameterAttributes(0, method.MethodReturnType, method.MethodReturnType);
			foreach (var p in method.Parameters)
			{
				WriteParameterAttributes(p.Index + 1, p, p);
			}
			WriteSecurityDeclarations(method);

			if (method.HasBody)
				new MethodBodyDisassembler(_o)
					.Disassemble(method.Body);

			CloseBlock("end of method " + DisassemblerHelpers.Escape(method.DeclaringType.Name) + "::" + DisassemblerHelpers.Escape(method.Name));
		}

		#region Write Security Declarations

		private void WriteSecurityDeclarations(ISecurityDeclarationProvider secDeclProvider)
		{
			if (!secDeclProvider.HasSecurityDeclarations)
				return;
			foreach (var secdecl in secDeclProvider.SecurityDeclarations)
			{
				_o.Write(".permissionset ");
				switch (secdecl.Action)
				{
					//ncrunch: no coverage start
					case SecurityAction.Request:
						_o.Write("request");
						break;
					case SecurityAction.Demand:
						_o.Write("demand");
						break;
					case SecurityAction.Assert:
						_o.Write("assert");
						break;
					case SecurityAction.Deny:
						_o.Write("deny");
						break;
					case SecurityAction.PermitOnly:
						_o.Write("permitonly");
						break;
					case SecurityAction.LinkDemand:
						_o.Write("linkcheck");
						break;
					case SecurityAction.InheritDemand:
						_o.Write("inheritcheck");
						break;
					case SecurityAction.RequestMinimum:
						_o.Write("reqmin");
						break;
					case SecurityAction.RequestOptional:
						_o.Write("reqopt");
						break;
					case SecurityAction.RequestRefuse:
						_o.Write("reqrefuse");
						break;
					case SecurityAction.PreJitGrant:
						_o.Write("prejitgrant");
						break;
					case SecurityAction.PreJitDeny:
						_o.Write("prejitdeny");
						break;
					case SecurityAction.NonCasDemand:
						_o.Write("noncasdemand");
						break;
					case SecurityAction.NonCasLinkDemand:
						_o.Write("noncaslinkdemand");
						break;
					case SecurityAction.NonCasInheritance:
						_o.Write("noncasinheritance");
						break;
					default:
						_o.Write(secdecl.Action.ToString());
						break;
					//ncrunch: no coverage end
				}
				_o.WriteLine(" = {");
				_o.Indent();
				for (var i = 0; i < secdecl.SecurityAttributes.Count; i++)
				{
					var sa = secdecl.SecurityAttributes[i];
					if (sa.AttributeType.Scope == sa.AttributeType.Module)
					{
						_o.Write("class ");
						_o.Write(DisassemblerHelpers.Escape(GetAssemblyQualifiedName(sa.AttributeType)));
					}
					else
						sa.AttributeType.WriteTo(_o, IlNameSyntax.TypeName);

					_o.Write(" = {");
					if (sa.HasFields || sa.HasProperties)
					{
						_o.WriteLine();
						_o.Indent();

						foreach (var na in sa.Fields)
						{
							_o.Write("field ");
							WriteSecurityDeclarationArgument(na);
							_o.WriteLine();
						}

						foreach (var na in sa.Properties)
						{
							_o.Write("property ");
							WriteSecurityDeclarationArgument(na);
							_o.WriteLine();
						}

						_o.Unindent();
					}
					_o.Write('}');

					if (i + 1 < secdecl.SecurityAttributes.Count)
						_o.Write(',');
					_o.WriteLine();
				}
				_o.Unindent();
				_o.WriteLine("}");
			}
		}

		private void WriteSecurityDeclarationArgument(CustomAttributeNamedArgument na)
		{
			var type = na.Argument.Type;
			if (type.MetadataType == MetadataType.Class || type.MetadataType == MetadataType.ValueType)
			{
				_o.Write("enum ");
				if (type.Scope != type.Module)
				{
					_o.Write("class ");
					_o.Write(DisassemblerHelpers.Escape(GetAssemblyQualifiedName(type)));
				}
				else
					type.WriteTo(_o, IlNameSyntax.TypeName);
			}
			else
				type.WriteTo(_o);

			_o.Write(' ');
			_o.Write(DisassemblerHelpers.Escape(na.Name));
			_o.Write(" = ");

		    var s = na.Argument.Value as string;
		    if (s != null)
				// secdecls use special syntax for strings
				_o.Write("string('{0}')", DisassemblerHelpers.ConvertString(s).Replace("'", "\'"));
			else
				WriteConstant(na.Argument.Value);
		}

		private string GetAssemblyQualifiedName(TypeReference type)
		{
			var anr = type.Scope as AssemblyNameReference;
			if (anr == null)
			{
				var md = type.Scope as ModuleDefinition;
				if (md != null)
					anr = md.Assembly.Name;
			}
			if (anr != null)
				return type.FullName + ", " + anr.FullName;
			return type.FullName;
		}
		#endregion

		#region WriteMarshalInfo

		private void WriteMarshalInfo(MarshalInfo marshalInfo)
		{
			_o.Write("marshal(");
			WriteNativeType(marshalInfo.NativeType, marshalInfo);
			_o.Write(") ");
		}

		private void WriteNativeType(NativeType nativeType, MarshalInfo marshalInfo = null)
		{
			//ncrunch: no coverage start
		    // ReSharper disable once SwitchStatementMissingSomeCases
			switch (nativeType)
			{
				case NativeType.None: break;
				case NativeType.Boolean: _o.Write("bool"); break;
				case NativeType.I1: _o.Write("int8"); break;
				case NativeType.U1: _o.Write("unsigned int8"); break;
				case NativeType.I2: _o.Write("int16"); break; 
				case NativeType.U2: _o.Write("unsigned int16"); break;
				case NativeType.I4: _o.Write("int32"); break;
				case NativeType.U4: _o.Write("unsigned int32"); break;
				case NativeType.I8: _o.Write("int64"); break;
				case NativeType.U8: _o.Write("unsigned int64"); break;
				case NativeType.R4: _o.Write("float32"); break;
				case NativeType.R8: _o.Write("float64"); break;
				case NativeType.LPStr: _o.Write("lpstr"); break;
				case NativeType.Int: _o.Write("int"); break;
				case NativeType.UInt: _o.Write("unsigned int"); break;
				case NativeType.Func: goto default; // ??
				case NativeType.Array: var ami = (ArrayMarshalInfo)marshalInfo;
					if (ami == null)
                        goto default;
					if (ami.ElementType != NativeType.Max)
                        WriteNativeType(ami.ElementType);
					_o.Write('[');
					if (ami.SizeParameterMultiplier == 0) _o.Write(ami.Size.ToString());
					else
					{
						if (ami.Size >= 0)
                            _o.Write(ami.Size.ToString());
						_o.Write(" + ");
						_o.Write(ami.SizeParameterIndex.ToString());
					}
					_o.Write(']'); break;
				case NativeType.Currency: _o.Write("currency"); break;
				case NativeType.BStr: _o.Write("bstr"); break;
				case NativeType.LPWStr: _o.Write("lpwstr"); break;
				case NativeType.LPTStr: _o.Write("lptstr"); break;
			    // ReSharper disable once PossibleNullReferenceException
				case NativeType.FixedSysString: _o.Write("fixed sysstring[{0}]", ((FixedSysStringMarshalInfo)marshalInfo).Size); break;
				case NativeType.IUnknown: _o.Write("iunknown"); break;
				case NativeType.IDispatch: _o.Write("idispatch"); break;
				case NativeType.Struct: _o.Write("struct"); break;
				case NativeType.IntF: _o.Write("interface"); break;
				case NativeType.SafeArray: _o.Write("safearray "); var sami = marshalInfo as SafeArrayMarshalInfo;
					if (sami != null)
					{
						switch (sami.ElementType)
						{
							case VariantType.None: break;
							case VariantType.I2: _o.Write("int16"); break;
							case VariantType.I4: _o.Write("int32"); break;
							case VariantType.R4: _o.Write("float32"); break;
							case VariantType.R8: _o.Write("float64"); break;
							case VariantType.CY: _o.Write("currency"); break;
							case VariantType.Date: _o.Write("date"); break;
							case VariantType.BStr: _o.Write("bstr"); break;
							case VariantType.Dispatch: _o.Write("idispatch"); break;
							case VariantType.Error: _o.Write("error"); break;
							case VariantType.Bool: _o.Write("bool"); break;
							case VariantType.Variant: _o.Write("variant"); break;
							case VariantType.Unknown: _o.Write("iunknown"); break;
							case VariantType.Decimal: _o.Write("decimal"); break;
							case VariantType.I1: _o.Write("int8"); break;
							case VariantType.UI1: _o.Write("unsigned int8"); break;
							case VariantType.UI2: _o.Write("unsigned int16"); break;
							case VariantType.UI4: _o.Write("unsigned int32"); break;
							case VariantType.Int: _o.Write("int"); break;
							case VariantType.UInt: _o.Write("unsigned int"); break;
							default: _o.Write(sami.ElementType.ToString()); break;
						}
					} break;
				case NativeType.FixedArray: _o.Write("fixed array");
					var fami = marshalInfo as FixedArrayMarshalInfo;
					if (fami != null)
					{
						_o.Write("[{0}]", fami.Size);
						if (fami.ElementType != NativeType.None)
							_o.Write(' '); WriteNativeType(fami.ElementType);
					} break;
				case NativeType.ByValStr: _o.Write("byvalstr"); break;
				case NativeType.ANSIBStr: _o.Write("ansi bstr"); break;
				case NativeType.TBStr: _o.Write("tbstr"); break;
				case NativeType.VariantBool: _o.Write("variant bool"); break;
				case NativeType.ASAny: _o.Write("as any"); break;
				case NativeType.LPStruct: _o.Write("lpstruct"); break;
				case NativeType.CustomMarshaler: var cmi = marshalInfo as CustomMarshalInfo; if (cmi == null) goto default;
					_o.Write("custom(\"{0}\", \"{1}\"", DisassemblerHelpers.ConvertString(cmi.ManagedType.FullName), DisassemblerHelpers.ConvertString(cmi.Cookie));
					if (cmi.Guid != Guid.Empty || !string.IsNullOrEmpty(cmi.UnmanagedType))
						_o.Write(", \"{0}\", \"{1}\"", cmi.Guid.ToString(), DisassemblerHelpers.ConvertString(cmi.UnmanagedType));
					_o.Write(')'); break;
				case NativeType.Error: _o.Write("error"); break;
				default: _o.Write(nativeType.ToString()); break;
			}
			//ncrunch: no coverage end
		}
		#endregion

		private void WriteParameters(Collection<ParameterDefinition> parameters)
		{
			for (var i = 0; i < parameters.Count; i++)
			{
				var p = parameters[i];
				if (p.IsIn)
					_o.Write("[in] ");
				if (p.IsOut)
					_o.Write("[out] ");
				if (p.IsOptional)
					_o.Write("[opt] ");
				p.ParameterType.WriteTo(_o);
				_o.Write(' ');
				if (p.HasMarshalInfo)
					WriteMarshalInfo(p.MarshalInfo);

				_o.WriteDefinition(DisassemblerHelpers.Escape(p.Name), p);
				if (i < parameters.Count - 1)
					_o.Write(',');
				_o.WriteLine();
			}
		}

		private void WriteParameterAttributes(int index, IConstantProvider cp, ICustomAttributeProvider cap)
		{
			if (!cp.HasConstant && !cap.HasCustomAttributes)
				return;
			_o.Write(".param [{0}]", index);
			if (cp.HasConstant)
			{
				_o.Write(" = ");
				WriteConstant(cp.Constant);
			}
			_o.WriteLine();
			WriteAttributes(cap.CustomAttributes);
		}

		private void WriteConstant(object constant)
		{
			if (constant == null)
				_o.Write("nullref");
			else
			{
				var typeName = DisassemblerHelpers.PrimitiveTypeName(constant.GetType().FullName);
				if (typeName != null && typeName != "string")
				{
					_o.Write(typeName);
					_o.Write('(');
					var cf = constant as float?;
					var cd = constant as double?;
					if (cf.HasValue && (float.IsNaN(cf.Value) || float.IsInfinity(cf.Value)))
						_o.Write("0x{0:x8}", BitConverter.ToInt32(BitConverter.GetBytes(cf.Value), 0));
					else if (cd.HasValue && (double.IsNaN(cd.Value) || double.IsInfinity(cd.Value)))
						_o.Write("0x{0:x16}", BitConverter.DoubleToInt64Bits(cd.Value));
					else
						DisassemblerHelpers.WriteOperand(_o, constant);
					_o.Write(')');
				}
				else
					DisassemblerHelpers.WriteOperand(_o, constant);
			}
		}
		#endregion

		#region Disassemble Field

		private readonly EnumNameCollection<FieldAttributes> _fieldVisibility = new EnumNameCollection<FieldAttributes>() {
			{ FieldAttributes.Private, "private" },
			{ FieldAttributes.FamANDAssem, "famandassem" },
			{ FieldAttributes.Assembly, "assembly" },
			{ FieldAttributes.Family, "family" },
			{ FieldAttributes.FamORAssem, "famorassem" },
			{ FieldAttributes.Public, "public" },
		};

		private readonly EnumNameCollection<FieldAttributes> _fieldAttributes = new EnumNameCollection<FieldAttributes>() {
			{ FieldAttributes.Static, "static" },
			{ FieldAttributes.Literal, "literal" },
			{ FieldAttributes.InitOnly, "initonly" },
			{ FieldAttributes.SpecialName, "specialname" },
			{ FieldAttributes.RTSpecialName, "rtspecialname" },
			{ FieldAttributes.NotSerialized, "notserialized" },
		};

		public void DisassembleField(FieldDefinition field)
		{
			_o.WriteDefinition(".field ", field);
			if (field.HasLayoutInfo)
				_o.Write("[" + field.Offset + "] ");
			WriteEnum(field.Attributes & FieldAttributes.FieldAccessMask, _fieldVisibility);
			const FieldAttributes hasXAttributes = FieldAttributes.HasDefault | FieldAttributes.HasFieldMarshal | FieldAttributes.HasFieldRVA;
			WriteFlags(field.Attributes & ~(FieldAttributes.FieldAccessMask | hasXAttributes), _fieldAttributes);
			if (field.HasMarshalInfo)
				WriteMarshalInfo(field.MarshalInfo);
			field.FieldType.WriteTo(_o);
			_o.Write(' ');
			_o.Write(DisassemblerHelpers.Escape(field.Name));
			if ((field.Attributes & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA)
				_o.Write(" at I_{0:x8}", field.RVA);
			if (field.HasConstant)
			{
				_o.Write(" = ");
				WriteConstant(field.Constant);
			}
			_o.WriteLine();
			if (field.HasCustomAttributes)
				WriteAttributes(field.CustomAttributes);
		}
		#endregion

		#region Disassemble Property

		private readonly EnumNameCollection<PropertyAttributes> _propertyAttributes = new EnumNameCollection<PropertyAttributes>() {
			{ PropertyAttributes.SpecialName, "specialname" },
			{ PropertyAttributes.RTSpecialName, "rtspecialname" },
			{ PropertyAttributes.HasDefault, "hasdefault" },
		};

		public void DisassembleProperty(PropertyDefinition property)
		{
			_o.WriteDefinition(".property ", property);
			WriteFlags(property.Attributes, _propertyAttributes);
			if (property.HasThis)
				_o.Write("instance ");
			property.PropertyType.WriteTo(_o);
			_o.Write(' ');
			_o.Write(DisassemblerHelpers.Escape(property.Name));

			_o.Write("(");
			if (property.HasParameters)
			{
				_o.WriteLine();
				_o.Indent();
				WriteParameters(property.Parameters);
				_o.Unindent();
			}
			_o.Write(")");

			OpenBlock();
			WriteAttributes(property.CustomAttributes);
			WriteNestedMethod(".get", property.GetMethod);
			WriteNestedMethod(".set", property.SetMethod);

			foreach (var method in property.OtherMethods)
				WriteNestedMethod(".other", method);

			CloseBlock();
		}

		private void WriteNestedMethod(string keyword, MethodDefinition method)
		{
			if (method == null)
				return;

			_o.Write(keyword);
			_o.Write(' ');
			method.WriteTo(_o);
			_o.WriteLine();
		}
		#endregion

		#region Disassemble Event

		private readonly EnumNameCollection<EventAttributes> _eventAttributes = new EnumNameCollection<EventAttributes>() {
			{ EventAttributes.SpecialName, "specialname" },
			{ EventAttributes.RTSpecialName, "rtspecialname" },
		};

		public void DisassembleEvent(EventDefinition ev)
		{
			_o.WriteDefinition(".event ", ev);
			WriteFlags(ev.Attributes, _eventAttributes);
			ev.EventType.WriteTo(_o, IlNameSyntax.TypeName);
			_o.Write(' ');
			_o.Write(DisassemblerHelpers.Escape(ev.Name));
			OpenBlock();
			WriteAttributes(ev.CustomAttributes);
			WriteNestedMethod(".addon", ev.AddMethod);
			WriteNestedMethod(".removeon", ev.RemoveMethod);
			WriteNestedMethod(".fire", ev.InvokeMethod);
			foreach (var method in ev.OtherMethods)
				WriteNestedMethod(".other", method);

			CloseBlock();
		}
		#endregion

		#region Disassemble Type

		private readonly EnumNameCollection<TypeAttributes> _typeVisibility = new EnumNameCollection<TypeAttributes>() {
			{ TypeAttributes.Public, "public" },
			{ TypeAttributes.NotPublic, "private" },
			{ TypeAttributes.NestedPublic, "nested public" },
			{ TypeAttributes.NestedPrivate, "nested private" },
			{ TypeAttributes.NestedAssembly, "nested assembly" },
			{ TypeAttributes.NestedFamily, "nested family" },
			{ TypeAttributes.NestedFamANDAssem, "nested famandassem" },
			{ TypeAttributes.NestedFamORAssem, "nested famorassem" },
		};

		private readonly EnumNameCollection<TypeAttributes> _typeLayout = new EnumNameCollection<TypeAttributes>() {
			{ TypeAttributes.AutoLayout, "auto" },
			{ TypeAttributes.SequentialLayout, "sequential" },
			{ TypeAttributes.ExplicitLayout, "explicit" },
		};

		private readonly EnumNameCollection<TypeAttributes> _typeStringFormat = new EnumNameCollection<TypeAttributes>() {
			{ TypeAttributes.AutoClass, "auto" },
			{ TypeAttributes.AnsiClass, "ansi" },
			{ TypeAttributes.UnicodeClass, "unicode" },
		};

		private readonly EnumNameCollection<TypeAttributes> _typeAttributes = new EnumNameCollection<TypeAttributes>() {
			{ TypeAttributes.Abstract, "abstract" },
			{ TypeAttributes.Sealed, "sealed" },
			{ TypeAttributes.SpecialName, "specialname" },
			{ TypeAttributes.Import, "import" },
			{ TypeAttributes.Serializable, "serializable" },
			{ TypeAttributes.WindowsRuntime, "windowsruntime" },
			{ TypeAttributes.BeforeFieldInit, "beforefieldinit" },
			{ TypeAttributes.HasSecurity, null },
		};

		public void DisassembleType(TypeDefinition type)
		{
			// start writing IL
			_o.WriteDefinition(".class ", type);

			if ((type.Attributes & TypeAttributes.ClassSemanticMask) == TypeAttributes.Interface)
				_o.Write("interface ");
			WriteEnum(type.Attributes & TypeAttributes.VisibilityMask, _typeVisibility);
			WriteEnum(type.Attributes & TypeAttributes.LayoutMask, _typeLayout);
			WriteEnum(type.Attributes & TypeAttributes.StringFormatMask, _typeStringFormat);
			const TypeAttributes masks = TypeAttributes.ClassSemanticMask | TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask | TypeAttributes.StringFormatMask;
			WriteFlags(type.Attributes & ~masks, _typeAttributes);

			_o.Write(DisassemblerHelpers.Escape(type.DeclaringType != null ? type.Name : type.FullName));
			WriteTypeParameters(_o, type);
			_o.WriteLine();

			if (type.BaseType != null)
			{
				_o.Indent();
				_o.Write("extends ");
				type.BaseType.WriteTo(_o, IlNameSyntax.TypeName);
				_o.WriteLine();
				_o.Unindent();
			}
			if (type.HasInterfaces)
			{
				_o.Indent();
				for (var index = 0; index < type.Interfaces.Count; index++)
				{
					if (index > 0)
						_o.WriteLine(",");
					_o.Write(index == 0 ? "implements " : "           ");
					type.Interfaces[index].WriteTo(_o, IlNameSyntax.TypeName);
				}
				_o.WriteLine();
				_o.Unindent();
			}

			_o.WriteLine("{");
			_o.Indent();
			var oldIsInType = _isInType;
			_isInType = true;
			WriteAttributes(type.CustomAttributes);
			WriteSecurityDeclarations(type);
			if (type.HasLayoutInfo)
			{
				_o.WriteLine(".pack {0}", type.PackingSize);
				_o.WriteLine(".size {0}", type.ClassSize);
				_o.WriteLine();
			}
			if (type.HasNestedTypes)
			{
				_o.WriteLine("// Nested Types");
				foreach (var nestedType in type.NestedTypes)
				{
					DisassembleType(nestedType);
					_o.WriteLine();
				}
				_o.WriteLine();
			}
			if (type.HasFields)
			{
				_o.WriteLine("// Fields");
				foreach (var field in type.Fields)
				{
					DisassembleField(field);
				}
				_o.WriteLine();
			}
			if (type.HasMethods)
			{
				_o.WriteLine("// Methods");
				foreach (var m in type.Methods)
				{
					// write method header
					_o.WriteDefinition(".method ", m);
					DisassembleMethodInternal(m);
					_o.WriteLine();
				} 
			}
			if (type.HasEvents)
			{
				_o.WriteLine("// Events");
				foreach (var ev in type.Events)
				{
					DisassembleEvent(ev);
					_o.WriteLine();
				}
				_o.WriteLine();
			}
			if (type.HasProperties)
			{
				_o.WriteLine("// Properties");
				foreach (var prop in type.Properties)
				{
					DisassembleProperty(prop);
				}
				_o.WriteLine();
			}
			CloseBlock("end of class " + (type.DeclaringType != null ? type.Name : type.FullName));
			_isInType = oldIsInType;
		}

		private static void WriteTypeParameters(PlainTextOutput output, IGenericParameterProvider p)
		{
			if (!p.HasGenericParameters)
				return;

			output.Write('<');
			for (var i = 0; i < p.GenericParameters.Count; i++)
			{
				if (i > 0)
					output.Write(", ");
				var gp = p.GenericParameters[i];
				if (gp.HasReferenceTypeConstraint)
				{
					output.Write("class ");
				}
				else if (gp.HasNotNullableValueTypeConstraint)
				{
					output.Write("valuetype ");
				}
				if (gp.HasDefaultConstructorConstraint)
				{
					output.Write(".ctor ");
				}
				if (gp.HasConstraints)
				{
					output.Write('(');
					for (var j = 0; j < gp.Constraints.Count; j++)
					{
						if (j > 0)
							output.Write(", ");
						gp.Constraints[j].WriteTo(output, IlNameSyntax.TypeName);
					}
					output.Write(") ");
				}
				if (gp.IsContravariant)
				{
					output.Write('-');
				}
				else if (gp.IsCovariant)
				{
					output.Write('+');
				}
				output.Write(DisassemblerHelpers.Escape(gp.Name));
			}
			output.Write('>');
		}
		#endregion

		#region Helper methods

		private void WriteAttributes(Collection<CustomAttribute> attributes)
		{
			foreach (var a in attributes)
			{
				_o.Write(".custom ");
				a.Constructor.WriteTo(_o);
				var blob = a.GetBlob();
				if (blob != null)
				{
					_o.Write(" = ");
					WriteBlob(blob);
				}
				_o.WriteLine();
			}
		}

		private void WriteBlob(byte[] blob)
		{
			_o.Write("(");
			_o.Indent();

			for (var i = 0; i < blob.Length; i++)
			{
				if (i % 16 == 0 && i < blob.Length - 1)
					_o.WriteLine();
				else
					_o.Write(' ');

				_o.Write(blob[i].ToString("x2"));
			}

			_o.WriteLine();
			_o.Unindent();
			_o.Write(")");
		}

		private void OpenBlock()
		{
			_o.WriteLine();
			_o.WriteLine("{");
			_o.Indent();
		}

		private void CloseBlock(string comment = null)
		{
			_o.Unindent();
			_o.Write("}");
			if (comment != null)
				_o.Write(" // " + comment);
			_o.WriteLine();
		}

		private void WriteFlags<T>(T flags, EnumNameCollection<T> flagNames) where T : struct
		{
			var val = Convert.ToInt64(flags);
			long tested = 0;
			foreach (var pair in flagNames)
			{
				tested |= pair.Key;
				if ((val & pair.Key) == 0 || pair.Value == null)
					continue;
				_o.Write(pair.Value);
				_o.Write(' ');
			}
			if ((val & ~tested) != 0)
				_o.Write("flag({0:x4}) ", val & ~tested);
		}

		private void WriteEnum<T>(T enumValue, EnumNameCollection<T> enumNames) where T : struct
		{
			var val = Convert.ToInt64(enumValue);
			foreach (var pair in enumNames.Where(pair => pair.Key == val))
			{
				if (pair.Value == null)
					return;
				_o.Write(pair.Value);
				_o.Write(' ');
				return;
			}
			if (val == 0)
				return;
			_o.Write("flag({0:x4})", val);
			_o.Write(' ');
		}

		private sealed class EnumNameCollection<T> : IEnumerable<KeyValuePair<long, string>> where T : struct
		{
			private readonly List<KeyValuePair<long, string>> _names = new List<KeyValuePair<long, string>>();

			public void Add(T flag, string name)
			{
				_names.Add(new KeyValuePair<long, string>(Convert.ToInt64(flag), name));
			}

			public IEnumerator<KeyValuePair<long, string>> GetEnumerator()
			{
				return _names.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return _names.GetEnumerator();
			}
		}
		#endregion 
	}
}
