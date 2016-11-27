using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody LdThis()
		{
			if(!MethodDefinition.HasThis)
				throw new InvalidOperationException("can not load this parameter for static methods"); // ncrunch: no coverage

			return Emit(OpCodes.Ldarg_0);
		}

		public ParameterDefinition GetParameter(string paramname)
		{
			if (string.IsNullOrEmpty(paramname))
				throw new ArgumentException("paramname cannot be null or empty"); //ncrunch: no coverage

			var param = Parameters.FirstOrDefault(v => v.Name == paramname);

			if (param == null)
				throw new KeyNotFoundException($"found no parameter with name '{paramname}'"); //ncrunch: no coverage

			return param;
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
				if (Parameters.Count + (MethodDefinition.HasThis ? 1 : 0) <= i)
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
				Ldarg((uint) GetParameter(name).Index);

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
				Starg((uint) GetParameter(name).Index);

			return this;
		}

		public FluentMethodBody Starg(MagicNumberArgument value, params string[] names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names)); //ncrunch: no coverage

			var count = names.Length;

			var paramgroups = names
				.Select(GetParameter)
				.GroupBy(p => p.ParameterType.GetILType())
				.OrderBy(pgroup => pgroup.Sum(p => p.Index));

			foreach (var paramgroup in paramgroups)
			{
				var isfirst = true;
				foreach (var param in paramgroup.OrderBy(p => p.Index))
				{
					if (isfirst)
					{
						switch (param.ParameterType.GetILType())
						{
							case ILType.I4: value.EmitLdcI4(this); break;
							case ILType.I8: value.EmitLdcI8(this); break;
							case ILType.R8: value.EmitLdcR8(this); break;
							case ILType.R4: value.EmitLdcR4(this); break;
							default:
								throw new NotSupportedException( // ncrunch: no coverage
							   "variable type must be primitive valuetype in system namespace and convertible to I4, I8, R4 or R8");
						}
						isfirst = false;
					}
					else
						Dup();
				}
				foreach (var param in paramgroup)
				{
					Starg((uint)param.Index);
				}
			}

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