namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		public FluentMethodBody AbsI4()
		{
			return System.Math.Abs<int>();
		}
		public FluentMethodBody AbsI8()
		{
			return System.Math.Abs<long>();
		}

		public FluentMethodBody AbsR4()
		{
			return System.Math.Abs<float>();
		}

		public FluentMethodBody AbsR8()
		{
			return System.Math.Abs<double>();
		}
	}
}
