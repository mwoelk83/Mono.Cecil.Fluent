using System;
using System.Collections.Generic;

namespace Mono.Cecil.Fluent.Utils
{
	internal static class Generate
	{
		public static class Name
		{
			private static readonly Random _rnd = new Random();
			private static readonly object _syncRoot = new object();
			private static readonly HashSet<string> UsedClassNames = new HashSet<string>();
			private static readonly HashSet<string> UsedMethodNames = new HashSet<string>();
			private const string IdentifierFirstLetterChars = "abcdefghijklmnopqrstuvwxyz";
			private const string IdentifierChars = "abcdefghijklmnopqrstuvwxyz0123456789";

			public static string ForMethod()
			{
				return GenereateInternal(UsedMethodNames);
			}

			public static string ForClass()
			{
				return GenereateInternal(UsedClassNames);
			}

			private static string GenereateInternal(HashSet<string> used)
			{
				var ret = "";
				ret += IdentifierFirstLetterChars[_rnd.Next(0, IdentifierFirstLetterChars.Length - 1)];
				ret += IdentifierChars[_rnd.Next(0, IdentifierChars.Length - 1)];

				while (true)
				{
					if (ret.Length > 16)
						ret = ret.Substring(0, 2);

					ret += IdentifierChars[_rnd.Next(0, IdentifierChars.Length - 1)];
					ret += IdentifierChars[_rnd.Next(0, IdentifierChars.Length - 1)];

					lock (_syncRoot)
					{
						if (used.Contains(ret))
							continue;
						used.Add(ret);
						return ret;
					}
				}
			}

		}
	}
}
