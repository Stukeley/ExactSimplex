namespace ExactSimplex.Models
{
	using System;
	using Services;

	public class SimplexIndexResult
	{
		public readonly Tuple<int, int> Index;
		public readonly SimplexResult Result;

		public SimplexIndexResult(Tuple<int, int> index, SimplexResult result)
		{
			Index = index;
			Result = result;
		}
	}
}