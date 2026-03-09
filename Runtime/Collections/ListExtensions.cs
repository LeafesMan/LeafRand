using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace LeafRand.Collections
{
    public static class ListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Span<T> AsSpan<T>(this List<T> source) => UnityInternals.AsSpan(source);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static ReadOnlySpan<T> AsReadOnlySpan<T>(this List<T> source) => UnityInternals.AsSpan(source);
    }
}