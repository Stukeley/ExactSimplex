namespace ExactSimplex.Models
{
	using Fractions;

	/// <summary>
	/// A constraint in the form of Ax1 + Bx2 + Cx3 + ... <= B.
	/// Sign can be either <= or >=.
	/// </summary>
	public class Constraint
	{
		public Fraction B;
		public readonly string Sign;
		public readonly Fraction[] Variables;

		public Constraint(Fraction[] variables, Fraction b, string sign)
		{
			Variables = variables;
			B = b;
			Sign = sign;
		}
	}
}