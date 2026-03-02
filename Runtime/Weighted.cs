using Unity.Mathematics;
using UnityEngine;

namespace LeafRand
{
    /// <summary>
    /// An item-weight pair.
    /// </summary>
    [System.Serializable]
    public struct Weighted<T>
    {
        [SerializeField] T item;
        [SerializeField] float weight;

        public T Item { readonly get => item; set => item = value; }
        public float Weight { readonly get => weight; set => weight = math.max(0, value); }

        public Weighted(T item, float weight = 1)
        {
            this.item = item;
            this.weight = math.max(0,weight);
        }
    }
}
