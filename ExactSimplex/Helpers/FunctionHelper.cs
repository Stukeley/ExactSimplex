namespace ExactSimplex.Helpers
{
	using Fractions;
	using Models;

	public static class FunctionHelper
	{
		public static Function Canonize(Function function)
		{
			var newFuncVars = new Fraction[function.Variables.Length];

			for (int i = 0; i < function.Variables.Length; i++)
			{
				newFuncVars[i] = function.Variables[i].Invert();
			}

			return new Function(newFuncVars, function.C.Invert(), true);
		}
	}
}