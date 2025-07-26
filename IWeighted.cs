/*
 * Auth: Ian
 * 
 * Proj: Wall
 * 
 * Date: 1/20/24
 * 
 * Desc: Forces the implementer to have a Weight and Item property
 */

public interface IWeighted<T> 
{
    public T Item { get; }
    public float Weight { get; }
}