using System;
using UnityEngine;

namespace LeafRand
{
    /// <summary>
    /// Scriptable object holding an array of weighted gameobjects.
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeightedObjects", menuName = "Weighted Collection/GameObject")]
    public class WeightedGameObjects : ScriptableObject
    {
        [SerializeField] Weighted<GameObject>[] weightedGameObjects = new Weighted<GameObject>[0];

        public static implicit operator ReadOnlySpan<Weighted<GameObject>>(WeightedGameObjects weightedGameObjects) => weightedGameObjects.weightedGameObjects.AsSpan();
        public static implicit operator Span<Weighted<GameObject>>(WeightedGameObjects weightedGameObjects) => weightedGameObjects.weightedGameObjects.AsSpan();

        public Weighted<GameObject> this[int index] => weightedGameObjects[index];

        public Weighted<GameObject>[] Clone() => (Weighted<GameObject>[])weightedGameObjects.Clone();
    }
}