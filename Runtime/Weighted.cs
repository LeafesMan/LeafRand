namespace LeafRand
{
    /// <summary>
    /// An item-weight pair.
    /// </summary>
    [System.Serializable]
    public struct Weighted<T>
    {
        public T item;
        public float weight;
    }
}
