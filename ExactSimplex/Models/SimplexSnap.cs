namespace ExactSimplex.Models
{
	using Fractions;
	using Helpers;

	/// <summary>
	/// Contains information about a single step of the simplex algorithm.
	/// </summary>
	public class SimplexSnap
	{
		public Fraction[] B;
		public int[] C;
		public Fraction[] F;
		public Fraction FValue;
		public Fraction[] FVars;
		public bool IsMDone;
		public bool[] Mb;
		public Fraction[] M;
		public Fraction[][] Matrix;

		public SimplexSnap(Fraction[] b, Fraction[][] matrix, Fraction[] m, Fraction[] f, int[] c, Fraction[] fVars, bool isMDone, bool[] mb)
		{
			B = ArrayHelper.Copy(b);
			Matrix = ArrayHelper.Copy(matrix);
			M = ArrayHelper.Copy(m);
			F = ArrayHelper.Copy(f);
			C = ArrayHelper.Copy(c);
			IsMDone = isMDone;
			Mb = ArrayHelper.Copy(mb);
			FVars = ArrayHelper.Copy(fVars);
			
			FValue = 0;
			
			for (int i = 0; i < c.Length; i++)
			{
				FValue += fVars[c[i]] * b[i];
			}
		}
	}
}