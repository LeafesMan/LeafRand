using System;
using System.Runtime.CompilerServices;
namespace LeafRand.Extensions
{
    public static class ArrayExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] source) => source;
    }
}