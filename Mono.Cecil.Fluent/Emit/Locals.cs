using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class FluentEmitter
	{
		public VariableDefinition GetVariable(string varname)
		{
			if(string.IsNullOrEmpty(varname))
				throw new ArgumentException("varname cannot be null or empty"); //ncrunch: no coverage

			var var = Variables.FirstOrDefault(v => v.Name == varname);

			if (var == null)
				throw new KeyNotFoundException($"found no variable with name '{varname}'"); //ncrunch: no coverage

			return var;
		}

		public FluentEmitter Ldloc(params uint[] indexes)
		{
			if(indexes == null)
				throw new ArgumentNullException(nameof(indexes)); //ncrunch: no coverage

			foreach (var i in indexes)
			{
				if(Variables.Count <= i)
					throw new IndexOutOfRangeException($"no variable found at index {i}"); //ncrunch: no coverage

				switch (i)
				{
					case 0:
						Emit(OpCodes.Ldloc_0);
						break;
					case 1:
						Emit(OpCodes.Ldloc_1);
						break;
					case 2:
						Emit(OpCodes.Ldloc_2);
						break;
					case 3:
						Emit(OpCodes.Ldloc_3);
						break;
					default:
						Emit(i < sbyte.MaxValue ? OpCodes.Ldloc_S : OpCodes.Ldloc, Variables[(int)i]);
						break;
				}
			}

			return this;
		}

		public FluentEmitter Ldloc(params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			foreach (var name in names)
				Ldloc((uint) GetVariable(name).Index);

			return this;
		}

		public FluentEmitter Ldloc(params VariableDefinition[] vars)
		{
			if (vars == null)
				throw new ArgumentNullException(nameof(vars)); //ncrunch: no coverage

			foreach (var var in vars)
			{
				if (var == null)
					throw new ArgumentNullException($"variable is null"); //ncrunch: no coverage
				if (Variables.All(v => v != var))
					throw new ArgumentException("variable must be declared in method body before using it"); //ncrunch: no coverage

				Ldloc((uint)var.Index);
			}

			return this;
		}

		public FluentEmitter Ldloca(params VariableDefinition[] vars)
		{
			if (vars == null)
				throw new ArgumentNullException(nameof(vars)); //ncrunch: no coverage

			foreach (var var in vars)
			{
				if (var == null)
					throw new ArgumentNullException($"variable is null"); //ncrunch: no coverage
				if (Variables.All(v => v != var))
					throw new ArgumentException("variable must be declared in method body before using it"); //ncrunch: no coverage

				Ldloca((uint)var.Index);
			}

			return this;
		}

		public FluentEmitter Ldloca(params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			foreach (var name in names)
				Ldloca((uint)GetVariable(name).Index);

			return this;
		}

		public FluentEmitter Ldloca(params uint[] indexes)
		{
			if (indexes == null)
				throw new ArgumentNullException(nameof(indexes)); //ncrunch: no coverage

			foreach (var i in indexes)
			{
				if (Variables.Count <= i)
					throw new IndexOutOfRangeException($"no variable found at index {i}"); //ncrunch: no coverage
				
				Emit(i < sbyte.MaxValue ? OpCodes.Ldloca_S : OpCodes.Ldloca, Variables[(int)i]);
				break;
			}

			return this;
		}

		public FluentEmitter Stloc(params uint[] indexes)
		{
			if (indexes == null)
				throw new ArgumentNullException(nameof(indexes)); //ncrunch: no coverage

			foreach (var i in indexes)
			{
				if (Variables.Count <= i)
					throw new IndexOutOfRangeException($"no variable found at index {i}"); //ncrunch: no coverage

				switch (i)
				{
					case 0:
						Emit(OpCodes.Stloc_0);
						break;
					case 1:
						Emit(OpCodes.Stloc_1);
						break;
					case 2:
						Emit(OpCodes.Stloc_2);
						break;
					case 3:
						Emit(OpCodes.Stloc_3);
						break;
					default:
						Emit(i < sbyte.MaxValue ? OpCodes.Stloc_S : OpCodes.Stloc, Variables[(int) i]);
						break;
				}
			}

			return this;
		}

		public FluentEmitter Stloc(params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			foreach (var name in names)
				Stloc((uint) GetVariable(name).Index);

			return this;
		}

		public FluentEmitter Stloc(MagicNumberArgument value, params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			var count = names.Length;

			var variablegroups = names
				.Select(GetVariable)
				.GroupBy(v => v.VariableType.GetILType())
				.OrderBy(vgroup => vgroup.Sum(v => v.Index));

			foreach (var vargroup in variablegroups)
			{
				var isfirst = true;
				foreach (var variable in vargroup.OrderBy(v => v.Index))
				{
					if (isfirst)
					{
						switch (variable.VariableType.GetILType())
						{
							case ILType.I4: value.EmitLdcI4(this); break;
							case ILType.I8: value.EmitLdcI8(this); break;
							case ILType.R8: value.EmitLdcR8(this); break;
							case ILType.R4: value.EmitLdcR4(this); break;
							default: throw new NotSupportedException( // ncrunch: no coverage
									"variable type must be primitive valuetype in system namespace and convertible to I4, I8, R4 or R8");
						}
						isfirst = false;
					}
					else
						Dup();
				}
				foreach (var variable in vargroup)
				{
					Stloc((uint) variable.Index);
				}
			}

			return this;
		}

		public FluentEmitter Stloc(params VariableDefinition[] vars)
		{
			if (vars == null)
				throw new ArgumentNullException(nameof(vars)); //ncrunch: no coverage

			foreach (var var in vars)
			{
				if (var == null)
					throw new ArgumentNullException($"variable is null"); //ncrunch: no coverage
				if (Variables.All(v => v != var))
					throw new ArgumentException("variable must be declared in method body before using it"); //ncrunch: no coverage

				Stloc((uint)var.Index);
			}

			return this;
		}
	}
}