namespace ExactSimplex.Models
{
	using Fractions;

	/// <summary>
	/// A function to be optimized.
	/// Example: max P = 20x1 + 10x2 + 15x3
	/// Supports two types of functions: max and min.
	/// </summary>
	public class Function
	{
		public Fraction C;
		public readonly bool IsExtrMax;
		public readonly Fraction[] Variables;

		public Function(Fraction[] variables, Fraction c, bool isExtrMax)
		{
			Variables = variables;
			C = c;
			IsExtrMax = isExtrMax;
		}
	}
}