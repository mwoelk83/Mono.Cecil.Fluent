using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Cecil.Fluent
{
	partial class MethodDefinitionExtensions
	{
		public static T Compile<T>(this FluentMethodBody method)
			where T : class
		{
			if (!typeof(T).IsSubclassOf(typeof(Delegate)))
				throw new Exception("T must be delegate");

			try
			{
				var ret = method.ToDynamicMethod().CreateDelegate(typeof(T)) as T;
				if (ret == null)
					throw new Exception($"method signature missmatch.");
				return ret;
			}
			catch (Exception ex)
			{
				var message = ex.Message + Environment.NewLine;
				message += "decompiled method:" + Environment.NewLine;
				message += method.Disassemble();
				throw new Exception(message);
			}
		}

		public static T Compile<T>(this MethodDefinition method)
			where T : class
		{
			if (!typeof(T).IsSubclassOf(typeof(Delegate)))
				throw new Exception("T must be delegate");

			try
			{
				var ret = new FluentMethodBody(method).ToDynamicMethod().CreateDelegate(typeof(T)) as T;
				if (ret == null)
					throw new Exception($"method signature missmatch.");
				return ret;
			}
			catch (Exception ex)
			{
				var message = ex.Message + Environment.NewLine;
				message += "decompiled method:" + Environment.NewLine;
				message += method.Disassemble();
				throw new Exception(message);
			}
		}
	}
}
