using LeafRand.Instanced;
using LeafRand.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;

namespace LeafRand.Tests.Runtime
{
    public class PerfTests
    {
        const int SAMPLECOUNT = 30;
        const int NUMPICKSPERSAMPLE = 10000;
        readonly List<string> strings = new(new string[NUMPICKSPERSAMPLE].Select(_ => "Hello there!"));
        readonly List<Weighted<string>> weightedStrings =
            Enumerable.Range(0, NUMPICKSPERSAMPLE)
                      .Select(_ => new Weighted<string>("Hello there!", 1))
                      .ToList();

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemWeighted(int pickCount)
        {
            BurstRand rand = new(1);

            Measure.Method(() =>  
            {
                var cached = weightedStrings.AsReadOnlySpan();
                for (int i = 0; i < NUMPICKSPERSAMPLE; i++) rand.ItemWeighted(cached);
                }).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
    }
}