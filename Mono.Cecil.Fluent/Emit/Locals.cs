using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public uint GetVariableIndex(string varname)
		{
			if(string.IsNullOrEmpty(varname))
				throw new ArgumentException("varname cannot be null or empty"); //ncrunch: no coverage

			var vars = Body.Variables.ToArray();

			for(var i = 0; i < vars.Length; ++i)
				if (vars[i].Name == varname)
					return (uint)i;

			throw new KeyNotFoundException($"found no variable with name '{varname}'"); //ncrunch: no coverage
		}

		public FluentMethodBody Ldloc(params uint[] indexes)
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
						Emit(i < sbyte.MaxValue ? OpCodes.Ldloc_S : OpCodes.Ldloc, Body.Variables[(int)i]);
						break;
				}
			}

			return this;
		}

		public FluentMethodBody Ldloc(params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			foreach (var name in names)
				Ldloc(GetVariableIndex(name));

			return this;
		}

		public FluentMethodBody Ldloc(params VariableDefinition[] vars)
		{
			if (vars == null)
				throw new ArgumentNullException(nameof(vars)); //ncrunch: no coverage

			foreach (var var in vars)
			{
				if(var == null)
					throw new ArgumentNullException($"variable is null"); //ncrunch: no coverage
				if (Variables.All(v => v != var))
					throw new ArgumentException("variable must be declared in method body before using it"); //ncrunch: no coverage

				Ldloc((uint)var.Index);
			}

			return this;
		}

		public FluentMethodBody Stloc(params uint[] indexes)
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
						Emit(i < sbyte.MaxValue ? OpCodes.Stloc_S : OpCodes.Stloc, Body.Variables[(int) i]);
						break;
				}
			}

			return this;
		}

		public FluentMethodBody Stloc(params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			foreach (var name in names)
				Stloc(GetVariableIndex(name));

			return this;
		}

		public FluentMethodBody Stloc(NumberArgument value, params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			value.EmitLdc(this);

			var count = names.Length;
			for (var i = 1; i < count; i++)
				Dup();

			for (var i = 0; i < count; i++)
				Stloc(GetVariableIndex(names[i]));

			return this;
		}

		public FluentMethodBody Stloc(params VariableDefinition[] vars)
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