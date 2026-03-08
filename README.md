# LeafRand
A unified, deterministic, burst-compatible random library for Unity supporting global, managed, and burst calls. This is effectively a set of Unity.Mathematics.Random wrappers with extensive high-level helper functions.

## Types
Unity provides different RNG types for different use cases, each with its own API, supported primitives, and seeding behavior.

**Unity Types**
- `UnityEngine.Random`       — global calls
- `System.Random`            — instanced class
- `Unity.Mathematics.Random` — instanced burst-compatible struct
> Seeding is not consistent across these types and `System.Random` is not consistent across platforms

**LeafRand Types**
- `LeafRand.Rand`                — global calls
- `LeafRand.Instanced.Rand`      — instanced class
- `LeafRand.Instanced.BurstRand` — instanced burst-compatible struct 

> Seeding is consistent across these types

## Helpers
Random primitives `uint`, `int`, `float`, `double`, and `bool`
- Some of Unity’s built-in RNGs lack primitive helpers 
    - `UnityEngine.Random` only supports `int` and `float`
    - `System.Random` only supports `int` and `double`

Random sampling uniform and weighted with or without replacement returning items or indices
- Overloads that take `Span` input and output
- Editor tooling for weighted collections

Random directions, colors, and shuffling
> **Performance:** This library is optimized for IL2CPP builds and Burst. Mono performance suffers due to poor inlining of multiple wrappers.