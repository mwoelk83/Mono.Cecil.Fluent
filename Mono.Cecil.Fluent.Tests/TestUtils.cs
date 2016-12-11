using System.Diagnostics;
using Should.Core.Assertions;
using Should.Fluent;
using Should.Fluent.Model;

namespace Mono.Cecil.Fluent.Tests
{
	internal static class TestUtils
	{
		[DebuggerStepThrough]
		// ReSharper disable once UnusedMember.Global
		public static T DebuggerBreak<T>(this T obj)
		{
			Debugger.Break();
			return obj;
		}

		public static void Equal<T>(this Should<object, Be<object>> that)
		{
			that.Apply((t, _, __) =>
			{
				Assert.IsType<TypeReference>(t);
				((TypeReference)t).FullName.Should().Equal(typeof(T).FullName);
			});
		}
	}
}
