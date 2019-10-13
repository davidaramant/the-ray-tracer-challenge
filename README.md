# The Ray Tracer Challenge

WIP implementation of the book [The Ray Tracer Challenge](http://raytracerchallenge.com/) by Jamis Buck.

Implemented using .NET Core 3.

## Chapter Notes

### Chapter 1 - Tuples

 Used [`Vector4`](https://docs.microsoft.com/en-us/dotnet/api/system.numerics.vector4?view=netcore-3.0) as the tuple primitive.  I attempted to use [SpecFlow](https://specflow.org/) for the supplied Cucumber tests but it seemed too cumbersome.

### Chapter 2 - Canvas

Looked into using [FNA](https://fna-xna.github.io/) as a real-time canvas since outputting images seems simple enough to add in later should I need it. Unfortunately just like MonoGame, FNA is still tied to .NET Framework as of 2019-10-13 (although they are working on supporting Core).  

I decided to use the [WPF version](https://github.com/davidaramant/DotNetPixelByPixel) since I wanted to keep everything .NET Core.  Had some difficulties porting it over; I guess they dropped some methods in `WriteableBitmap` along the way?

Did not bother with the projectile example the book used since I already had a simplistic interactive demo.
