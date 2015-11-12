
using Should.Core.Assertions;
using Should.Fluent;
using Should.Fluent.Model;

namespace Mono.Cecil.Fluent.Tests
{
	internal static class TestUtils
	{
		public static void Equal<T>(this Should<object, Be<object>> that)
		{
			that.Apply(Action<T>);
		}

		private static async void Action<T>(object t, IAssertProvider _, bool arg3)
		{
			Assert.IsType<TypeReference>(t);
			((TypeReference)t).FullName.Should().Equal(typeof(T).FullName);
		}
	}
}
