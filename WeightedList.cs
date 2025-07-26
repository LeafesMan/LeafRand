/*
 * Auth: Ian
 * 
 * Proj: Rand
 * 
 * Desc: A single type weighted list that exposes nicely in the inspector.
 * 
 * Date: 11/18/24
 */
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedList<T>
{
    [SerializeField] List<T> items = new();
    [SerializeField] List<float> weights = new();

    /// <summary>
    /// Returns a readonly items wrapper.
    /// </summary>
    public IReadOnlyList<T> Items => items;
    /// <summary>
    /// Returns a readonly weights wrapper.
    /// </summary>
    public IReadOnlyList<float> Weights => weights;
    public int Count => items.Count;
    public void Add(T item, float weight)
    {
        items.Add(item);
        weights.Add(weight);

    }
    public void SetItemAt(int index, T item) => items[index] = item;
    public void SetWeightAt(int index, float weight) => weights[index] = weight;
    public int IndexOf(T item) => items.IndexOf(item);
    public void Remove(T item)
    {
        int i = items.IndexOf(item);
        if (i == -1) throw new System.Exception($"Item {item} not found in Weighted List! Remove an item that exists in the list."); 

        items.RemoveAt(i);
        weights.RemoveAt(i);
    }
    public void RemoveAt(int index)
    {
        items.RemoveAt(index);
        weights.RemoveAt(index);
    }
    public WeightedList<T> Clone()
    {
        WeightedList<T> copy = new WeightedList<T>();
        copy.items = new List<T>(items);
        copy.weights = new List<float>(weights);

        return copy;
    }
}