// Sample code to invoke the ExactSimplex class.
// Problem description:
// Maximize:
// P = 20x1 + 10x2 + 15x3
// Subject to:
// 3x1 + 2x2 + 5x3 <= 55
// 2x1 + x2 + x3 <= 26
// x1 + x2 + 3x3 <= 30
// 5x1 + 2x2 + 4x3 <= 57
// x1, x2, x3 >= 0

using System;
using System.Linq;
using ExactSimplex.Models;
using ExactSimplex.Services;
using Fractions;

var constraints = new Constraint[]
{
	new Constraint(new Fraction[] { 3, 2, 5 }, new Fraction(55), "<="),
	new Constraint(new Fraction[] { 2, 1, 1 }, new Fraction(26), "<="),
	new Constraint(new Fraction[] { 1, 1, 3 }, new Fraction(30), "<="),
	new Constraint(new Fraction[] { 5, 2, 4 }, new Fraction(57), "<="),
	new Constraint(new Fraction[] { 1, 0, 0 }, new Fraction(0), ">="),
	new Constraint(new Fraction[] { 0, 1, 0 }, new Fraction(0), ">="),
	new Constraint(new Fraction[] { 0, 0, 1 }, new Fraction(0), ">=")
};

var function = new Function(new Fraction[] { 20, 10, 15 }, new Fraction(0), true);

var simplex = new Simplex(function, constraints);

var result = simplex.GetResult();

switch (result.ResultType)
{
	case SimplexResult.Unbounded:
		Console.WriteLine("Unbounded.");
		break;

	case SimplexResult.NotYetFound:
		Console.WriteLine("Solution wasn't found after 100 steps.");
		break;

	case SimplexResult.Found:
		Console.WriteLine("Solution was found.");
		Console.WriteLine("Function value: " + result.Results.Last().FValue);
		break;
}