# The Ray Tracer Challenge

WIP implementation of the book [The Ray Tracer Challenge](http://raytracerchallenge.com/) by Jamis Buck.

Implemented using .NET Core 3.

## Chapter Notes

### Chapter 1 - Tuples

 Used [`Vector4`](https://docs.microsoft.com/en-us/dotnet/api/system.numerics.vector4?view=netcore-3.0) as the tuple primitive.  I attempted to use [SpecFlow](https://specflow.org/) for the supplied Cucumber tests but it seemed too cumbersome.

### Chapter 2 - Canvas

Looked into using [FNA](https://fna-xna.github.io/) as a real-time canvas since outputting images seems simple enough to add in later should I need it. Unfortunately, just like [MonoGame](http://www.monogame.net/), FNA is still tied to .NET Framework as of 2019-10-13.  Both projects are working on supporting Core.

I decided to use the [WPF example I've made](https://github.com/davidaramant/DotNetPixelByPixel) since I wanted to keep everything .NET Core.  Had some difficulties porting it over until I realized I had used [WriteableBitmapEx](https://github.com/reneschulte/WriteableBitmapEx) in the example.  That library is now also Core 3 compatible; yay!

Did not bother with the projectile example the book used since I already had a simplistic interactive demo.

### Chapter 3 - Matrices

Unsurprisingly used [`Matrix4x4`](https://docs.microsoft.com/en-us/dotnet/api/system.numerics.matrix4x4?view=netcore-3.0).  Unfortunately .NET did not seem to include a method for multiplying a matrix by a `Vector4`.  I guess since `Vector4` is supposed to represent a row and they don't want to confuse things by using it for columns too?

There also weren't types for 2x2 or 3x3 matrices.  Kind of tedious.

Not sure if these SIMD types like `Vector4` really benefit from being passed as `ref` or not.

### Chapter 4 - Matrix Transformations

With regards to the previous chapter, _of course_ there is a method to multiply a `Matrix4x4` and a `Vector4` (`Vector4.Transform`); it's just that it's based around `Vector4` being a row vector and not a column vector.  Ran into this headache when trying to make a method to construct a shear matrix since there didn't seem to be one.  Hopefully this won't cause too much confusion for later chapters.  **Really** glad the book includes tests for everything!

### Chapter 5 - Ray-Sphere Intersections

Perhaps I got a bit too clever and went ahead of the book in some ways...  But, eventually I found all the stupid typos and mistakes I made and everything works.  

I ended up making a simple console runner to directly spit out an image to test my abstracted ray tracer renderer.  Once that worked, I managed to fix the WPF viewer to actually work the way I envisioned it.  The console runner has a nice progress bar, the WPF renderer will visualize the traced scene in real-time, and everything is nicely threaded!  I only wish I had more threads to throw at it...

### Chapter 6 - Light and Shading

I don't remember too many surprises from this chapter.

I was severely tempted to hook up the key bindings again to be able to move the sphere around on screen, but I figure I'll wait until the book introduces a real camera.

I included [Colourful](https://github.com/tompazourek/Colourful) to support the conversion from linear RGB (which, I suspect, is what the book is really using) to sRGB.  I learned about this from a wonderful [talk at Strange Loop 2019](https://www.youtube.com/watch?v=AS1OHMW873s).  Messing with CIELAB would be interesting too, but my understanding is that would drastically change all the math done on colors throughout the ray tracer and I would probably get lost from diverging from the book too much.

### Chapter 7 - Making a Scene

Pretty long chapter!

* Ran into a few hiccups that thankfully were due to a transcription error on my part as well as forgetting some of the things I've already implemented.  Once I remembered that, oh yeah, there was a special type for intersections that the book had me make, it went a bit smoother.  I ended up deleting that type in favor of a single extension method which I'm not sure was a good idea or not... It certainly cuts down on boilerplate code (since it was just a `List` with a single method) but I feel like all these static methods everywhere aren't quite as discoverable.
* I really dislike the name of the "`Computations`" class but I'm not sure I can come up with a better name.  
* I had to write a bunch of boilerplate equality code for things like shapes that probably wasn't worth it.  Equality is definitely a sore spot in C# that feels pretty archaic - should it really require like four methods to really _really_ define equality?
* The `Matrix4x4` did have a `CreateLookAt` method that mostly does the same thing as `CreateViewTransform` but the book threw in some tweaks that later chapters probably depend on so I ended up keeping the custom one.  The .NET one looks to be a bit faster but this method won't be called very often so who cares.
* Still pretty happy with the speed of the rendering.
* I _could_ at this point hook up the camera to movement controls in the GUI app, but I don't have the energy right now.

### Chapter 8 - Shadows

Big problem here; the tests pass but the output looks horrible.  I've narrowed it down to the `OverPoint` calculation in `Computations`.  Using the shifted amount that the book uses, the large "spheres" that act as the walls of the scene have tons of visual artifacts.  If that distance is made a lot larger they diminish.  Not sure what on earth that's about.

I don't know how to reason about why this is happening, so hopefully when planes are implemented in the next chapter the issue will magically go away...

### Chapter 9 - Planes

* No real surprises in the content of the chapter.  I had to name the plane `XZPlane` since `System.Numerics.Plane` already exists and it would have been tedious to specify which one to use in every file.
* I implemented the concept of rendering at a lower resolution and started down the path of making everything dynamically react to changes in the output resolution.  Being able to move the camera around and change rendering quality at run time will be a bit tricky since the current rendering has to be interrupted.  Updating the variables used in the current rendering pass would cause severe problems.
* Spent some time trying to debug the visual artifacts from the previous chapter.  I'm not sure where the resolution of a `float` is causing an issue but I refuse to change to `double` on both pragmatic and philosophical grounds (no fast ray tracer has to resort to `double`s).  Switching the walls from incredibly distorted spheres to actual planes did clear it up, but when I moved the camera around a bit I did see them come back...

### Chapter 10 - Patterns

* Another fairly straightforward chapter.  Just like with shapes I didn't bother implementing the generic tests.
* Shoving the pattern shading method on `IShape` _works_ but feels a bit clunky... I wonder if I'm just sticking to OO patterns because that's what I'm used to.  I suppose there's nothing preventing me from moving them out to free standing static functions in the future.
* Didn't bother trying to do pattern combinations quite yet.
* I did some work on a hack to get around that horrible `float` issue.  I made a new `FarOverPoint` on `Computation` that's shifted farther out and that's what's used for determining if a point is in shadow.
