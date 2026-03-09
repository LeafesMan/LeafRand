using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

namespace LeafRand.Collections
{
    /// <summary>
    /// Stores a collection of weighted generics. Implement to create a WeightedCollectionAsset type for any serializable type.
    /// </summary>
    public abstract class WeightedCollectionAsset<T> : ScriptableObject, IReadOnlyList<Weighted<T>>
    {
        [SerializeField] private Weighted<T>[] weightedArray = Array.Empty<Weighted<T>>();

        public Weighted<T> this[int index] => weightedArray[index];

        public int Count => weightedArray.Length;

        public Weighted<T>[] CopyAsArray() => weightedArray.ToArray();
        public List<Weighted<T>> CopyAsList() => weightedArray.ToList();

        public IEnumerator<Weighted<T>> GetEnumerator() => ((IEnumerable<Weighted<T>>)weightedArray).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ReadOnlySpan<Weighted<T>>(WeightedCollectionAsset<T> asset) => asset.weightedArray.AsSpan();
    }

}