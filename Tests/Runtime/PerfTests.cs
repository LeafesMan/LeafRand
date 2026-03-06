using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;


namespace LeafRand.Tests.Runtime
{
    public class PerfTests
    {
        readonly List<string> pickFrom = new(new string[100].Select(_ => "Hello there!"));

        [Test, Performance, TestCase(100)]
        public void ItemsWithReplacement(int pickCount)
        {
            Measure.Method(() => Rand.ItemsWithReplacement(pickFrom, pickCount)).MeasurementCount(100).GC().Run();
        }

        [Test, Performance, TestCase(100)]
        public void ItemsWithoutReplacement(int pickCount)
        {
            Measure.Method(() => Rand.ItemsWithoutReplacement(pickFrom, pickCount)).MeasurementCount(100).GC().Run();
        }
    }
}