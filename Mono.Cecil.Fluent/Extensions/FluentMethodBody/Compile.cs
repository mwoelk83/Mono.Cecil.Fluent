using System;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBodyExtensions
    {
        /// <summary>
        /// Compilation of FluentMethodBody and bind to given to Func or Action signature.
        /// </summary>
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
	}
}
