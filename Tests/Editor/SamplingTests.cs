using LeafRand.Collections;
using LeafRand.Global;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LeafRand.Tests.Editor
{
    public class SamplingTests
    {
        const int LOWCOUNT = 33, MIDCOUNT = 50, HIGHCOUNT = 66, MAXCOUNT = 100;
        readonly List<string> strings = new( Enumerable.Range(0, MAXCOUNT) .Select(i => $"Hello there!{i}") .ToList());
        readonly List<Weighted<string>> weightedStrings = new(Enumerable.Range(0, MAXCOUNT).Select(i => new Weighted<string>($"Hello there!{i}", 1)).ToList());

        [Test] public void ItemUniform() { Rand.SetSeed(1); var result = Rand.Item(strings); Assert.True(strings.Contains(result)); }
        [Test] public void ItemWeighted() { Rand.SetSeed(1); var result = Rand.ItemWeighted(weightedStrings); }

        [Test, TestCase(0), TestCase(LOWCOUNT), TestCase(MIDCOUNT), TestCase(HIGHCOUNT), TestCase(MAXCOUNT)]
        public void ItemsUniformWithReplacement(int pickCount) 
        { 
            Rand.SetSeed(1); 
            var results = Rand.ItemsWithReplacement(strings, pickCount); 
            Assert.AreEqual(results.Length, pickCount); 
            foreach (var result in results) Assert.True(strings.Contains(result));
        }
        [Test, TestCase(0), TestCase(LOWCOUNT), TestCase(MIDCOUNT), TestCase(HIGHCOUNT), TestCase(MAXCOUNT)]
        public void ItemsUniformWithoutReplacement(int pickCount)
        {
            Rand.SetSeed(1);
            var results = Rand.ItemsWithoutReplacement(strings, pickCount);
            Assert.AreEqual(results.Length, pickCount);
            AssertResultsAreFromStrings(results);
            AssertAllResultsUnique(results);
        }
        [Test, TestCase(0), TestCase(LOWCOUNT), TestCase(MIDCOUNT), TestCase(HIGHCOUNT), TestCase(MAXCOUNT)]
        public void ItemsWeightedWithReplacement(int pickCount) 
        { 
            Rand.SetSeed(1);
            var results = Rand.ItemsWeightedWithReplacement(weightedStrings, pickCount);
            Assert.AreEqual(results.Length, pickCount);
            AssertResultsAreFromWeightedStrings(results);
        }
        [Test, TestCase(0), TestCase(LOWCOUNT), TestCase(MIDCOUNT), TestCase(HIGHCOUNT), TestCase(MAXCOUNT)]
        public void ItemsWeightedWithoutReplacement(int pickCount)
        {
            Rand.SetSeed(1);
            var results = Rand.ItemsWeightedWithoutReplacement(weightedStrings, pickCount);
            Assert.AreEqual(results.Length, pickCount);
            AssertResultsAreFromWeightedStrings(results);
            AssertAllResultsUnique(results);
        }

        void AssertResultsAreFromStrings(string[] results)
        {
            foreach (var result in results) Assert.True(strings.Contains(result)); // Result should be from strings 
        }
        void AssertResultsAreFromWeightedStrings(string[] results)
        {
            foreach (var result in results)
            {
                bool found = false;
                foreach (var weighted in weightedStrings)
                    if (weighted.Item.Equals(result))
                    {
                        found = true;
                        continue;
                    }
                Assert.True(found);
            }
        }
        static void AssertAllResultsUnique(string[] results)
        {
            foreach (var result in results) // Should only have any result once
            {
                int foundCount = 0;
                foreach (var resultt in results) if (resultt.Equals(result)) foundCount++;
                if (foundCount > 1) Assert.Fail();
            }
        }
    }
}