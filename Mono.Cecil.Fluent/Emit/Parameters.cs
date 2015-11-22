using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody Ldthis()
		{
			if(!MethodDefinition.HasThis)
				throw new InvalidOperationException("can not load this parameter for static methods"); // ncrunch: no coverage

			return Emit(OpCodes.Ldarg_0);
		}

		public uint GetParameterIndex(string paramname)
		{
			if (string.IsNullOrEmpty(paramname))
				throw new ArgumentException("paramname cannot be null or empty"); //ncrunch: no coverage

			var @params = Parameters.ToArray();

			for (var i = 0; i < @params.Length; ++i)
				if (@params[i].Name == paramname)
					return MethodDefinition.IsStatic ? (uint)i : (uint) i + 1;

			throw new KeyNotFoundException($"found no parameter with name '{paramname}'"); //ncrunch: no coverage
		}

		public FluentMethodBody LdParam(params uint[] indexes)
		{
			if (indexes == null)
				throw new ArgumentNullException(nameof(indexes)); //ncrunch: no coverage

			if (!MethodDefinition.HasThis)
				return Ldarg(indexes);

			for (var i = 0; i < indexes.Length; ++i)
				++indexes[i];

			return Ldarg(indexes);
		}

		public FluentMethodBody LdParam(params string[] names)
		{
			return Ldarg(names);
		}

		/// <summary>
		/// you should use LdParam() because it takes care of the this parameter
		/// </summary>
		public FluentMethodBody Ldarg(params uint[] indexes)
		{
			if (indexes == null)
				throw new ArgumentNullException(nameof(indexes)); //ncrunch: no coverage

			foreach (var i in indexes)
			{
				if (Parameters.Count <= i)
					throw new IndexOutOfRangeException($"no parameter found at index {i}"); //ncrunch: no coverage

				switch (i)
				{
					case 0:
						Emit(OpCodes.Ldarg_0);
						break;
					case 1:
						Emit(OpCodes.Ldarg_1);
						break;
					case 2:
						Emit(OpCodes.Ldarg_2);
						break;
					case 3:
						Emit(OpCodes.Ldarg_3);
						break;
					default:
						Emit(i < sbyte.MaxValue ? OpCodes.Ldarg_S : OpCodes.Ldarg, Parameters[(int)i]);
						break;
				}
			}

			return this;
		}

		public FluentMethodBody Ldarg(params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			foreach (var name in names)
				Ldarg(GetParameterIndex(name));

			return this;
		}

		public FluentMethodBody Ldarg(params ParameterDefinition[] @params)
		{
			if (@params == null)
				throw new ArgumentNullException(nameof(@params)); //ncrunch: no coverage

			foreach (var param in @params)
			{
				if (param == null)
					throw new ArgumentNullException($"parameter is null"); //ncrunch: no coverage
				if (Parameters.All(v => v != param))
					throw new ArgumentException("parameter must be declared in method definition before using it"); //ncrunch: no coverage

				Ldarg((uint)param.Index);
			}

			return this;
		}

		public FluentMethodBody Starg(params uint[] indexes)
		{
			if (indexes == null)
				throw new ArgumentNullException(nameof(indexes)); //ncrunch: no coverage

			foreach (var i in indexes)
			{
				if (Parameters.Count <= i)
					throw new IndexOutOfRangeException($"no parameter found at index {i}"); //ncrunch: no coverage
				
				Emit(i < sbyte.MaxValue ? OpCodes.Starg_S : OpCodes.Starg, Parameters[(int)i]);
			}

			return this;
		}

		public FluentMethodBody Starg(params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			foreach (var name in names)
				Starg(GetParameterIndex(name));

			return this;
		}

		public FluentMethodBody Starg(NumberArgument value, params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			value.EmitLdc(this);

			var count = names.Length;
			for (var i = 1; i < count; i++)
				Dup();

			for (var i = 0; i < count; i++)
				Starg(GetParameterIndex(names[i]));

			return this;
		}

		public FluentMethodBody Starg(params ParameterDefinition[] @params)
		{
			if (@params == null)
				throw new ArgumentNullException(nameof(@params)); //ncrunch: no coverage

			foreach (var param in @params)
			{
				if (param == null)
					throw new ArgumentNullException($"parameter is null"); //ncrunch: no coverage
				if (Parameters.All(v => v != param))
					throw new ArgumentException("parameter must be declared in method declaration before using it"); //ncrunch: no coverage

				Starg((uint)param.Index);
			}

			return this;
		}
	}
}