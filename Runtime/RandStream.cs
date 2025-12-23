using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace LeafRand
{
    /// <summary>
    /// Deterministic random number generator with high-level helper functions.<br></br>
    /// Helpers for random numbers, chance, sampling, direction, color, and points. <br></br>
    /// Backed by a configurable core RNG (currently System.Random).
    /// </summary>
    public class RandStream
    {
        #region Core
        /// <include file="Docs.xml" path="Doc/Items/Class"/>
        public readonly ItemsVariants Items;
        readonly System.Random rand;

        public RandStream(int seed = 0)
        {
            rand = new System.Random(seed);
            Items = new ItemsVariants(this);
        }
        #endregion
        #region Helpers
        #region Num
        /// <include file="Docs.xml" path="Doc/Num/Int"/>
        public int Num(int max) => rand.Next(max);
        /// <include file="Docs.xml" path="Doc/Num/IntInt"/>
        public int Num(int min, int max) => rand.Next(min, max);
        /// <include file="Docs.xml" path="Doc/Num/Vector2Int"/>
        public int Num(Vector2Int range) => rand.Next(range.x, range.y);
        /// <include file="Docs.xml" path="Doc/Num"/>
        public float Num() => (float)rand.NextDouble();
        /// <include file="Docs.xml" path="Doc/Num/Float"/>
        public float Num(float max) => Num() * max;
        /// <include file="Docs.xml" path="Doc/Num/FloatFloat"/>
        public float Num(float min, float max) => min + Num() * (max - min);
        /// <include file="Docs.xml" path="Doc/Num/Vector2"/>
        public float Num(Vector2 range) => Num(range.x, range.y);
        public double Num(double max) => rand.NextDouble() * max;
        /// <include file="Docs.xml" path="Doc/Angle"/>
        public float Angle() => Num(2 * Mathf.PI);
        #endregion
        #region Bool
        /// <include file="Docs.xml" path="Doc/Bool"/>
        public bool Bool(float successChance = 0.5f) => rand.NextDouble() < successChance;
        /// <include file="Docs.xml" path="Doc/Chance"/>
        public bool Chance(float successChance = 0.5f) => Bool(successChance);
        #endregion
        #region Direction
        /// <include file="Docs.xml" path="Doc/Sign"/>
        public int Sign() => Num(2) * 2 - 1;
        #region 2D
        /// <include file="Docs.xml" path="Doc/Dir2D"/>
        public Vector2 Dir2D() => Dir2D(0, 360);
        /// <include file="Docs.xml" path="Doc/Dir2D/FloatFloat"/>
        public Vector2 Dir2D(float minTheta, float maxTheta)
        {
            float thetaRadians = Num(minTheta, maxTheta) * Mathf.Deg2Rad;
            return new(Mathf.Cos(thetaRadians), Mathf.Sin(thetaRadians));
        }

        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2"/>
        public Vector2 Dir2D(Vector2 thetaRange) => Dir2D(thetaRange.x, thetaRange.y);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2FloatFloat"/>
        public Vector2 Dir2D(Vector2 basis, float minDegrees, float maxDegrees)
        {   // Normalize the basis to ensure direction is normal
            basis.Normalize();

            // Get Random Rotate in the passed range and convert to RADs
            float randomDegrees = Num(minDegrees, maxDegrees) * Mathf.Deg2Rad;

            // Calculate direction relative to basis rotating randomDegrees
            float newX = basis.x * Mathf.Cos(randomDegrees) - basis.y * Mathf.Sin(randomDegrees);
            float newY = basis.x * Mathf.Sin(randomDegrees) + basis.y * Mathf.Cos(randomDegrees);

            return new(newX, newY);
        }
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2Vector2"/>
        public Vector2 Dir2D(Vector2 basis, Vector2 degreesRange) => Dir2D(basis, degreesRange.x, degreesRange.y);

        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2Float"/>
        public Vector2 Dir2D(Vector2 basis, float withinDegrees) => Dir2D(basis, -withinDegrees, withinDegrees);
        #endregion
        #region 3D
        /// <include file="Docs.xml" path="Doc/Dir"/>
        public Vector3 Dir() => Dir(0, 360, 0, 180);
        /// <include file="Docs.xml" path="Doc/Dir/FloatFloatFloatFloat"/>
        public Vector3 Dir(float thetaRangeX, float thetaRangeY, float phiRangeX, float phiRangeY)
        {   // Get Random Theta
            float thetaRadians = Num(thetaRangeX, thetaRangeY) * Mathf.Deg2Rad;

            // Get Random Phi
            float randPhiDegrees = Num(phiRangeX, phiRangeY);
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
        public Vector3 Dir(Vector2 thetaRange, Vector2 phiRange) => Dir(thetaRange.x, thetaRange.y, phiRange.x, phiRange.y);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3FloatFloat"/>
        public Vector3 Dir(Vector3 basis, float minDegrees, float maxDegrees)
        {   // Normalize basis
            basis.Normalize();

            Vector3 chosenDirection = Dir(0, 360, minDegrees, maxDegrees);


            var randRotation = Quaternion.FromToRotation(Vector3.up, basis);


            return randRotation * chosenDirection;
        }
        /// <include file="Docs.xml" path="Doc/Dir/Vector3Vector2"/>
        public Vector3 Dir(Vector3 basis, Vector2 degreesRange) => Dir(basis, degreesRange.x, degreesRange.y);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3Float"/>
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
        public Color Color(Vector2 hueRange, Vector2 saturationRange, Vector2 valueRange) => UnityEngine.Color.HSVToRGB(Num(hueRange), Num(saturationRange), Num(valueRange));
        #endregion
        #region Item
        #region Single
        /// <include file="Docs.xml" path="Doc/Item/List"/>
        public T Item<T>(IReadOnlyList<T> items) => items[Num(items.Count)];
        /// <include file="Docs.xml" path="Doc/Item/ListList"/>
        public T Item<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights)
        {   // Prevents O(n^2) picking for Non-Indexed Enumerables
            // (Count() and ElementAt() both execute in O(n) time for many Enumerables)

            float totalWeight = weights.Sum();

            if (totalWeight == 0) return Item(items);

            // Return Weighted Random Element
            double randVal = rand.NextDouble() * totalWeight;
            float weightPosition = 0;
            for (int i = 0; i < items.Count; i++)
            {
                weightPosition += weights[i];
                if (weightPosition > randVal)
                    return items[i];
            }

            // This should be impossible!
            throw new Exception($"LeafNoise: rand weight of: {rand} > totalweight: {totalWeight}! Sorry! My fault LOL");
        }
        /// <include file="Docs.xml" path="Doc/Item/WeightedList"/>
        public T Item<T>(WeightedList<T> weightedList) => Item(weightedList.Items, weightedList.Weights);
        #endregion
        #region Multi
        public class ItemsVariants
        {
            // Setup Fluent API
            readonly RandStream rand;
            internal ItemsVariants(RandStream stream) => rand = stream;


            /// <include file="Docs.xml" path="Doc/Items/WithReplacement/ListInt"/>
            public T[] WithReplacement<T>(IReadOnlyList<T> items, int count)
            {   // Input validation
                if (items == null) throw new ArgumentNullException(nameof(items));
                if (items.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(items));
                if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be positive.");

                return ItemUniformWithReplacement(items, count);
            }
            /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/ListInt"/>
            public T[] WithoutReplacement<T>(IReadOnlyList<T> items, int count)
            {   // Input validation
                if (items == null) throw new ArgumentNullException(nameof(items));
                if (items.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(items));
                if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be positive.");
                if (count > items.Count) throw new ArgumentOutOfRangeException(nameof(count), count, $"Count must not exceed the number of items.");

                // Without Replacement
                // Determine Best Algorithm based on use resevoir threshold
                float USERESEVOIRTHRESHOLD = 0.14f;
                if ((float)count / items.Count > USERESEVOIRTHRESHOLD) return ItemUniformWithoutReplacementResevoirMethod(items, count);
                else return ItemUniformWithoutReplacementRetryMethod(items, count);
            }


            /// <include file="Docs.xml" path="Doc/Items/WithReplacement/ListListInt"/>
            public T[] WithReplacement<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count)
            {   // Input Validation
                if (items == null) throw new ArgumentNullException(nameof(items));
                if (items.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(items));
                if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be non-negative.");
                if (weights == null) throw new ArgumentNullException(nameof(weights));
                if (items.Count != weights.Count) throw new ArgumentException($"Items ({items.Count}) and Weights ({weights.Count}) must have the same length.", nameof(weights));

                // Decide on best algorithm
                if (items.Count * count < 100 || (float)items.Count / 5 > count) return ItemWeightedWithReplacementBinarySearch(items, weights, count);
                else return ItemWeightedWithReplacementAliasMethod(items, weights, count);
            }
            /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/ListListInt"/>
            public T[] WithoutReplacement<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count)
            {   // Input Validation
                if (items == null) throw new ArgumentNullException(nameof(items));
                if (items.Count == 0) throw new ArgumentException("Items must be non-empty.", nameof(items));
                if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be non-negative.");
                if (weights == null) throw new ArgumentNullException(nameof(weights));
                if (items.Count != weights.Count) throw new ArgumentException($"Items ({items.Count}) and Weights ({weights.Count}) must have the same length.", nameof(weights));
                if (count > items.Count) throw new ArgumentOutOfRangeException(nameof(count), count, $"Count must not exceed the number of items.");


                return ItemWeightedWithoutReplacement(items, weights, count);
            }


            /// <include file="Docs.xml" path="Doc/Items/WithReplacement/WeightedListInt"/>
            public T[] WithReplacement<T>(WeightedList<T> weightedList, int count) => WithReplacement(weightedList.Items, weightedList.Weights, count);
            /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/WeightedListInt"/>
            public T[] WithoutReplacement<T>(WeightedList<T> weightedList, int count) => WithoutReplacement(weightedList.Items, weightedList.Weights, count);
            #region Algorithms
            /// <summary>
            /// Picks <paramref name="count"/> uniform random items with replacement in O(n) time.
            /// </summary>
            T[] ItemUniformWithReplacement<T>(IReadOnlyList<T> items, int count)
            {
                T[] chosen = new T[count];

                for (int i = 0; i < count; i++) chosen[i] = items.ElementAt(rand.Num(items.Count));

                return chosen;
            }
            /// <summary>
            /// Picks <paramref name="count"/> uniform random items with replacement in O(n)-O(oo) time.
            /// </summary>
            T[] ItemUniformWithoutReplacementRetryMethod<T>(IReadOnlyList<T> items, int count)
            {   // Kind of weird but pretty much O(infinity) worst case and O(n) best case
                // in practice lightning fast for small pick ratios (Picking < 15% of items)

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
                    int randIndex = rand.Num(items.Count());
                    while (removed.ContainsKey(randIndex))
                        randIndex = rand.Num(items.Count());

                    removed.Add(randIndex, true);
                    chosen[i] = items[randIndex];
                }

                return chosen;
            }
            T[] ItemUniformWithoutReplacementResevoirMethod<T>(IEnumerable<T> items, int count)
            {   // An O(n) unform random sampling approach with IEnumerable items
                T[] resevior = new T[count];

                IEnumerator<T> enumerator = items.GetEnumerator();

                // Initial Resevior
                int i = 0;
                for (; i < count; i++)
                {
                    enumerator.MoveNext();
                    resevior[i] = enumerator.Current;
                }

                // Item
                while (enumerator.MoveNext())
                {
                    int random = rand.Num(++i);
                    if (random < count) resevior[random] = enumerator.Current;
                }

                return resevior;
            }
            T[] ItemWeightedWithReplacementAliasMethod<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count)
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
                if (allWeightsEqual) return ItemUniformWithReplacement(items, count);

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
                    int bucket = rand.Num(weights.Count);

                    // Flip a weighted coin between the two possibilities in this slot
                    picked[i] = items[rand.Num(1.0) < probability[bucket] ? bucket : alias[bucket]];
                }
                #endregion

                return picked;
            }
            T[] ItemWeightedWithReplacementBinarySearch<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count)
            {
                // Prevents O(n^2) picking for Non-Indexed Enumerables
                // (Count() and ElementAt() both execute in O(n) time for many Enumerables)

                // Get CumulativeWeights
                // Simultaneously determine if all weights are same
                bool allWeightsEqual = true;
                float firstWeight = weights[0];
                List<float> cumulativeWeights = new() { weights[0] };
                for (int i = 1; i < items.Count; i++)
                {
                    if (firstWeight != weights[i]) allWeightsEqual = false;
                    cumulativeWeights.Add(cumulativeWeights[i - 1] + weights[i]);
                }

                // Edge case: All weights are equal fall back to PickNonWeighted
                if (allWeightsEqual) return ItemUniformWithReplacement(items, count);

                // Choose
                T[] chosen = new T[count];
                for (int i = 0; i < count; i++)
                {
                    // Return Weighted Random Element
                    double randVal = rand.Num(1.0) * cumulativeWeights[cumulativeWeights.Count - 1];
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

                    chosen[i] = items[top];
                }


                return chosen;
            }
            /// <summary>
            /// Picks <paramref name="count"/> items from the items list without replacement<br></br>
            /// - Executes in O(nlog(n)) time<br></br>
            /// </summary>
            /// <returns>
            /// An array containing the results.
            /// </returns>
            T[] ItemWeightedWithoutReplacement<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count)
            {   // Prep vars to detect edge cases
                bool allWeightsEqual = true;
                float firstWeight = weights[0];
                int numNonZeroWeights = weights[0] != 0 ? 1 : 0;

                // Get CumulativeWeights
                List<float> cumulativeWeights = new() { weights[0] };
                for (int i = 1; i < items.Count; i++)
                {
                    if (weights[i] != 0) numNonZeroWeights++;
                    if (weights[i] != firstWeight) allWeightsEqual = false;
                    cumulativeWeights.Add(cumulativeWeights[i - 1] + weights[i]);
                }

                // Edge case: All weights are equal fall back to PickNonWeighted
                if (allWeightsEqual) return WithoutReplacement(items, count);

                // Edge Case: Requested pick of more items than the number of items with non-zero weights
                // - All weighted items should be returned
                // - Then perform non-weighted random picks for the remaining items
                // (This is faster + this method breaks if there are no remaining weighted items. It would always pick the first item)
                T[] picked = new T[count];
                if (numNonZeroWeights < count)
                {
                    int pickIndex = 0;
                    int zeroWeightIndex = 0;
                    T[] nonWeightedItems = new T[items.Count - numNonZeroWeights];
                    // Grab weighted items into picked and non-weighted items into nonWeightedItems
                    for (int i = 0; i < items.Count; i++)
                        if (weights[i] == 0) nonWeightedItems[zeroWeightIndex++] = items[i];
                        else picked[pickIndex++] = items[i];

                    // Than grab remaining items randomly from non weighted items
                    T[] nonWeightedPicks = WithoutReplacement(nonWeightedItems, count);

                    // Concat nonWeightedPicks with picked
                    int remaining = count - numNonZeroWeights;
                    for (int i = 0; i < remaining; i++)
                        picked[pickIndex++] = nonWeightedPicks[i];

                    return picked;
                }

                // Pick items
                for (int i = 0; i < count; i++)
                {
                    // Return Weighted Random Element
                    double randVal = rand.Num(1.0) * cumulativeWeights[cumulativeWeights.Count - 1];
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
                    picked[i] = items[top];

                    float weightOfPicked = cumulativeWeights[top] - (top == 0 ? 0 : cumulativeWeights[top - 1]);
                    cumulativeWeights[top] = 0; // Zero picked weight
                    for (int j = top + 1; j < cumulativeWeights.Count; j++) cumulativeWeights[j] -= weightOfPicked; // Apply weight change to cumulative weights
                }

                return picked;
            }
            /* Need to make a binary tree to get this working will do it later
            public T[] PickWeightedWithoutReplacementAExpJ<T>(IEnumerable<Weighted<T>> items, int count)
            {   // The A-ExpJ Resevoir Sampling method as described at: https://en.wikipedia.org/wiki/Reservoir_sampling
                IEnumerator<Weighted<T>> iter = items.GetEnumerator();
                SortedDictionary<float, T> H = new();

                // Fill the initial reservoir
                // just grabbing first Count elements into H
                while (H.Count < count && iter.MoveNext())
                {
                    float r = MathF.Pow((float)rand.NextDouble(), 1f / iter.Current.Weight);
                    H.Add(r, iter.Current.Element);
                }

                // Process to end of iterator
                // (Do some Black Magic)
                float X = MathF.Log((float)rand.NextDouble()) / MathF.Log(H.First().Key);
                while (iter.MoveNext())
                {
                    X -= iter.Current.Weight;
                    if (X <= 0)
                    {
                        float t = MathF.Pow(H.First().Key, iter.Current.Weight);
                        float r = MathF.Pow((float)rand.NextDouble() * (1f - t) + t, 1f / iter.Current.Weight);

                        H.Remove(H.First().Key);
                        H[r] = iter.Current.Element;

                        X = MathF.Log((float)rand.NextDouble()) / MathF.Log(H.First().Key);
                    }
                }


                T[] picked = new T[count];
                int i = 0;
                foreach (var pair in H) picked[i++] = pair.Value;

                return picked;
            }*/
            #endregion
        }
        #endregion
        #endregion
        #region Index
        /// <include file="Docs.xml" path="Doc/Index"/>
        public int Index<T>(IReadOnlyCollection<T> items) => Num(items.Count);
        #endregion
        #endregion
    }
}