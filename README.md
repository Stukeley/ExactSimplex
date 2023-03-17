# ExactSimplex
An implementation of the Simplex algorithm in C# that gives exact results (in form of fractions). Based on: https://github.com/timdolenko/simplex

Available as a NuGet package: https://www.nuget.org/packages/ExactSimplex

# Why?

Countless implementations of the Simplex algorithms exist online, in multiple languages, but it is hard to find an implementation in C# that gives exact, precise results.

Most of the available implementations operate on the 'double' data type, which has limited precision and makes the results look weird (and just straight-up wrong).

This code is meant to help those who seek an exact result for a Linear Programming problem. An exact result is in the form of (numerator/denominator).

# Example

An example of how to use ExactSimplex is given in the ExactSimplex.Example console project.

**Consider the following LP problem.**

Maximize: 

```math
P = 20x1 + 10x2 + 15x3
```

Subject to:
```math
3x1 + 2x2 + 5x3 \le 55
```

```math
2x1 + x2 + x3 \le 26
```

```math
x1 + x2 + 3x3 \le 30
```

```math
5x1 + 2x2 + 4x3 \le 57
```

```math
x1, x2, x3 \ge 0
```

### Create an array of Constraints with the desired amount of variables (just their coefficients).

```csharp
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
```

### Create a function to be minimized or maximized.

```csharp
var function = new Function(new Fraction[] { 20, 10, 15 }, new Fraction(0), true);
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
		// Function value: 268
		break;
}
```

You can use the GetVariableValues method to retrieve the final values of the variables.

```csharp
var variableValues = simplex.GetVariableValues();

for (int i = 0; i < variableValues.Length; i++)
{
    Console.WriteLine($"x{i+1} = {variableValues[i]}");
}

// Variable values:
// x1 = 9/5
// x2 = 104/5
// x3 = 8/5
```

You can also retrieve the simplex tableau.

```csharp
var tableau = result.Results.Last().Matrix;
```


# Dependencies and references
The project is based on the following publicly available projects and packages:
- [Fractions](https://www.nuget.org/packages/Fractions)
- [Simplex by Tim Dolenko](https://github.com/timdolenko/simplex)
