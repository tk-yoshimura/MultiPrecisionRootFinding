# MultiPrecisionRootFinding
 MultiPrecision Root Finding Method Implements 

## Requirement
.NET 10.0  
AVX2 suppoted CPU. (Intel:Haswell(2013)-, AMD:Excavator(2015)-)  
[MultiPrecision](https://github.com/tk-yoshimura/MultiPrecision)

## Install

[Download DLL](https://github.com/tk-yoshimura/MultiPrecisionRootFinding/releases)  
[Download Nuget](https://www.nuget.org/packages/tyoshimura.multiprecision.rootfinding/)  

## Usage
```csharp
// Newton-Raphson Method: solve x for x^3 = 2
static (MultiPrecision<Pow2.N8> v, MultiPrecision<Pow2.N8> d) f(MultiPrecision<Pow2.N8> x) {
    return (x * x * x - 2, 3 * x * x);
}

MultiPrecision<Pow2.N8> y = NewtonRaphsonFinder<Pow2.N8>.RootFind(f, x0: 2);
```

```csharp
// Brent Method: solve x for x^3 = 2
static MultiPrecision<Pow2.N8> f(MultiPrecision<Pow2.N8> x) {
    return x * x * x - 2;
}

MultiPrecision<Pow2.N8> y = BrentFinder<Pow2.N8>.RootFind(f, x1: 1, x2: 2);
```

## Licence
[MIT](https://github.com/tk-yoshimura/MultiPrecisionRootFinding/blob/main/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)
