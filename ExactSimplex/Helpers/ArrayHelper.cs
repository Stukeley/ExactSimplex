namespace ExactSimplex.Helpers
{
	using System.Linq;

	public static class ArrayCopyHelper
	{
		public static T[] Copy<T>(T[] array)
		{
			var newArr = new T[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				newArr[i] = array[i];
			}

			return newArr;
		}

		public static T[][] Copy<T>(T[][] matrix)
		{
			var newMatr = new T[matrix.Length][];
			for (int i = 0; i < matrix.Length; i++)
			{
				newMatr[i] = new T[matrix.First().Length];
				for (int j = 0; j < matrix.First().Length; j++)
				{
					newMatr[i][j] = matrix[i][j];
				}
			}

			return newMatr;
		}
	}
}