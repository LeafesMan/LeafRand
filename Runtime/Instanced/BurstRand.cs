using UnityEngine;
using Unity.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LeafRand.Collections;

namespace LeafRand.Instanced
{
    /// <summary>
    /// Burst-compatible RNG struct with high-level helper methods.<br></br>
    /// Faster than Global and Managed Rand. State is a single uint.
    /// </summary>    
    public struct BurstRand
    {
        #region Core
        /// <include file="../Docs.xml" path="Doc/Items/Class"/>
        Unity.Mathematics.Random state;

        /// <include file="../Docs.xml" path="Doc/Seed"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public BurstRand(uint seed) => state = new(seed);

        /// <include file="../Docs.xml" path="Doc/Seed"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetSeed(uint seed) => state.InitState(seed);
        /// <include file="../Docs.xml" path="Doc/CreateScrambled"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static BurstRand CreateScrambled(uint seed) => new() { state = Unity.Mathematics.Random.CreateFromIndex(seed) };
        #endregion
        #region Helpers
        #region Num
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public uint UInt() => state.NextUInt();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public uint UInt(uint max) => state.NextUInt(max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public uint UInt(uint min, uint max) => state.NextUInt(min, max);

        /// <include file="../Docs.xml" path="Doc/Num/Int">
        /// </include>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int Int() => state.NextInt();
        /// <include file="../Docs.xml" path="Doc/Num/Int"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int Int(int max) => state.NextInt(max);
        /// <include file="../Docs.xml" path="Doc/Num/IntInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int Int(int min, int max) => state.NextInt(min, max);
        /// <include file="../Docs.xml" path="Doc/Num/Vector2Int"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int Int(Vector2Int range) => state.NextInt(range.x, range.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int IntInclusive(int max) => state.NextInt(max + 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int IntInclusive(int min, int max) => state.NextInt(min, max + 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int IntInclusive(Vector2Int range) => state.NextInt(range.x, range.y + 1);

        /// <include file="../Docs.xml" path="Doc/Num"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public float Float() => state.NextFloat();
        /// <include file="../Docs.xml" path="Doc/Num/Float"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public float Float(float max) => state.NextFloat(max);
        /// <include file="../Docs.xml" path="Doc/Num/FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public float Float(float min, float max) => state.NextFloat(min, max);
        /// <include file="../Docs.xml" path="Doc/Num/Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public float Float(Vector2 range) => state.NextFloat(range.x, range.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public double Double() => state.NextDouble();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public double Double(double max) => state.NextDouble(max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public double Double(double min, double max) => state.NextDouble(min, max);
        /// <include file="../Docs.xml" path="Doc/Angle"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public float Angle() => state.NextFloat(2 * Mathf.PI);
        #endregion
        #region Bool
        /// <include file="../Docs.xml" path="Doc/Bool"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool Bool() => state.NextBool();
        /// <include file="../Docs.xml" path="Doc/Chance"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool Chance(float probability) => state.NextFloat() < probability;
        /// <include file="../Docs.xml" path="Doc/Chance"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool Chance(double probability) => state.NextDouble() < probability;
        #endregion
        #region Direction
        /// <include file="../Docs.xml" path="Doc/Sign"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int Sign() => state.NextInt(2) * 2 - 1;
        #region 2D
        /// <include file="../Docs.xml" path="Doc/Dir2D"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public Vector2 Dir2D() => Dir2D(0, 360);
        /// <include file="../Docs.xml" path="Doc/Dir2D/FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Dir2D(float minTheta, float maxTheta)
        {
            float thetaRadians = state.NextFloat(minTheta, maxTheta) * Mathf.Deg2Rad;
            return new(Mathf.Cos(thetaRadians), Mathf.Sin(thetaRadians));
        }

        /// <include file="../Docs.xml" path="Doc/Dir2D/Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public Vector2 Dir2D(Vector2 thetaRange) => Dir2D(thetaRange.x, thetaRange.y);
        /// <include file="../Docs.xml" path="Doc/Dir2D/Vector2FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Dir2D(Vector2 basis, float minDegrees, float maxDegrees)
        {   // Normalize the basis to ensure direction is normal
            basis.Normalize();

            // Get Random Rotate in the passed range and convert to RADs
            float randomDegrees = state.NextFloat(minDegrees, maxDegrees) * Mathf.Deg2Rad;

            // Calculate direction relative to basis rotating randomDegrees
            float newX = basis.x * Mathf.Cos(randomDegrees) - basis.y * Mathf.Sin(randomDegrees);
            float newY = basis.x * Mathf.Sin(randomDegrees) + basis.y * Mathf.Cos(randomDegrees);

            return new(newX, newY);
        }
        /// <include file="../Docs.xml" path="Doc/Dir2D/Vector2Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public Vector2 Dir2D(Vector2 basis, Vector2 degreesRange) => Dir2D(basis, degreesRange.x, degreesRange.y);

        /// <include file="../Docs.xml" path="Doc/Dir2D/Vector2Float"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public Vector2 Dir2D(Vector2 basis, float withinDegrees) => Dir2D(basis, -withinDegrees, withinDegrees);
        #endregion
        #region 3D
        /// <include file="../Docs.xml" path="Doc/Dir"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public Vector3 Dir() => Dir(0, 360, 0, 180);
        /// <include file="../Docs.xml" path="Doc/Dir/FloatFloatFloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Dir(float thetaRangeX, float thetaRangeY, float phiRangeX, float phiRangeY)
        {   // Get Random Theta
            float thetaRadians = state.NextFloat(thetaRangeX, thetaRangeY) * Mathf.Deg2Rad;

            // Get Random Phi
            float randPhiDegrees = state.NextFloat(phiRangeX, phiRangeY);
            //if (randPhiDegrees > 180) randPhiDegrees %= 180;  // Inputs above 180 wrap back around to 0
            //else if (randPhiDegrees < 0) randPhiDegrees = 1;
            float randPhi01 = randPhiDegrees / 180;           // Remap to (0,1)

            // Need to redistribute phi otherwise our random directions will be weighted toward the poles
            // A little formula to redistribute values thanks to: https://dornsife.usc.edu/sergey-lototsky/wp-content/uploads/sites/211/2023/06/UniformOnTheSphere.pdf
            // domain of [0,1] range -> [0,PI] so need to convert input degrees (0, 180) -> (0,1)
            float phiRadians = Mathf.Acos(1 - 2 * randPhi01);

            // Grab direction based on theta and phi
            float x = Mathf.Cos(thetaRadians) * Mathf.Sin(phiRadians);
            float y = Mathf.Cos(phiRadians);
            float z = Mathf.Sin(thetaRadians) * Mathf.Sin(phiRadians);

            return new(x, y, z);
        }

        /// <include file="../Docs.xml" path="Doc/Dir/Vector2Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public Vector3 Dir(Vector2 thetaRange, Vector2 phiRange) => Dir(thetaRange.x, thetaRange.y, phiRange.x, phiRange.y);
        /// <include file="../Docs.xml" path="Doc/Dir/Vector3FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Dir(Vector3 basis, float minDegrees, float maxDegrees)
        {   // Normalize basis
            basis.Normalize();

            Vector3 chosenDirection = Dir(0, 360, minDegrees, maxDegrees);


            var randRotation = Quaternion.FromToRotation(Vector3.up, basis);


            return randRotation * chosenDirection;
        }
        /// <include file="../Docs.xml" path="Doc/Dir/Vector3Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public Vector3 Dir(Vector3 basis, Vector2 degreesRange) => Dir(basis, degreesRange.x, degreesRange.y);
        /// <include file="../Docs.xml" path="Doc/Dir/Vector3Float"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Dir(Vector3 basis, float withinDegrees)
        {
            Vector3 withinDegreesOfUp = Dir(0, 360, 0, withinDegrees);

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, withinDegreesOfUp);

            return rotation * basis;
        }
        #endregion
        #endregion
        #region Color
        /// <include file="../Docs.xml" path="Doc/Color"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Color(Vector2 hueRange, Vector2 saturationRange, Vector2 valueRange)
        {
            return UnityEngine.Color.HSVToRGB(state.NextFloat(hueRange.x, hueRange.y), state.NextFloat(saturationRange.x, saturationRange.y), state.NextFloat(valueRange.x, valueRange.y));
        }
        #endregion
        #region Sampling
        #region Generic Selectors
        internal interface ISelector<In, Out> { Out Select(ReadOnlySpan<In> from, int index); }
        internal readonly struct ItemSelector<T> : ISelector<T, T> { [MethodImpl(MethodImplOptions.AggressiveInlining)] public readonly T Select(ReadOnlySpan<T> from, int index) => from[index]; }
        internal readonly struct IndexSelector<T> : ISelector<T, int> { [MethodImpl(MethodImplOptions.AggressiveInlining)] public readonly int Select(ReadOnlySpan<T> from, int index) => index; }
        internal readonly struct WeightedItemSelector<T> : ISelector<Weighted<T>, T> { [MethodImpl(MethodImplOptions.AggressiveInlining)] public readonly T Select(ReadOnlySpan<Weighted<T>> from, int index) => from[index].Item; }
        internal readonly struct WeightedIndexSelector<T> : ISelector<Weighted<T>, int> { [MethodImpl(MethodImplOptions.AggressiveInlining)] public readonly int Select(ReadOnlySpan<Weighted<T>> from, int index) => index; }
        #endregion
        #region Core
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Out SampleUniform<In, Out, Selector>(ReadOnlySpan<In> source) where Selector : struct, ISelector<In, Out>
        {
            Selector selector = default;
            return selector.Select(source, state.NextInt(source.Length));
        }
        internal Out SampleWeighted<In, Out, Selector>(ReadOnlySpan<Weighted<In>> source) where Selector : struct, ISelector<Weighted<In>, Out>
        {
            Selector selector = default;
            float sumWeights = 0;
            foreach (var item in source) sumWeights += item.Weight;

            if (sumWeights == 0) throw new ArgumentException("Sum of weights must be positive!", nameof(sumWeights));

            // Return Weighted Random Element
            float randVal = state.NextFloat() * sumWeights;
            float weightPosition = 0;
            for (int i = 0; i < source.Length; i++)
            {
                weightPosition += source[i].Weight;
                if (weightPosition > randVal)
                    return selector.Select(source, i);
            }

            // Based on the logic above this should be impossible!
            throw new Exception($"I don't know how this could possibly have occured!");
        }
        internal void SampleUniformWithReplacement<In, Out, Selector>(ReadOnlySpan<In> source, Span<Out> output) where Selector : struct, ISelector<In, Out>
        {
            Selector selector = default;

            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length == 0) throw new ArgumentException("Items must be non-empty.", nameof(source));
            if (output == null) throw new ArgumentNullException(nameof(output));

            for (int i = 0; i < output.Length; i++) output[i] = selector.Select(source, state.NextInt(source.Length));
        }
        internal void SampleUniformWithoutReplacement<In, Out, Selector>(ReadOnlySpan<In> source, Span<Out> output) where Selector : struct, ISelector<In, Out>
        {   // Input validation
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length == 0) throw new ArgumentException("Items must be non-empty.", nameof(source));
            if (output.Length > source.Length) throw new ArgumentException($"Cannot request more items than are in source! Requested {output.Length} items but only {source.Length} items in source!");

            // Determine Best Algorithm based on use reservoir threshold
            float USERESERVOIRTHRESHOLD = 0.14f;
            if ((float)output.Length / source.Length > USERESERVOIRTHRESHOLD) SampleUniformWithoutReplacementRetryMethod<In, Out, Selector>(source, output);
            else SampleUniformWithoutReplacementReservoirMethod<In, Out, Selector>(source, output);
        }
        internal void SampleUniformWithoutReplacementRetryMethod<In, Out, Selector>(ReadOnlySpan<In> source, Span<Out> output) where Selector : struct, ISelector<In, Out>
        {   // Kind of weird but pretty much O(infinity) worst case and O(k) best case
            // In practice lightning fast for small pick ratios (Picking < 15% of source)
            // relative to somthing like reservoir sampling which will still take O(n) when k is small

            Selector selecor = default;

            // Pre Hash to count Capacity
            // We know final map size so no need to Rehash on the fly            
            NativeHashSet<int> removed = new(output.Length, Allocator.Temp);

            // Pick items
            for (int i = 0; i < output.Length; i++)
            {
                // Keep Picking Until Distinct Index found
                // This can be very slow if Count is close to itemsLength
                int randIndex = state.NextInt(source.Length);
                while (removed.Contains(randIndex))
                    randIndex = state.NextInt(source.Length);

                removed.Add(randIndex);
                output[i] = selecor.Select(source, randIndex);
            }

            removed.Dispose();
        }
        internal void SampleUniformWithoutReplacementReservoirMethod<In, Out, Selector>(ReadOnlySpan<In> source, Span<Out> output) where Selector : struct, ISelector<In, Out>
        {
            Selector selector = default;

            // Initial Resevior
            int i = 0;
            for (; i < output.Length; i++)
            {
                output[i] = selector.Select(source, i);
            }

            // Roll for each item
            for (; i < source.Length; i++)
            {
                int random = state.NextInt(i + 1);
                if (random < output.Length) output[random] = selector.Select(source, i);
            }
        }
        internal void SampleWeightedWithReplacement<In, Out, Selector>(ReadOnlySpan<Weighted<In>> source, Span<Out> output) where Selector : struct, ISelector<Weighted<In>, Out>
        {   // Input Validation
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length == 0) throw new ArgumentException("Items must be non-empty.", nameof(source));
            if (output == null) throw new ArgumentNullException(nameof(output));


            // Decide on best algorithm
            if (source.Length * output.Length < 100 || (float)source.Length / 5 > output.Length) SampleWeightedWithReplacementAliasMethod<In, Out, Selector>(source, output);
            else SampleWeightedWithReplacementBinarySearchMethod<In, Out, Selector>(source, output);
        }
        internal void SampleWeightedWithReplacementAliasMethod<In, Out, Selector>(ReadOnlySpan<Weighted<In>> source, Span<Out> output) where Selector : struct, ISelector<Weighted<In>, Out>
        {   // Uses Vose's Alias method.
            // A lovely method for effecient generation of weighted random values
            // Time Complexity: O(n) setup and O(1) picks

            Selector selector = default;

            #region Setup
            // Initialize work Arrays
            NativeArray<float> weights = new(source.Length, Allocator.Temp);
            for (int i = 0; i < source.Length; i++) weights[i] = source[i].Weight;
            NativeArray<float> probability = new(weights.Length, Allocator.Temp);
            NativeArray<int> alias = new(weights.Length, Allocator.Temp);
            NativeList<int> aboveIndices = new(source.Length, Allocator.Temp);
            NativeList<int> belowIndices = new(source.Length, Allocator.Temp);

            // Split all Probabilities into 
            // stacks above or below average
            float sumWeights = 0;
            for (int i = 0; i < weights.Length; i++) sumWeights += weights[i];
            float avg = sumWeights / weights.Length; // Calculate Average
            for (int i = 0; i < weights.Length; i++)
            {
                if (weights[i] >= avg) aboveIndices.Add(i);
                else belowIndices.Add(i);
            }

            // Grab one from less and one from more
            while (belowIndices.Length != 0 && aboveIndices.Length != 0)
            {
                int aboveIndex = aboveIndices[aboveIndices.Length - 1];
                int belowIndex = belowIndices[belowIndices.Length - 1];

                aboveIndices.RemoveAt(aboveIndices.Length - 1);
                belowIndices.RemoveAt(belowIndices.Length - 1);


                // Scale Probabilities
                probability[belowIndex] = weights[belowIndex] / avg;
                alias[belowIndex] = aboveIndex;

                // Reduce probability of above by probability of below
                weights[aboveIndex] += weights[belowIndex] - avg;

                // Place above back into aboveAverage or belowAverage bucket based on it's new weight
                if (weights[aboveIndex] >= avg) aboveIndices.Add(aboveIndex);
                else belowIndices.Add(aboveIndex);
            }


            // Empty remaining elements from bbucket
            // Due to floating point imprecision we cant be sure which bucket will have elements remaining
            // (Although mathematically it should always be the above average bucket)
            foreach (var index in belowIndices) probability[index] = 1;
            foreach (var index in aboveIndices) probability[index] = 1;

            #endregion
            #region Pick
            for (int i = 0; i < output.Length; i++)
            {
                int bucket = state.NextInt(weights.Length);

                // Flip a weighted coin between the two possibilities in this slot
                output[i] = selector.Select(source, Float() < probability[bucket] ? bucket : alias[bucket]);
            }
            #endregion
            #region Dispose
            weights.Dispose();
            probability.Dispose();
            alias.Dispose();
            belowIndices.Dispose();
            aboveIndices.Dispose();
            #endregion
        }
        internal void SampleWeightedWithReplacementBinarySearchMethod<In, Out, Selector>(ReadOnlySpan<Weighted<In>> source, Span<Out> output) where Selector : struct, ISelector<Weighted<In>, Out>
        {
            Selector selector = default;

            // Build CumulativeWeights
            NativeList<float> cumulativeWeights = new(source.Length, Allocator.Temp);
            cumulativeWeights.ResizeUninitialized(source.Length);
            cumulativeWeights[0] = source[0].Weight;
            for (int i = 1; i < source.Length; i++)
                cumulativeWeights[i] = cumulativeWeights[i - 1] + source[i].Weight;

            // Choose
            for (int i = 0; i < output.Length; i++)
            {
                // Return Weighted Random Element
                float randVal = state.NextFloat() * cumulativeWeights[cumulativeWeights.Length - 1];
                int bottom = 0; // The current split size
                int top = cumulativeWeights.Length - 1;
                while (bottom != top)
                {
                    int mid = (bottom + top) / 2;
                    if (randVal < cumulativeWeights[mid])
                        top = mid;
                    else
                        bottom = mid + 1;
                }

                output[i] = selector.Select(source, top);
            }

            cumulativeWeights.Dispose();
        }
        internal void SampleWeightedWithoutReplacement<In, Out, Selector>(ReadOnlySpan<Weighted<In>> source, Span<Out> output) where Selector : struct, ISelector<Weighted<In>, Out>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length == 0) throw new ArgumentException("Items must be non-empty.", nameof(source));
            if (output == null) throw new ArgumentNullException(nameof(output));

            Selector selector = default;

            // Get cumulative weights and num weighted
            NativeList<float> cumulativeWeights = new(source.Length, Allocator.Temp);
            cumulativeWeights.ResizeUninitialized(source.Length);
            cumulativeWeights[0] = source[0].Weight;
            int numWeighted = source[0].Weight != 0 ? 1 : 0;
            for (int i = 1; i < source.Length; i++)
            {
                if (source[0].Weight != 0) numWeighted++;
                cumulativeWeights[i] = cumulativeWeights[i - 1] + source[i].Weight;
            }


            // Edge Case: Requested pick of more items than the number of items with non-zero weights
            if (numWeighted < output.Length)
            {
                cumulativeWeights.Dispose();
                throw new ArgumentException("Count must not exceed the number of weighted items!");
            }

            // Pick items
            for (int i = 0; i < output.Length; i++)
            {
                // Return Weighted Random Element
                float randVal = state.NextFloat() * cumulativeWeights[cumulativeWeights.Length - 1];
                int bottom = 0; // The current split size
                int top = cumulativeWeights.Length - 1;
                while (bottom != top)
                {
                    int mid = (bottom + top) / 2;
                    if (randVal < cumulativeWeights[mid])
                        top = mid;
                    else
                        bottom = mid + 1;
                }

                // Cache the picked element
                output[i] = selector.Select(source, top);

                float weightOfPicked = cumulativeWeights[top] - (top == 0 ? 0 : cumulativeWeights[top - 1]);
                cumulativeWeights[top] -= weightOfPicked; // Adjust the selected elements weight in cumulative weights
                for (int j = top + 1; j < cumulativeWeights.Length; j++) cumulativeWeights[j] -= weightOfPicked; // Apply cascading change
            }

            cumulativeWeights.Dispose();
        }
        #endregion
        #region Item Wrappers
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T Item<T>(List<T> source) => Item(source.AsReadOnlySpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T Item<T>(T[] source) => Item(source.AsReadOnlySpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T Item<T>(ReadOnlySpan<T> source) => SampleUniform<T, T, ItemSelector<T>>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T ItemWeighted<T>(List<Weighted<T>> source) => ItemWeighted(source.AsReadOnlySpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T ItemWeighted<T>(Weighted<T>[] source) => ItemWeighted(source.AsReadOnlySpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T ItemWeighted<T>(ReadOnlySpan<Weighted<T>> source) => SampleWeighted<T, T, WeightedItemSelector<T>>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWithReplacement<T>(List<T> source, int count) => ItemsWithReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWithReplacement<T>(T[] source, int count) => ItemsWithReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWithReplacement<T>(ReadOnlySpan<T> source, int count) { T[] selectedItems = new T[count]; ItemsWithReplacement(source, selectedItems); return selectedItems; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void ItemsWithReplacement<T>(ReadOnlySpan<T> source, Span<T> output) => SampleUniformWithReplacement<T,T,ItemSelector<T>>(source, output);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWithoutReplacement<T>(List<T> source, int count) => ItemsWithoutReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWithoutReplacement<T>(T[] source, int count) => ItemsWithoutReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWithoutReplacement<T>(ReadOnlySpan<T> source, int count) { T[] output = new T[count]; ItemsWithoutReplacement(source, output); return output; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void ItemsWithoutReplacement<T>(ReadOnlySpan<T> source, Span<T> output) => SampleUniformWithoutReplacement<T, T, ItemSelector<T>>(source, output);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWeightedWithReplacement<T>(List<Weighted<T>> source, int count) => ItemsWeightedWithReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWeightedWithReplacement<T>(Weighted<T>[] source, int count) => ItemsWeightedWithReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWeightedWithReplacement<T>(ReadOnlySpan<Weighted<T>> source, int count) { T[] output = new T[count]; ItemsWeightedWithReplacement(source, output); return output; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void ItemsWeightedWithReplacement<T>(ReadOnlySpan<Weighted<T>> source, Span<T> output) => SampleWeightedWithReplacement<T, T, WeightedItemSelector<T>>(source, output);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWeightedWithoutReplacement<T>(List<Weighted<T>> source, int count) => ItemsWeightedWithoutReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWeightedWithoutReplacement<T>(Weighted<T>[] source, int count) => ItemsWeightedWithoutReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public T[]  ItemsWeightedWithoutReplacement<T>(ReadOnlySpan<Weighted<T>> source, int count) { T[] output = new T[count]; ItemsWeightedWithoutReplacement(source, output); return output; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void ItemsWeightedWithoutReplacement<T>(ReadOnlySpan<Weighted<T>> source, Span<T> output) => SampleWeightedWithoutReplacement<T, T, WeightedItemSelector<T>>(source, output);
        #region Extract
        /// <include file="../Docs.xml" path="Doc/Items/Extract/ListInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ItemsExtract<T>(List<T> source, int count)
        {
            T[] output = new T[count];
            ItemsExtract(source, output);
            return output;
        }
        /// <include file="../Docs.xml" path="Doc/Items/Extract/ListInt"/>
        public void ItemsExtract<T>(List<T> source, Span<T> output)
        {   // Input Validation
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(source));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (output.Length > source.Count) throw new ArgumentException($"Cannot request more items than are in source! Requested {output.Length} items but only {source.Count} items in source!");

            for (int i = 0; i < output.Length; i++)
            {   // Rand Selection
                int randIndex = Index(source.AsReadOnlySpan());

                // Cache Result
                output[i] = source[randIndex];

                // Swap Removal
                (source[randIndex], source[^1]) = (source[^1], source[randIndex]); // Swap
                source.RemoveAt(source.Count - 1);
            }
        }
        #endregion
        #endregion
        #region Index Wrappers
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int Index<T>(List<T> source) => Index(source.AsReadOnlySpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int Index<T>(T[] source) => Index(source.AsReadOnlySpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int Index<T>(ReadOnlySpan<T> source) => SampleUniform<T, int, IndexSelector<T>>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int IndexWeighted<T>(List<Weighted<T>> source) => IndexWeighted(source.AsReadOnlySpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int IndexWeighted<T>(Weighted<T>[] source) => IndexWeighted(source.AsReadOnlySpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int IndexWeighted<T>(ReadOnlySpan<Weighted<T>> source) => SampleWeighted<T, int, WeightedIndexSelector<T>>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWithReplacement<T>(List<T> source, int count) => IndicesWithReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWithReplacement<T>(T[] source, int count) => IndicesWithReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWithReplacement<T>(ReadOnlySpan<T> source, int count) { int[] selectedIndices = new int[count]; IndicesWithReplacement(source, selectedIndices); return selectedIndices; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void  IndicesWithReplacement<T>(ReadOnlySpan<T> source, Span<int> output) => SampleUniformWithReplacement<T, int, IndexSelector<T>>(source, output);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWithoutReplacement<T>(List<T> source, int count) => IndicesWithoutReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWithoutReplacement<T>(T[] source, int count) => IndicesWithoutReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWithoutReplacement<T>(ReadOnlySpan<T> source, int count) { int[] output = new int[count]; IndicesWithoutReplacement(source, output); return output; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void  IndicesWithoutReplacement<T>(ReadOnlySpan<T> source, Span<int> output) => SampleUniformWithoutReplacement<T, int, IndexSelector<T>>(source, output);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWeightedWithReplacement<T>(List<Weighted<T>> source, int count) => IndicesWeightedWithReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWeightedWithReplacement<T>(Weighted<T>[] source, int count) => IndicesWeightedWithReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWeightedWithReplacement<T>(ReadOnlySpan<Weighted<T>> source, int count) { int[] output = new int[count]; IndicesWeightedWithReplacement(source, output); return output; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void  IndicesWeightedWithReplacement<T>(ReadOnlySpan<Weighted<T>> source, Span<int> output) => SampleWeightedWithReplacement<T, int, WeightedIndexSelector<T>>(source, output);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWeightedWithoutReplacement<T>(List<Weighted<T>> source, int count) => IndicesWeightedWithoutReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWeightedWithoutReplacement<T>(Weighted<T>[] source, int count) => IndicesWeightedWithoutReplacement(source.AsReadOnlySpan(), count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int[] IndicesWeightedWithoutReplacement<T>(ReadOnlySpan<Weighted<T>> source, int count) { int[] output = new int[count]; IndicesWeightedWithoutReplacement(source, output); return output; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void  IndicesWeightedWithoutReplacement<T>(ReadOnlySpan<Weighted<T>> source, Span<int> output) => SampleWeightedWithoutReplacement<T, int, WeightedIndexSelector<T>>(source, output);
        #endregion
        #endregion
        #region Shuffle
        /// <include file="../Docs.xml" path="Doc/Shuffle"/>
        public void Shuffle<T>(Span<T> items)
        {   // A Fisher–Yates shuffle
            for (int i = 0; i < items.Length; i++)
            {
                // Choose a random index to swap with in the remaining range
                int randIndex = state.NextInt(0, i + 1);

                // Swap
                (items[i], items[randIndex]) = (items[randIndex], items[i]);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Shuffle<T>(List<T> items) => Shuffle(items.AsSpan());
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Shuffle<T>(T[] items) => Shuffle(items.AsSpan());
        #endregion
        #endregion
    }
}