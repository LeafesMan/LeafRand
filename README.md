# LeafRand
A unified, deterministic, Burst-compatible random library for Unity with a consistent API across global, managed, and Burst contexts. 

Unity provides multiple RNG systems for different use cases, each with its own API, supported primitives, and seeding behavior. Identical seeds do not produce identical sequences across these systems.

LeafRand replaces this fragmented ecosystem with a single consistent RNG family. All types share the same API, which is designed to reduce boilerplate without sacrificing performance. They produce identical sequences when initialized with the same seed, regardless of context or platform, and outperform their native counterparts.



## Key Features

### Deterministic and Cross-Platform
- Identical seeded results across:
  - Global, managed, and Burst RNG types
  - Platforms and build targets

### Unified API
Unity: `Random.value()`, `(float)rand.NextDouble()`, or `rand.NextFloat()`

LeafRand: `Rand.Float()` or `rand.Float()`

**Unity RNG Types**
- `UnityEngine.Random`       for Global calls
- `System.Random`            for reference semantics
- `Unity.Mathematics.Random` for Burst calls

**LeafRand Types**
- `LeafRand.Rand`        for Global calls
- `LeafRand.ManagedRand` for reference semantics
- `LeafRand.BurstRand`   for Burst calls

### Extensive Helpers
- Primitives `uint`, `int`, `float`, `double`, and `bool` with more to come
    - All have overloads you would expect
    - Unity’s built-in RNGs lack primitive helpers 
        - `UnityEngine.Random` only supports `int` and `float`
        - `System.Random` only supports `int` and `double`
- Uniform and weighted sampling with or without replacement returning items or indices
    - Includes editor tooling for weighted collections
    - Overloads that take `Span` input and output
- Directions, colors, points, and shuffling

### Performance
This library is optimized for IL2CPP builds and Burst.
Mono performance suffers due to poor inlining on multiple wrappers.

**Random Primitive Generation**

in Burst or IL2CPP builds is faster than `UnityEngine.Random` and `System.Random` and effectively equivalent to `Unity.Mathematics.Random`

in Mono builds is slower than `UnityEngine.Random`, `System.Random`, and `Mathematics.Random`

> **Recommendation:** Use IL2CPP for release builds for maximum performance.