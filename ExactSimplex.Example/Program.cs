// Sample code to invoke the ExactSimplex class.
// Problem description:
// Get the exact solution of the following problem:
// max z = 141𝑥1 + 393𝑥2 + 273𝑥3 + 804𝑥4 + 175𝑥5
// 3𝑥1 + 5𝑥2 + 2𝑥3 + 5𝑥4 + 4𝑥5 ≤ 36
// 7𝑥1 + 12𝑥2 + 11𝑥3 + 10𝑥4 ≤ 21
// −3𝑥2 + 12𝑥3 + 7𝑥4 + 2𝑥5 ≤ 17
// 0 ≤ 𝑥1, 𝑥2, 𝑥3, 𝑥4, 𝑥5 ≤ 20

using System;
using System.Linq;
using ExactSimplex.Models;
using ExactSimplex.Services;
using Fractions;

var constraints = new Constraint[]
{
	new Constraint(new Fraction[] { 3, 5, 2, 5, 4 }, new Fraction(36), "<="),
	new Constraint(new Fraction[] { 7, 12, 11, 10, 0 }, new Fraction(21), "<="),
	new Constraint(new Fraction[] { 0, -3, 12, 7, 2 }, new Fraction(17), "<="),
	new Constraint(new Fraction[] { 1, 0, 0, 0, 0 }, new Fraction(20), "<="),
	new Constraint(new Fraction[] { 1, 0, 0, 0, 0 }, new Fraction(0), ">="),
	new Constraint(new Fraction[] { 0, 1, 0, 0, 0 }, new Fraction(20), "<="),
	new Constraint(new Fraction[] { 0, 1, 0, 0, 0 }, new Fraction(0), ">="),
	new Constraint(new Fraction[] { 0, 0, 1, 0, 0 }, new Fraction(20), "<="),
	new Constraint(new Fraction[] { 0, 0, 1, 0, 0 }, new Fraction(0), ">="),
	new Constraint(new Fraction[] { 0, 0, 0, 1, 0 }, new Fraction(20), "<="),
	new Constraint(new Fraction[] { 0, 0, 0, 1, 0 }, new Fraction(0), ">="),
	new Constraint(new Fraction[] { 0, 0, 0, 0, 1 }, new Fraction(20), "<="),
	new Constraint(new Fraction[] { 0, 0, 0, 0, 1 }, new Fraction(0), ">=")
};

var function = new Function(new Fraction[] { 141, 393, 273, 804, 175 }, new Fraction(0), true);

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