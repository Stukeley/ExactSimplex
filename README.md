# ExactSimplex
An implementation of the Simplex algorithm in C# that gives exact results (in form of fractions). Based on: https://github.com/timdolenko/simplex

Available as a NuGet package: https://www.nuget.org/packages/ExactSimplex/1.0.0

# Why?

Countless implementations of the Simplex algorithms exist online, in multiple languages, but it is hard to find an implementation in C# that gives exact, precise results.

Most of the available implementations operate on the 'double' data type, which has limited precision and makes the results look weird (and just straight-up wrong).

This code is meant to help those who seek an exact result for a Linear Programming problem. An exact result is in the form of (numerator/denominator).

# Example

An example of how to use ExactSimplex is given in the ExactSimplex.Example console project.

Consider the following LP problem:

Get the exact solution to the following problem:  
max z = 141x1 + 393x2 + 273x3 + 804x4 + 175x5  
3x1 + 5x2 + 2x3 + 5x4 + 4x5 <= 36  
7x1 + 12x2 + 11x3 + 10x4 <= 21  
-3x2 + 12x3 + 7x4 + 2x5 <= 17  
0 <= x1, x2, x3, x4, x5 <= 20  

### Create an array of Constraints with the desired amount of variables (just their coefficients).

```csharp
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
```

### Create a function to be minimized or maximized.

```csharp
var function = new Function(new Fraction[] { 141, 393, 273, 804, 175 }, new Fraction(0), true);
```

### Create an instance of Simplex and call GetResult on it.

```csharp
var simplex = new Simplex(function, constraints);
var result = simplex.GetResult();
```

### Interpret the result.

```csharp
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
```

You can also retrieve the simplex tableau.

```csharp
var tableau = result.Results.Last().Matrix;
```


# Dependencies and references
The project is based on the following publicly available projects and packages:
- [Fractions](https://www.nuget.org/packages/Fractions)
- [Simplex by Tim Dolenko](https://github.com/timdolenko/simplex)
