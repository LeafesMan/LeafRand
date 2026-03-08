using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This whole class and assembly is just a work around to get Nonallocated access to a lists backing array.<br></br>
/// When Unity finishes the process of switching from mono to CoreClr we can switch to CollectionsMarshal.AsSpan() and drop this whole hack-job assembly
/// </summary>
public class UnityInternals : MonoBehaviour
{
    /// <summary>
    /// Returns a reference to the list's internal array. <br></br>
    /// * This is unsafe territory when the list resizes it may point to a new array.<br></br>
    /// * Remember the size of this array is not the length of the list but the amount of space the list currently allocated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static Span<T> AsSpan<T>(List<T> list) => NoAllocHelpers.ExtractArrayFromList(list).AsSpan()[..list.Count];
}