using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LeafRand
{
    /// <summary>
    /// Burst-compatible RNG struct with high-level helper methods.<br></br>
    /// Faster than Global and Managed Rand. State is a single uint.
    /// </summary>    
    public struct BurstRand
    {
        #region Core
        /// <include file="Docs.xml" path="Doc/Items/Class"/>
        Unity.Mathematics.Random state;

        /// <include file="Docs.xml" path="Doc/Seed"/>
        public BurstRand(uint seed = 1) => state = new(seed);

        /// <include file="Docs.xml" path="Doc/Seed"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSeed(uint seed) => state.InitState(seed);
        #endregion
        #region Helpers
        #region Num
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint UInt() => state.NextUInt();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint UInt(uint max) => state.NextUInt(max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint UInt(uint min, uint max) => state.NextUInt(min, max);

        /// <include file="Docs.xml" path="Doc/Num/Int"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Int() => state.NextInt();
        /// <include file="Docs.xml" path="Doc/Num/Int"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Int(int max) => state.NextInt(max);
        /// <include file="Docs.xml" path="Doc/Num/IntInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Int(int min, int max) => state.NextInt(min, max);
        /// <include file="Docs.xml" path="Doc/Num/Vector2Int"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Int(Vector2Int range) => state.NextInt(range.x, range.y);

        /// <include file="Docs.xml" path="Doc/Num"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Float() => state.NextFloat();
        /// <include file="Docs.xml" path="Doc/Num/Float"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Float(float max) => state.NextFloat(max);
        /// <include file="Docs.xml" path="Doc/Num/FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Float(float min, float max) => state.NextFloat(min, max);
        /// <include file="Docs.xml" path="Doc/Num/Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Float(Vector2 range) => state.NextFloat(range.x, range.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Double() => state.NextDouble();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Double(double max) => state.NextDouble(max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Double(double min, double max) => state.NextDouble(min, max);
        /// <include file="Docs.xml" path="Doc/Angle"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Angle() => state.NextFloat(2 * Mathf.PI);
        #endregion
        #region Bool
        /// <include file="Docs.xml" path="Doc/Bool"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Bool() => state.NextBool();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool BoolFloat(float chance) => state.NextFloat() < chance;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool BoolDouble(double chance) => state.NextDouble() < chance;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool BoolUInt(float chance) => state.NextUInt() < (uint)(chance * uint.MaxValue);


        /// <include file="Docs.xml" path="Doc/Bool"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Bool(float successChance) => state.NextDouble() < successChance;
        /// <include file="Docs.xml" path="Doc/Chance"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Chance(float successChance = 0.5f) => Bool(successChance);
        #endregion
        #region Direction
        /// <include file="Docs.xml" path="Doc/Sign"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sign() => state.NextInt(2) * 2 - 1;
        #region 2D
        /// <include file="Docs.xml" path="Doc/Dir2D"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Dir2D() => Dir2D(0, 360);
        /// <include file="Docs.xml" path="Doc/Dir2D/FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Dir2D(float minTheta, float maxTheta)
        {
            float thetaRadians = state.NextFloat(minTheta, maxTheta) * Mathf.Deg2Rad;
            return new(Mathf.Cos(thetaRadians), Mathf.Sin(thetaRadians));
        }

        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Dir2D(Vector2 thetaRange) => Dir2D(thetaRange.x, thetaRange.y);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2FloatFloat"/>
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
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Dir2D(Vector2 basis, Vector2 degreesRange) => Dir2D(basis, degreesRange.x, degreesRange.y);

        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2Float"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Dir2D(Vector2 basis, float withinDegrees) => Dir2D(basis, -withinDegrees, withinDegrees);
        #endregion
        #region 3D
        /// <include file="Docs.xml" path="Doc/Dir"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Dir() => Dir(0, 360, 0, 180);
        /// <include file="Docs.xml" path="Doc/Dir/FloatFloatFloatFloat"/>
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

        /// <include file="Docs.xml" path="Doc/Dir/Vector2Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Dir(Vector2 thetaRange, Vector2 phiRange) => Dir(thetaRange.x, thetaRange.y, phiRange.x, phiRange.y);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Dir(Vector3 basis, float minDegrees, float maxDegrees)
        {   // Normalize basis
            basis.Normalize();

            Vector3 chosenDirection = Dir(0, 360, minDegrees, maxDegrees);


            var randRotation = Quaternion.FromToRotation(Vector3.up, basis);


            return randRotation * chosenDirection;
        }
        /// <include file="Docs.xml" path="Doc/Dir/Vector3Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Dir(Vector3 basis, Vector2 degreesRange) => Dir(basis, degreesRange.x, degreesRange.y);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3Float"/>
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
        /// <include file="Docs.xml" path="Doc/Color"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Color(Vector2 hueRange, Vector2 saturationRange, Vector2 valueRange) => UnityEngine.Color.HSVToRGB(state.NextFloat(hueRange.x, hueRange.y), state.NextFloat(saturationRange.x, saturationRange.y), state.NextFloat(valueRange.x, valueRange.y));
        #endregion
        #region Item
        #region Single
        /// <include file="Docs.xml" path="Doc/Item/List"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Item<T>(IReadOnlyList<T> source) => source[state.NextInt(source.Count)];
        /// <include file="Docs.xml" path="Doc/Item/ListList"/>
        public T Item<T>(IReadOnlyList<T> source, IReadOnlyList<float> weights)
        {   // Prevents O(n^2) picking for Non-Indexed Enumerables
            // (Count() and ElementAt() both execute in O(n) time for many Enumerables)

            float totalWeight = weights.Sum();

            if (totalWeight == 0) return Item(source);

            // Return Weighted Random Element
            double randVal = state.NextDouble() * totalWeight;
            float weightPosition = 0;
            for (int i = 0; i < source.Count; i++)
            {
                weightPosition += weights[i];
                if (weightPosition > randVal)
                    return source[i];
            }

            // This should be impossible!
            throw new Exception($"LeafNoise: state weight of: {state} > totalweight: {totalWeight}! Sorry! My fault LOL");
        }
        /// <include file="Docs.xml" path="Doc/Item/WeightedList"/>
        public T Item<T>(WeightedList<T> weightedList) => Item(weightedList.Items, weightedList.Weights);
        #endregion
        #region Multi
        /// <include file="Docs.xml" path="Doc/Items/WithReplacement/ListInt"/>
        public T[] ItemsWithReplacement<T>(IReadOnlyList<T> items, int count)
        {   // Input validation
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (items.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(items));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be positive.");

            return ItemsUniformWithReplacement(items, count);
        }
        /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/ListInt"/>
        public T[] ItemsWithoutReplacement<T>(IReadOnlyList<T> items, int count)
        {   // Input validation
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (items.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(items));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be positive.");
            if (count > items.Count) throw new ArgumentOutOfRangeException(nameof(count), count, $"Count must not exceed the number of items.");

            // Without Replacement
            // Determine Best Algorithm based on use reservoir threshold
            float USERESERVOIRTHRESHOLD = 0.14f;
            if ((float)count / items.Count > USERESERVOIRTHRESHOLD) return ItemsUniformWithoutReplacementReservoirMethod(items, count);
            else return ItemsUniformWithoutReplacementRetryMethod(items, count);
        }

        /// <include file="Docs.xml" path="Doc/Items/WithReplacement/ListListInt"/>
        public T[] ItemsWithReplacement<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count)
        {   // Input Validation
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (items.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(items));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be non-negative.");
            if (weights == null) throw new ArgumentNullException(nameof(weights));
            if (items.Count != weights.Count) throw new ArgumentException($"Items ({items.Count}) and Weights ({weights.Count}) must have the same length.", nameof(weights));

            // Decide on best algorithm
            if (items.Count * count < 100 || (float)items.Count / 5 > count) return ItemsWeightedWithReplacementBinarySearch(items, weights, count);
            else return ItemsWeightedWithReplacementAliasMethod(items, weights, count);
        }
        /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/ListListInt"/>
        public T[] ItemsWithoutReplacement<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count)
        {   // Input Validation
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (items.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(items));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be non-negative.");
            if (weights == null) throw new ArgumentNullException(nameof(weights));
            if (items.Count != weights.Count) throw new ArgumentException($"Items ({items.Count}) and Weights ({weights.Count}) must have the same length.", nameof(weights));
            if (count > items.Count) throw new ArgumentOutOfRangeException(nameof(count), count, $"Count must not exceed the number of items.");


            return ItemsWeightedWithoutReplacementBinarySearch(items, weights, count);
        }
        /// <include file="Docs.xml" path="Doc/Items/WithReplacement/WeightedListInt"/>
        public T[] ItemsWithReplacement<T>(WeightedList<T> weightedList, int count) => ItemsWithReplacement(weightedList.Items, weightedList.Weights, count);
        /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/WeightedListInt"/>
        public T[] ItemsWithoutReplacement<T>(WeightedList<T> weightedList, int count) => ItemsWithoutReplacement(weightedList.Items, weightedList.Weights, count);
        /// <include file="Docs.xml" path="Doc/Items/Extract/ListInt"/>
        public T[] ItemsExtract<T>(List<T> source, int count)
        {   // Input Validation
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be non-negative.");
            if (count > source.Count) throw new ArgumentOutOfRangeException(nameof(count), count, $"Count must not exceed the number of items.");

            return ItemsUniformWithoutReplacementSwapRemoved(source, count);
        }
        #region Algorithms
        /// <summary>
        /// Picks <paramref name="count"/> items from <paramref name="source"/> using uniform random sampling with replacement.<br/>
        /// Time Complexity: O(k)
        /// </summary>
        internal T[] ItemsUniformWithReplacement<T>(IReadOnlyList<T> source, int count)
        {
            T[] chosen = new T[count];

            for (int i = 0; i < count; i++) chosen[i] = source.ElementAt(state.NextInt(source.Count));

            return chosen;
        }
        /// <summary>
        /// Picks <paramref name="count"/> items from <paramref name="source"/> using uniform random sampling without replacement.<br/>
        /// Time Complexity: O(k)-O(oo)
        /// </summary>
        internal T[] ItemsUniformWithoutReplacementRetryMethod<T>(IReadOnlyList<T> source, int count)
        {   // Kind of weird but pretty much O(infinity) worst case and O(k) best case
            // in practice lightning fast for small pick ratios (Picking < 15% of items)
            // relative to somthing like reservoir sampling which will still take O(n) when k is small

            // Prep
            // Avoid copy if possible
            T[] chosen = new T[count];

            // Pre Hash to count Capacity
            // We know final map size so no need to Rehash on the fly
            Dictionary<int, bool> removed = new(count);

            // Pick items
            for (int i = 0; i < count; i++)
            {
                // Keep Picking Until Distinct Index found
                // This can be very slow if Count is close to itemsLength
                int randIndex = state.NextInt(source.Count());
                while (removed.ContainsKey(randIndex))
                    randIndex = state.NextInt(source.Count());

                removed.Add(randIndex, true);
                chosen[i] = source[randIndex];
            }

            return chosen;
        }
        /// <summary>
        /// Picks <paramref name="count"/> items from <paramref name="source"/> using uniform random sampling without replacement.<br/>
        /// Time Complexity: O(n)
        /// </summary>
        internal T[] ItemsUniformWithoutReplacementReservoirMethod<T>(IReadOnlyList<T> source, int count)
        {
            T[] reservoir = new T[count];

            // Initial Resevior
            int i = 0;
            for (; i < count; i++)
            {
                reservoir[i] = source[i];
            }

            // Roll for each item
            for (; i < source.Count; i++)
            {
                int random = state.NextInt(i + 1);
                if (random < count) reservoir[random] = source[i];
            }

            return reservoir;
        }
        /// <summary>
        /// Picks <paramref name="count"/> items from <paramref name="source"/> using uniform random sampling without replacement.<br/>
        /// Time Complexity: O(n + k)
        /// </summary>
        internal T[] ItemsUniformWithoutReplacementSwapRemovedOnCopy<T>(ICollection<T> source, int count) => ItemsUniformWithoutReplacementSwapRemoved(source.ToList(), count);
        /// <summary>
        /// Picks <paramref name="count"/> items from <paramref name="source"/> using uniform random sampling without replacement.<br/>
        /// Note:  Picked items are removed and relative order is destroyed.<br/>
        /// Time Complexity: O(k)
        /// </summary>
        internal T[] ItemsUniformWithoutReplacementSwapRemoved<T>(List<T> source, int count)
        {
            T[] results = new T[count];

            for (int i = 0; i < count; i++)
            {   // Rand Selection
                int randIndex = Index(source);

                // Cache Result
                results[i] = source[randIndex];

                // Swap Removal
                (source[i], source[source.Count - 1]) = (source[source.Count - 1], source[i]);// Swap
                source.RemoveAt(source.Count - 1);
            }

            return results;
        }
        /// <summary>
        /// Picks <paramref name="count"/> items from <paramref name="source"/> using weighted random sampling with replacement.<br/>
        /// Time Complexity: O(n)
        /// </summary>
        internal T[] ItemsWeightedWithReplacementAliasMethod<T>(IReadOnlyList<T> source, IReadOnlyList<float> weights, int count)
        {   // Uses Vose's Alias method.
            // A lovely method for effecient generation of weighted random values
            // Time Complexity: O(n) setup and O(1) picks

            // For edge case where all weights are equal
            bool allWeightsEqual = true;
            float firstWeight = weights[0];

            #region Setup
            // Initialize work Arrays and Array Copy
            var weightsCopy = weights.ToArray();
            float[] probability = new float[weights.Count];
            int[] alias = new int[weights.Count];


            // Split all Probabilities into 
            // stacks above or below average
            Stack<int> belowIndices = new();
            Stack<int> aboveIndices = new();
            float avg = weights.Sum() / weights.Count; // Calculate Average
            for (int i = 0; i < weights.Count; i++)
            {   // While splitting weights see if all are equal to determine if this work may be skipped
                if (weightsCopy[i] != firstWeight) allWeightsEqual = false;

                if (weightsCopy[i] >= avg) aboveIndices.Push(i);
                else belowIndices.Push(i);


            }

            // Edge case: All weights are equal fall back to PickNonWeighted
            if (allWeightsEqual) return ItemsUniformWithReplacement(source, count);

            // Grab one from less and one from more
            while (belowIndices.Count != 0 && aboveIndices.Count != 0)
            {
                int belowIndex = belowIndices.Pop();
                int aboveIndex = aboveIndices.Pop();

                // Scale Probabilities
                probability[belowIndex] = weightsCopy[belowIndex] / avg;
                alias[belowIndex] = aboveIndex;

                // Reduce probability of above by probability of below
                weightsCopy[aboveIndex] += weightsCopy[belowIndex] - avg;

                // Place above back into aboveAverage or belowAverage bucket based on it's new weight
                if (weightsCopy[aboveIndex] >= avg) aboveIndices.Push(aboveIndex);
                else belowIndices.Push(aboveIndex);
            }


            // Empty remaining elements from bbucket
            // Due to floating point imprecision we cant be sure which bucket will have elements remaining
            // (Although mathematically it should always be the above average bucket)
            foreach (var index in belowIndices) probability[index] = 1;
            foreach (var index in aboveIndices) probability[index] = 1;

            #endregion
            #region Pick
            T[] picked = new T[count];

            for (int i = 0; i < count; i++)
            {
                int bucket = state.NextInt(weights.Count);

                // Flip a weighted coin between the two possibilities in this slot
                picked[i] = source[Double() < probability[bucket] ? bucket : alias[bucket]];
            }
            #endregion

            return picked;
        }
        /// <summary>
        /// Picks <paramref name="count"/> items from <paramref name="source"/> using weighted random sampling with replacement.<br/>
        /// Time Complexity: O(n + klog(n))
        /// </summary>
        internal T[] ItemsWeightedWithReplacementBinarySearch<T>(IReadOnlyList<T> source, IReadOnlyList<float> weights, int count)
        {   // Get CumulativeWeights
            // Simultaneously determine if all weights are same
            bool allWeightsEqual = true;
            float firstWeight = weights[0];
            List<float> cumulativeWeights = new() { weights[0] };
            for (int i = 1; i < source.Count; i++)
            {
                if (firstWeight != weights[i]) allWeightsEqual = false;
                cumulativeWeights.Add(cumulativeWeights[i - 1] + weights[i]);
            }

            // Edge case: All weights are equal fall back to PickNonWeighted
            if (allWeightsEqual) return ItemsUniformWithReplacement(source, count);

            // Choose
            T[] chosen = new T[count];
            for (int i = 0; i < count; i++)
            {
                // Return Weighted Random Element
                double randVal = Double() * cumulativeWeights[cumulativeWeights.Count - 1];
                int bottom = 0; // The current split size
                int top = cumulativeWeights.Count - 1;
                while (bottom != top)
                {
                    int mid = (bottom + top) / 2;
                    if (randVal < cumulativeWeights[mid])
                        top = mid;
                    else if (randVal >= cumulativeWeights[mid])
                        bottom = mid + 1;
                }

                chosen[i] = source[top];
            }


            return chosen;
        }
        /// <summary>
        /// Picks <paramref name="count"/> items from <paramref name="source"/> using weighted random sampling without replacement.<br/>
        /// Time Complexity: O(n + klog(n) + nk)
        /// </summary>
        internal T[] ItemsWeightedWithoutReplacementBinarySearch<T>(IReadOnlyList<T> source, IReadOnlyList<float> weights, int count)
        {   // Prep vars to detect edge cases
            bool allWeightsEqual = true;
            float firstWeight = weights[0];
            int numWeighted = weights[0] != 0 ? 1 : 0;
            
            // Get CumulativeWeights
            List<float> cumulativeWeights = new() { weights[0] };
            for (int i = 1; i < source.Count; i++)
            {
                if (weights[i] != 0) numWeighted++;
                if (weights[i] != firstWeight) allWeightsEqual = false;
                cumulativeWeights.Add(cumulativeWeights[i - 1] + weights[i]);
            }

            // Edge case: All weights are equal fall back to PickNonWeighted
            if (allWeightsEqual) return ItemsWithoutReplacement(source, count);

            // Edge Case: Requested pick of more items than the number of items with non-zero weights
            // - All weighted items should be returned
            // - Then perform non-weighted random picks for the remaining items
            // (This is faster + this method breaks if there are no remaining weighted items. It would always pick the first item)
            T[] picked = new T[count];
            if (numWeighted < count)
            {
                int pickIndex = 0;
                int zeroWeightIndex = 0;
                T[] nonWeightedItems = new T[source.Count - numWeighted];
                // Grab weighted items into picked and non-weighted items into nonWeightedItems
                for (int i = 0; i < source.Count; i++)
                    if (weights[i] == 0) nonWeightedItems[zeroWeightIndex++] = source[i];
                    else picked[pickIndex++] = source[i];

                // Than grab remaining items randomly from non weighted items
                int remaining = count - numWeighted;
                T[] nonWeightedPicks = ItemsWithoutReplacement(nonWeightedItems, remaining);

                // Concat nonWeightedPicks with picked
                for (int i = 0; i < remaining; i++)
                    picked[pickIndex++] = nonWeightedPicks[i];

                return picked;
            }

            // Pick items
            for (int i = 0; i < count; i++)
            {
                // Return Weighted Random Element
                double randVal = Double() * cumulativeWeights[cumulativeWeights.Count - 1];
                int bottom = 0; // The current split size
                int top = cumulativeWeights.Count - 1;
                while (bottom != top)
                {
                    int mid = (bottom + top) / 2;
                    if (randVal < cumulativeWeights[mid])
                        top = mid;
                    else if (randVal >= cumulativeWeights[mid])
                        bottom = mid + 1;
                }

                // Cache the picked element
                picked[i] = source[top];

                float weightOfPicked = cumulativeWeights[top] - (top == 0 ? 0 : cumulativeWeights[top - 1]);
                cumulativeWeights[top] = 0; // Zero picked weight
                for (int j = top + 1; j < cumulativeWeights.Count; j++) cumulativeWeights[j] -= weightOfPicked; // Apply weight change to cumulative weights
            }

            return picked;
        }
        /* Need to make a binary tree to get this working will do it later also try
        public T[] ItemsWeightedWithoutReplacementAExpJ<T>(IEnumerable<Weighted<T>> items, int count)
        {   // The A-ExpJ Reservoir Sampling method as described at: https://en.wikipedia.org/wiki/Reservoir_sampling
            IEnumerator<Weighted<T>> iter = items.GetEnumerator();
            SortedDictionary<float, T> H = new();

            // Fill the initial reservoir
            // just grabbing first Count elements into H
            while (H.Count < count && iter.MoveNext())
            {
                float r = MathF.Pow((float)state.NextDouble(), 1f / iter.Current.Weight);
                H.Add(r, iter.Current.Element);
            }

            // Process to end of iterator
            // (Do some Black Magic)
            float X = MathF.Log((float)state.NextDouble()) / MathF.Log(H.First().Key);
            while (iter.MoveNext())
            {
                X -= iter.Current.Weight;
                if (X <= 0)
                {
                    float t = MathF.Pow(H.First().Key, iter.Current.Weight);
                    float r = MathF.Pow((float)state.NextDouble() * (1f - t) + t, 1f / iter.Current.Weight);

                    H.Remove(H.First().Key);
                    H[r] = iter.Current.Element;

                    X = MathF.Log((float)state.NextDouble()) / MathF.Log(H.First().Key);
                }
            }


            T[] picked = new T[count];
            int i = 0;
            foreach (var pair in H) picked[i++] = pair.Value;

            return picked;
        }*/
        #endregion
        #endregion
        #endregion
        #region Index
        /// <include file="Docs.xml" path="Doc/Index"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Index<T>(IReadOnlyCollection<T> items) => state.NextInt(items.Count);
        #endregion
        #region Shuffle
        /// <include file="Docs.xml" path="Doc/Shuffle"/>
        public void Shuffle<T>(IList<T> toShuffle)
        {   // A Fisher–Yates shuffle
            for (int i = toShuffle.Count - 1; i > 1; i--)
            {
                // Choose a random index to swap with in the remaining range
                int randIndex = state.NextInt(0, i + 1);

                // Swap
                (toShuffle[i], toShuffle[randIndex]) = (toShuffle[i], toShuffle[randIndex]);
            }
        }
        #endregion
        #endregion
    }
}