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
			private static readonly HashSet<string> _usedClassNames = new HashSet<string>();
			public const string IdentifierFirstLetterChars = "abcdefghijklmnopqrstuvwxyz";
			public const string IdentifierChars = "abcdefghijklmnopqrstuvwxyz0123456789";

			public static string ForClass()
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
						if (_usedClassNames.Contains(ret))
							continue;
						_usedClassNames.Add(ret);
						return ret;
					}
				}
			}
			
		}
	}
}
