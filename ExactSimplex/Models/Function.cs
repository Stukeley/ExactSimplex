namespace ExactSimplex.Models
{
	using Fractions;

	/// <summary>
	/// A function to be optimized.
	/// Example: max z = 141x1 + 393x2 + 273x3 + 804x4 + 175x5
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