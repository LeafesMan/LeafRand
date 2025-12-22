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
        readonly System.Random rand;
        public RandStream(int seed = 0) => rand = new System.Random(seed);
        #endregion
        #region Helpers
        #region Num
        /// <summary>
        /// Returns a random integer in: [0, max)
        /// </summary>
        public int Num(int max) => rand.Next(max);
        /// <summary>
        /// Returns a random integer in: [min, max)
        /// </summary>
        public int Num(int min, int max) => rand.Next(min, max);
        /// <summary>
        /// Returns a random integer in: [range.x, range.y)
        /// </summary>
        public int Num(Vector2Int range) => rand.Next(range.x, range.y);
        /// <summary>
        /// Returns a random float in: [0, 1)
        /// </summary>
        public float Num() => (float)rand.NextDouble();
        /// <summary>
        /// Returns a random float in: [0, max)
        /// </summary>
        public float Num(float max) => Num() * max;
        /// <summary>
        /// Returns a random float in: [min, max)
        /// </summary>
        public float Num(float min, float max) => min + Num() * (max - min);
        /// <summary>
        /// Returns a random float in: [range.x, range.y]
        /// </summary>
        public float Num(Vector2 range) => Num(range.x, range.y);
        /// <summary>
        /// Returns a random index within the size of Source.
        /// </summary>
        public int Index<T>(IReadOnlyCollection<T> source) => Num(source.Count);
        #endregion
        #region Angle
        /// <summary>
        /// Returns a random angle in: [minRadians, maxRadians)
        /// </summary>
        public float Angle(float minRadians = 0, float maxRadians = 2 * Mathf.PI) { return Num(minRadians, maxRadians);  }
        #endregion
        #region Bool
        ///<summary>
        /// Returns a random bool with SuccessChance of returning true.
        ///</summary>
        public bool Bool(float successChance = 0.5f) => rand.NextDouble() < successChance;
        #endregion
        #region Direction
        public int Sign() => Num(2) * 2 - 1;
        #region 2D
        /// <summary>
        /// Returns a random 2D direction.
        /// </summary>
        public Vector2 Dir2D() => Dir2D(0, 360);
        /// <summary>
        /// Returns random 2D direction within the given theta range.
        /// </summary>
        public Vector2 Dir2D(float thetaRangeX, float thetaRangeY)
        {
            float thetaRadians = Num(thetaRangeX, thetaRangeY) * Mathf.Deg2Rad;
            return new(Mathf.Cos(thetaRadians), Mathf.Sin(thetaRadians));
        }
        /// <summary>
        /// Returns random 2D direction within the given theta range.
        /// </summary>
        public Vector2 Dir2D(Vector2 thetaRange) => Dir2D(thetaRange.x, thetaRange.y);
        /// <summary>
        /// Returns the direction of the basis rotated by a random value within degreesRange.
        /// </summary>
        public Vector2 Dir2D(Vector2 basis, float degreesRangeX, float degreesRangeY)
        {   // Normalize the basis to ensure direction is normal
            basis.Normalize();

            // Get Random Rotate in the passed range and convert to RADs
            float randomDegrees = Num(degreesRangeX, degreesRangeY) * Mathf.Deg2Rad;

            // Calculate direction relative to basis rotating randomDegrees
            float newX = basis.x * Mathf.Cos(randomDegrees) - basis.y * Mathf.Sin(randomDegrees);
            float newY = basis.x * Mathf.Sin(randomDegrees) + basis.y * Mathf.Cos(randomDegrees);

            return new(newX, newY);
        }
        /// <summary>
        /// Returns the direction of the basis rotated by a random value within degreesRange.
        /// </summary>
        public Vector2 Dir2D(Vector2 basis, Vector2 degreesRange) => Dir2D(basis, degreesRange.x, degreesRange.y);
        /// <summary>
        /// Returns a new direction within degrees of the basis.
        /// </summary>
        public Vector2 Dir2D(Vector2 basis, float withinDegrees) => Dir2D(basis, -withinDegrees, withinDegrees);
        #endregion
        #region 3D
        /// <summary>
        /// Returns random 3D direction.
        /// </summary>
        public Vector3 Dir3D() => Dir3D(0, 360, 0, 180);
        /// <summary>
        /// Returns random 3D direction within the given theta and phi ranges.<br></br>
        /// Takes Theta and Phi in degrees. <br></br>
        /// Theta takes values (0,360) values outside this range will contine to rotate CC facing down at the XZ plane.<br></br>
        /// Phi takes values (0,180) values above will snap back to 0 and below will snap back to 180.<br></br>
        /// </summary>
        public Vector3 Dir3D(float thetaRangeX, float thetaRangeY, float phiRangeX, float phiRangeY)
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
        /// <summary>
        /// Returns random 2D direction given a theta and phi range.
        /// </summary>
        public Vector3 Dir3D(Vector2 thetaRange, Vector2 phiRange) => Dir3D(thetaRange.x, thetaRange.y, phiRange.x, phiRange.y);
        /// <summary>
        /// Returns the direction of the basis rotated by a random angles within the degreesranges.
        /// </summary>
        public Vector3 Dir3D(Vector3 forward, float degreesRangeX, float degreesRangeY)
        {   // Normalize basis
            forward.Normalize();

            Vector3 chosenDirection = Dir3D(0, 360, degreesRangeX, degreesRangeY);


            var randRotation = Quaternion.FromToRotation(Vector3.up, forward);


            return randRotation * chosenDirection;
        }
        /// <summary>
        /// Returns the direction of the basis rotated by a random angles within the degreesranges.
        /// </summary>
        public Vector3 Dir3D(Vector3 forward, Vector2 degreesRange) => Dir3D(forward, degreesRange.x, degreesRange.y);
        /// <summary>
        /// Returns a direction within degrees of the basis.
        /// </summary>
        public Vector3 Dir3D(Vector3 forward, float withinDegrees)
        {
            Vector3 withinDegreesOfUp = Dir3D(0, 360, 0, withinDegrees);

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, withinDegreesOfUp);

            return rotation * forward;
        }
        #endregion
        #endregion
        #region Color

        /// <summary>
        /// Takes a Hue, Saturation, and Value range all within [0,1] (Inclusive).
        /// </summary>
        /// <returns>A random Color with HSV within the given ranges.</returns>
        public Color Color(Vector2 hueRange, Vector2 saturationRange, Vector2 valueRange) => UnityEngine.Color.HSVToRGB(Num(hueRange), Num(saturationRange), Num(valueRange));
        #endregion
        #region Item
        #region Single
        /// <summary>
        /// Takes a single item uniformly random Item from <paramref name="source"/>.<br></br>
        /// * If you need multiple picks DO NOT loop this mehtod<br></br>
        /// </summary>
        public T Item<T>(IReadOnlyList<T> source) => source[Num(source.Count)];
        public T Item<T>(IReadOnlyList<T> source, IReadOnlyList<float> weights)
        {   // Prevents O(n^2) picking for Non-Indexed Enumerables
            // (Count() and ElementAt() both execute in O(n) time for many Enumerables)

            float totalWeight = weights.Sum();

            if (totalWeight == 0) return Item(source);

            // Return Weighted Random Element
            double randVal = rand.NextDouble() * totalWeight;
            float weightPosition = 0;
            for (int i = 0; i < source.Count; i++)
            {
                weightPosition += weights[i];
                if (weightPosition > randVal)
                    return source[i];
            }

            // This should be impossible!
            throw new Exception($"LeafNoise: rand weight of: {rand} > totalweight: {totalWeight}! Sorry! My fault LOL");
        }
        public T Item<T>(WeightedList<T> weightedList) => Item(weightedList.Items, weightedList.Weights);
        #endregion
        #region Multi
        /// <summary>
        /// Picks <paramref name="k"/> uniformly random items from <paramref name="items"/> .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">List of items to pick from.<br></br></param>
        /// <param name="k">How many items should be picked.<br></br>
        /// * values under 1 result in the return of an empty array<br></br></param>
        /// <param name="withReplacement">Whether the same item may be picked multiple times.</param>
        /// <returns> An array containing the chosen items </returns>
        public T[] Item<T>(IReadOnlyList<T> items, int k, bool withReplacement = true)
        {   // Input validation
            if (items == null) throw new ArgumentNullException("Pick failed. Items is null!");
            if (items.Count == 0) throw new ArgumentException("Pick failed. Items is empty!");
            if (k < 0) throw new ArgumentException($"Pick failed. Requested {k} picks! Pass k >= 0.");
            if (!withReplacement && items.Count < k) throw new ArgumentNullException($"Pick failed. Requested more items ({k}) than are available({items.Count}!");

            // With Replacement
            if (withReplacement) return ItemUniformWithReplacement(items, k);

            // Without Replacement
            // Determine Best Algorithm based on use resivoir threshold
            float USERESEVOIRTHRESHOLD = 0.14f;
            if ((float)k / items.Count > USERESEVOIRTHRESHOLD) return ItemUniformWithoutReplacementResevoirMethod(items, k);
            else return ItemUniformWithoutReplacementRetryMethod(items, k);
        }
        /// <summary>
        /// Picks <paramref name="k"/> items from <paramref name="items"/> with <paramref name="weights"/>.
        /// </summary>
        /// <param name="items">List of items to pick from.<br></br></param>
        /// <param name="weights">Weights corresponding to the items by index.<br></br></param>
        /// <param name="k">How many items should be picked.<br></br>
        /// <param name="withReplacement">Whether the same item may be picked multiple times.</param>
        /// <returns> An array containing the chosen items </returns>
        public T[] Item<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int k, bool withReplacement = true)
        {   // Input Validation
            if (items == null) throw new ArgumentNullException("Pick failed. Items is null!");
            if (items.Count == 0) throw new ArgumentException("Pick failed. Items is empty!");
            if (k < 0) throw new ArgumentException($"Pick failed. Requested {k} picks! Pass k >= 0.");
            if (!withReplacement && items.Count < k) throw new ArgumentException($"Pick failed. Requested more items ({k}) than are available({items.Count}!");
            if (weights == null) throw new ArgumentNullException("Pick failed. Weights is null!");
            if (items.Count != weights.Count) throw new ArgumentException($"Pick failed. Different number of items ({items.Count}) and weights ({weights.Count}).");


            // With Replacement
            if (withReplacement)
            {
                if (items.Count * k < 100 || (float)items.Count / 5 > k) return ItemWeightedWithReplacementBinarySearch(items, weights, k);
                else return ItemWeightedWithReplacementAliasMethod(items, weights, k);
            }

            // Without Replacement
            else return ItemWeightedWithoutReplacement(items, weights, k);
        }
        public T[] Item<T>(WeightedList<T> weightedList, int k, bool withReplacement = true) => Item(weightedList.Items, weightedList.Weights, k, withReplacement);
        /// <summary>
        /// Picks <paramref name="count"/> uniformly random items from <paramref name="source"/> with replacement<br></br>
        /// - Executes in O(n) time
        /// </summary>
        /// <returns>
        /// An array containing the chosen items
        /// </returns>
        T[] ItemUniformWithReplacement<T>(IReadOnlyList<T> source, int count)
        {
            T[] chosen = new T[count];

            for (int i = 0; i < count; i++) chosen[i] = source.ElementAt(Num(source.Count));

            return chosen;
        }
        /// <summary>
        /// Picks <paramref name="count"/> uniformly random items from <paramref name="source"/> without replacement<br></br>
        /// - Executes in O(n) time<br></br>
        /// * Do NOT pass this method an Array or List (the IList overload is more efficient)
        /// </summary>
        /// <returns>
        /// An array containing the chosen items
        /// </returns>
        T[] ItemUniformWithoutReplacementRetryMethod<T>(IReadOnlyList<T> source, int count)
        {   // Kind of weird but pretty much O(infinity) worst case and O(n) best case
            // in practice lightning fast for small pick ratios (Picking < 15% of source)

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
                // This can be very slow if Count is close to SourceLength
                int randIndex = Num(source.Count());
                while (removed.ContainsKey(randIndex))
                    randIndex = Num(source.Count());

                removed.Add(randIndex, true);
                chosen[i] = source[randIndex];
            }

            return chosen;
        }
        T[] ItemUniformWithoutReplacementResevoirMethod<T>(IEnumerable<T> source, int count)
        {   // An O(n) unform random sampling approach with IEnumerable source
            T[] resevior = new T[count];

            IEnumerator<T> enumerator = source.GetEnumerator();

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
                int random = Num(++i);
                if (random < count) resevior[random] = enumerator.Current;
            }

            return resevior;
        }
        /// <summary>
        /// Chooses a random item from source based on their weights<br></br>
        /// - Executes in O(n) time<br></br>
        /// * If you need multiple picks DO NOT loop PickWeighted<br></br>
        /// (Instead call PickWeightedWith or WithoutReplacement they are optimized for the task)
        /// </summary>
        /// <summary>
        /// Picks <paramref name="count"/> items from the source list based on their weights with replacement<br></br>
        /// - Executes in O(n) time<br></br>
        /// </summary>
        /// <returns>
        /// An array containing the results
        /// </returns>
        T[] ItemWeightedWithReplacementAliasMethod<T>(IReadOnlyList<T> source, IReadOnlyList<float> weights, int count)
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
            if (allWeightsEqual) return ItemUniformWithReplacement(source, count);

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
                int bucket = Num(weights.Count);

                // Flip a weighted coin between the two possibilities in this slot
                picked[i] = source[rand.NextDouble() < probability[bucket] ? bucket : alias[bucket]];

            }
            #endregion

            return picked;
        }
        T[] ItemWeightedWithReplacementBinarySearch<T>(IReadOnlyList<T> source, IReadOnlyList<float> weights, int count)
        {
            // Prevents O(n^2) picking for Non-Indexed Enumerables
            // (Count() and ElementAt() both execute in O(n) time for many Enumerables)

            // Get CumulativeWeights
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
            if (allWeightsEqual) return ItemUniformWithReplacement(source, count);

            // Choose
            T[] chosen = new T[count];
            for (int i = 0; i < count; i++)
            {
                // Return Weighted Random Element
                double randVal = rand.NextDouble() * cumulativeWeights[cumulativeWeights.Count - 1];
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
        /// Picks <paramref name="count"/> items from the source list without replacement<br></br>
        /// - Executes in O(nlog(n)) time<br></br>
        /// </summary>
        /// <returns>
        /// An array containing the results.
        /// </returns>
        T[] ItemWeightedWithoutReplacement<T>(IReadOnlyList<T> source, IReadOnlyList<float> weights, int count)
        {   // Prep vars to detect edge cases
            bool allWeightsEqual = true;
            float firstWeight = weights[0];
            int numNonZeroWeights = weights[0] != 0 ? 1 : 0;

            // Get CumulativeWeights
            List<float> cumulativeWeights = new() { weights[0] };
            for (int i = 1; i < source.Count; i++)
            {
                if (weights[i] != 0) numNonZeroWeights++;
                if (weights[i] != firstWeight) allWeightsEqual = false;
                cumulativeWeights.Add(cumulativeWeights[i - 1] + weights[i]);
            }

            // Edge case: All weights are equal fall back to PickNonWeighted
            if (allWeightsEqual) return Item(source, count, false);

            // Edge Case: Requested pick of more items than the number of items with non-zero weights
            // - All weighted items should be returned
            // - Then perform non-weighted random picks for the remaining items
            // (This is faster + this method breaks if there are no remaining weighted items. It would always pick the first item)
            T[] picked = new T[count];
            if (numNonZeroWeights < count)
            {
                int pickIndex = 0;
                int zeroWeightIndex = 0;
                T[] nonWeightedItems = new T[source.Count - numNonZeroWeights];
                // Grab weighted items into picked and non-weighted items into nonWeightedItems
                for (int i = 0; i < source.Count; i++)
                    if (weights[i] == 0) nonWeightedItems[zeroWeightIndex++] = source[i];
                    else picked[pickIndex++] = source[i];

                // Than grab remaining items randomly from non weighted items
                T[] nonWeightedPicks = Item(nonWeightedItems, count, false);

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
                double randVal = rand.NextDouble() * cumulativeWeights[cumulativeWeights.Count - 1];
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
        /* Need to make a binary tree to get this working will do it later
        public T[] PickWeightedWithoutReplacementAExpJ<T>(IEnumerable<Weighted<T>> source, int count)
        {   // The A-ExpJ Resevoir Sampling method as described at: https://en.wikipedia.org/wiki/Reservoir_sampling
            IEnumerator<Weighted<T>> iter = source.GetEnumerator();
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
        #endregion
        #endregion
    }
}