namespace ExactSimplex.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Fractions;
	using Helpers;
	using Models;

	/// <summary>
	/// Class that represents the simplex algorithm.
	/// </summary>
	public class Simplex
	{
		private readonly Function _function;
		private Fraction[] _b;
		private int[] _c;
		private Fraction[] _f;
		private Fraction[] _functionVariables;
		private bool _isMDone;
		private bool[] _mb;
		private Fraction[] _m;
		private Fraction[][] _matrix;
		private readonly List<SimplexSnap> _snaps;

		public Simplex(Function function, Constraint[] constraints)
		{
			_snaps = new List<SimplexSnap>();
			_function = function.IsExtrMax ? function : FunctionHelper.Canonize(function);

			GetMatrix(constraints);
			GetFunctionArray();
			GetMandF();

			for (int i = 0; i < _f.Length; i++)
			{
				_f[i] = _functionVariables[i].Invert();
			}
		}

		/// <summary>
		/// Use this method to get the result of the simplex algorithm.
		/// </summary>
		/// <returns>A list of steps of the Simplex algorithm, and its result type.</returns>
		public (List<SimplexSnap> Results, SimplexResult ResultType) GetResult()
		{
			_snaps.Clear();
			_snaps.Add(new SimplexSnap(_b, _matrix, _m, _f, _c, _functionVariables, _isMDone, _mb));

			var result = NextStep();
			int i = 0;
			while (result.Result == SimplexResult.NotYetFound && i < 100)
			{
				CalculateSimplexTableau(result.Index);
				_snaps.Add(new SimplexSnap(_b, _matrix, _m, _f, _c, _functionVariables, _isMDone, _mb));
				result = NextStep();
				i++;
			}

			return (_snaps, result.Result);
		}

		/// <summary>
		/// Use this method to get the function value for the problem's solution directly.
		/// </summary>
		/// <returns>The final function value.</returns>
		public Fraction GetFunctionValue()
		{
			var value = _snaps.LastOrDefault()?.FValue ?? 0;
			
			return value;
		}
		
		/// <summary>
		/// Use this method to get the variable values for the problem's solution.
		/// </summary>
		/// <returns>An array of Fractions representing each variable's value.</returns>
		public Fraction[] GetVariableValues()
		{
			// 1. Get the number of variables.
			var numberOfVariables = _function.Variables.Length;
			
			// 2. Create an array of Fractions to store the values.
			var variableValues = new Fraction[numberOfVariables];
			
			// 3. Look for unit columns in the matrix (only the first (numberOfVariables) columns).
			for (int i = 0; i < numberOfVariables; i++)
			{
				var column = _matrix[i];
				bool isASingleOne = column.Count(fraction => fraction == 1) == 1;
				bool areAllZeros = column.Count(fraction => fraction == 0) == column.Length - 1;

				if (isASingleOne && areAllZeros)
				{
					// 4. If a unit column is found, get the index of the row that contains the unit.
					int unitRowIndex = 0;

					for (int j = 0; j < column.Length; j++)
					{
						if (column[j] == 1)
						{
							unitRowIndex = j;
							break;
						}
					}
					
					// 5. Get the value of the variable from the b array.
					variableValues[i] = _b[unitRowIndex];
				}
				else
				{
					// 6. If it's not a unit column, the variable's value is 0.
					variableValues[i] = 0;
				}
			}

			return variableValues;
		}

		private void CalculateSimplexTableau(Tuple<int, int> xij)
		{
			var newMatrix = new Fraction[_matrix.Length][];

			_c[xij.Item2] = xij.Item1;

			var newJRow = new Fraction[_matrix.Length];

			for (int i = 0; i < _matrix.Length; i++)
			{
				newJRow[i] = _matrix[i][xij.Item2] / _matrix[xij.Item1][xij.Item2];
			}

			var newB = new Fraction[_b.Length];

			for (int i = 0; i < _b.Length; i++)
			{
				if (i == xij.Item2)
				{
					newB[i] = _b[i] / _matrix[xij.Item1][xij.Item2];
				}
				else
				{
					newB[i] = _b[i] - _b[xij.Item2] / _matrix[xij.Item1][xij.Item2] * _matrix[xij.Item1][i];
				}
			}

			_b = newB;

			for (int i = 0; i < _matrix.Length; i++)
			{
				newMatrix[i] = new Fraction[_c.Length];
				for (int j = 0; j < _c.Length; j++)
				{
					if (j == xij.Item2)
					{
						newMatrix[i][j] = newJRow[i];
					}
					else
					{
						newMatrix[i][j] = _matrix[i][j] - newJRow[i] * _matrix[xij.Item1][j];
					}
				}
			}

			_matrix = newMatrix;
			GetMandF();
		}

		private void GetMandF()
		{
			_m = new Fraction[_matrix.Length];
			_f = new Fraction[_matrix.Length];

			for (int i = 0; i < _matrix.Length; i++)
			{
				Fraction sumF = 0;
				Fraction sumM = 0;
				for (int j = 0; j < _matrix.First().Length; j++)
				{
					if (_mb[_c[j]])
					{
						sumM -= _matrix[i][j];
					}
					else
					{
						sumF += _functionVariables[_c[j]] * _matrix[i][j];
					}
				}

				_m[i] = _mb[i] ? sumM + 1 : sumM;
				_f[i] = sumF - _functionVariables[i];
			}
		}

		private SimplexIndexResult NextStep()
		{
			int columnM = GetIndexOfNegativeElementWithMaxAbsoluteValue(_m);

			if (_isMDone || columnM == -1)
			{
				//M doesn't have negative values
				_isMDone = true;
				int columnF = GetIndexOfNegativeElementWithMaxAbsoluteValue(_f);

				if (columnF != -1) //Has at least 1 negative value
				{
					int row = GetIndexOfMinimalRatio(_matrix[columnF], _b);

					if (row != -1)
					{
						return new SimplexIndexResult(new Tuple<int, int>(columnF, row), SimplexResult.NotYetFound);
					}

					return new SimplexIndexResult(null, SimplexResult.Unbounded);
				}

				return new SimplexIndexResult(null, SimplexResult.Found);
			}

			{
				int row = GetIndexOfMinimalRatio(_matrix[columnM], _b);

				if (row != -1)
				{
					return new SimplexIndexResult(new Tuple<int, int>(columnM, row), SimplexResult.NotYetFound);
				}

				return new SimplexIndexResult(null, SimplexResult.Unbounded);
			}
		}

		private int GetIndexOfNegativeElementWithMaxAbsoluteValue(Fraction[] array)
		{
			int index = -1;

			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] < 0)
				{
					if (!_isMDone || (_isMDone && !_mb[i]))
					{
						if (index == -1)
						{
							index = i;
						}
						else if (array[i].Abs() > array[index].Abs())
						{
							index = i;
						}
					}
				}
			}

			return index;
		}

		private static int GetIndexOfMinimalRatio(Fraction[] column, Fraction[] b)
		{
			int index = -1;

			for (int i = 0; i < column.Length; i++)
			{
				if (column[i] > 0 && b[i] > 0)
				{
					if (index == -1)
					{
						index = i;
					}
					else if (b[i] / column[i] < b[index] / column[index])
					{
						index = i;
					}
				}
			}

			return index;
		}

		private void GetFunctionArray()
		{
			var funcVars = new Fraction[_matrix.Length];
			for (int i = 0; i < _matrix.Length; i++)
			{
				funcVars[i] = i < _function.Variables.Length ? _function.Variables[i] : 0;
			}

			_functionVariables = funcVars;
		}

		private static Fraction[][] AppendColumn(Fraction[][] matrix, Fraction[] column)
		{
			var newMatrix = new Fraction[matrix.Length + 1][];
			for (int i = 0; i < matrix.Length; i++)
			{
				newMatrix[i] = matrix[i];
			}

			newMatrix[matrix.Length] = column;
			return newMatrix;
		}

		private static Fraction[] GetColumn(Fraction value, int place, int length)
		{
			var newColumn = new Fraction[length];

			for (int k = 0; k < length; k++)
			{
				newColumn[k] = k == place ? value : 0;
			}

			return newColumn;
		}

		private void GetMatrix(Constraint[] constraints)
		{
			for (int i = 0; i < constraints.Length; i++)
			{
				if (constraints[i].B < 0)
				{
					var cVars = new Fraction[constraints[i].Variables.Length];

					for (int j = 0; j < constraints[i].Variables.Length; j++)
					{
						cVars[j] = cVars[j].Invert();
					}

					string sign = constraints[i].Sign;

					if (sign == ">=")
					{
						sign = "<=";
					}
					else if (sign == "<=")
					{
						sign = ">=";
					}

					var cNew = new Constraint(cVars, constraints[i].B.Invert(), sign);
					constraints[i] = cNew;
				}
			}

			var matrix = new Fraction[constraints.First().Variables.Length][];

			for (int i = 0; i < constraints.First().Variables.Length; i++)
			{
				matrix[i] = new Fraction[constraints.Length];
				for (int j = 0; j < constraints.Length; j++)
				{
					matrix[i][j] = constraints[j].Variables[i];
				}
			}

			var appendixMatrix = new Fraction[0][];
			var Bs = new Fraction[constraints.Length];

			for (int i = 0; i < constraints.Length; i++)
			{
				var current = constraints[i];

				Bs[i] = current.B;

				if (current.Sign == ">=")
				{
					appendixMatrix = AppendColumn(appendixMatrix, GetColumn(-1, i, constraints.Length));
				}
				else if (current.Sign == "<=")
				{
					appendixMatrix = AppendColumn(appendixMatrix, GetColumn(1, i, constraints.Length));
				}
			}

			var newMatrix = new Fraction[constraints.First().Variables.Length + appendixMatrix.Length][];

			for (int i = 0; i < constraints.First().Variables.Length; i++)
			{
				newMatrix[i] = matrix[i];
			}

			for (int i = constraints.First().Variables.Length; i < constraints.First().Variables.Length + appendixMatrix.Length; i++)
			{
				newMatrix[i] = appendixMatrix[i - constraints.First().Variables.Length];
			}

			bool[] hasBasicVar = new bool[constraints.Length];

			for (int i = 0; i < constraints.Length; i++)
			{
				hasBasicVar[i] = false;
			}

			_c = new int[constraints.Length];

			int ci = 0;
			for (int i = 0; i < newMatrix.Length; i++)
			{
				bool hasOnlyNulls = true;
				bool hasOne = false;
				var onePosition = new Tuple<int, int>(0, 0);
				for (int j = 0; j < constraints.Length; j++)
				{
					if (newMatrix[i][j] == 1)
					{
						if (hasOne)
						{
							hasOnlyNulls = false;
							break;
						}

						hasOne = true;
						onePosition = new Tuple<int, int>(i, j);
					}
					else if (newMatrix[i][j] != 0)
					{
						hasOnlyNulls = false;
						break;
					}
				}

				if (hasOnlyNulls && hasOne)
				{
					hasBasicVar[onePosition.Item2] = true;
					_c[ci] = onePosition.Item1;
					ci++;
				}
			}

			_mb = new bool[newMatrix.Length];

			for (int i = 0; i < newMatrix.Length; i++)
			{
				_mb[i] = false;
			}

			for (int i = 0; i < constraints.Length; i++)
			{
				if (!hasBasicVar[i])
				{
					var basicColumn = new Fraction[constraints.Length];

					for (int j = 0; j < constraints.Length; j++)
					{
						basicColumn[j] = j == i ? 1 : 0;
					}

					newMatrix = AppendColumn(newMatrix, basicColumn);
					_mb = ArrayHelper.Append(_mb, true);
					_c[ci] = newMatrix.Length - 1;
					ci++;
				}
			}

			_b = Bs;
			this._matrix = newMatrix;
		}
	}
}