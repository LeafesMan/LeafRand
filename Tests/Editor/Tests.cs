using LeafRand.Instanced;
using LeafRand.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;


namespace LeafRand.Tests.Editor
{
    public class Tests
    {
        readonly List<string> pickFrom = new(new string[100].Select(_ => "Hello there!"));
        BurstRand rand = new BurstRand();

        [Test, Performance, TestCase(100)]
        public void ItemsWithReplacementGarbage(int pickCount)
        {
            rand = new BurstRand(1);
            new List<int>() { };

            Measure.Method(() => rand.ItemsWithReplacement(pickFrom.AsReadOnlySpan(), pickCount)).MeasurementCount(100).GC().Run();
        }

        [Test, Performance, TestCase(100)]
        public void ItemsWithReplacementNoGarbage(int pickCount)
        {
            rand = new BurstRand(1);
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWithReplacement(pickFrom.AsReadOnlySpan(), outputCached)).MeasurementCount(100).GC().Run();
        }
    }
}