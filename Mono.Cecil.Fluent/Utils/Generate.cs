using System;
using System.Collections.Generic;

namespace Mono.Cecil.Fluent.Utils
{
	internal static class Generate
	{
		public static class Name
		{
			// ncrunch: no coverage start
			private static readonly Random Rnd = new Random();
			private static readonly object SyncRoot = new object();
			private static readonly HashSet<string> UsedClassNames = new HashSet<string>();
			private static readonly HashSet<string> UsedMethodNames = new HashSet<string>();

			// ncrunch: no coverage end
			private const string IdentifierFirstLetterChars = "abcdefghijklmnopqrstuvwxyz";
			private const string IdentifierChars = "abcdefghijklmnopqrstuvwxyz0123456789";

			public static string ForMethod()
			{
                // todo: echeck if method name exists in class
				return GenereateInternal(UsedMethodNames);
			}

			public static string ForClass()
			{
                // todo: check if class name exists in namespace
				return GenereateInternal(UsedClassNames);
			}
			
			private static string GenereateInternal(ISet<string> used)
			{
				var ret = "";
				ret += IdentifierFirstLetterChars[Rnd.Next(0, IdentifierFirstLetterChars.Length - 1)];
				ret += IdentifierChars[Rnd.Next(0, IdentifierChars.Length - 1)];

				while (true)
				{
					if (ret.Length > 16)
						ret = ret.Substring(0, 2); // ncrunch: no coverage

					ret += IdentifierChars[Rnd.Next(0, IdentifierChars.Length - 1)];
					ret += IdentifierChars[Rnd.Next(0, IdentifierChars.Length - 1)];

					lock (SyncRoot)
					{
						if (used.Contains(ret))
							continue; // ncrunch: no coverage
						used.Add(ret);
						return ret;
					}
				}
			}

		}
	}
}
