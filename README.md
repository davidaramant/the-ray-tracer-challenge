# The Ray Tracer Challenge

WIP implementation of the book [The Ray Tracer Challenge](http://raytracerchallenge.com/) by Jamis Buck.

Implemented using .NET Core 3.

## Chapter Notes

### Chapter 1 - Tuples

 Used [`Vector4`](https://docs.microsoft.com/en-us/dotnet/api/system.numerics.vector4?view=netcore-3.0) as the tuple primitive.  I attempted to use [SpecFlow](https://specflow.org/) for the supplied Cucumber tests but it seemed too cumbersome.

### Chapter 2 - Canvas

Looked into using [FNA](https://fna-xna.github.io/) as a real-time canvas since outputting images seems simple enough to add in later should I need it. Unfortunately, just like MonoGame, FNA is still tied to .NET Framework as of 2019-10-13.  Both projects are working on supporting Core.

I decided to use the [WPF example I've made](https://github.com/davidaramant/DotNetPixelByPixel) since I wanted to keep everything .NET Core.  Had some difficulties porting it over until I realized I had used [WriteableBitmapEx](https://github.com/reneschulte/WriteableBitmapEx) in the example.  That library is now also Core 3 compatible; yay!

Did not bother with the projectile example the book used since I already had a simplistic interactive demo.

### Chapter 3 - Matrices

Unsurprisingly used [`Matrix4x4`](https://docs.microsoft.com/en-us/dotnet/api/system.numerics.matrix4x4?view=netcore-3.0).  Unfortunately .NET did not seem to include a method for multiplying a matrix by a `Vector4`.  I guess since `Vector4` is supposed to represent a row and they don't want to confuse things by using it for columns too?

There also weren't types for 2x2 or 3x3 matrices.  Kind of tedious.

Not sure if these SIMD types like `Vector4` really benefit from being passed as `ref` or not.

### Chapter 4 - Matrix Transformations

With regards to the previous chapter, _of course_ there is a method to multiple a `Matrix4x4` and a `Vector4` (`Vector4.Transform`); it's just that it's based around `Vector4` being a row vector and not a column vector.  Ran into this headache when trying to make a method to construct a shear matrix since there didn't seem to be one.  Hopefully this won't cause too much confusion for later chapters.  **Really** glad the book includes tests for everything!
