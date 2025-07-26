using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Weighted<T>
{
    [SerializeField] T element;
    public T Element { get => element; set => element = value; }
    [SerializeField] float weight;
    public float Weight { get => weight; set => weight = value; }

    public Weighted(T element, float weight)
    {
        this.element = element;
        this.weight = weight;
    }

    /// <summary>Returns index of weighted Element with same element. -1 if not found.</summary>
    public static int IndexOfElement(List<Weighted<T>> Weighteds, T element)
    {
        for( int i = 0; i < Weighteds.Count; i++)
            if (Weighteds[i].Element.Equals(element)) 
                return i;
        
        return -1;
    }
}
