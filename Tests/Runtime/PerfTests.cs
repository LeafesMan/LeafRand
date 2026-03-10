using LeafRand.Collections;
using LeafRand.Instanced;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;

namespace LeafRand.Tests.Runtime
{
    public class PerfTests
    {
        const int SAMPLECOUNT = 30;

        readonly List<string> strings = new(new string[1000].Select(_ => "Hello there!"));
        [Test, Performance, TestCase(1000000)]
        public void Item(int pickCount)
        {
            BurstRand rand = new(1);

            Measure.Method(() =>
            {
                var cached = strings.AsReadOnlySpan();
                for (int i = 0; i < pickCount; i++) rand.Item(cached);
            }).MeasurementCount(SAMPLECOUNT).Run();
        }

        readonly List<Weighted<string>> weightedStrings = Enumerable.Range(0, 1000).Select(_ => new Weighted<string>("Hello there!", 1)).ToList();
        [Test, Performance, TestCase(2000)]
        public void ItemWeighted(int pickCount)
        {
            BurstRand rand = new(1);

            Measure.Method(() =>  
            {
                var cached = weightedStrings.AsReadOnlySpan();
                for (int i = 0; i < pickCount; i++) rand.ItemWeighted(cached);
                }).MeasurementCount(SAMPLECOUNT).Run();
        }

        readonly List<string> wstrings = new(new string[1000].Select(_ => "Hello there!"));
        [Test, Performance, TestCase(1000000)]
        public void ItemsWithReplacement(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWithReplacement(wstrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }

        readonly List<string> wostrings = new(new string[100000].Select(_ => "Hello there!"));
        [Test, Performance, TestCase(80000)]
        public void ItemsWithoutReplacement(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWithoutReplacement(wostrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }

        readonly List<Weighted<string>> wwstrings = Enumerable.Range(0, 1000).Select(_ => new Weighted<string>("Hello there!", 1)).ToList();
        [Test, Performance, TestCase(100000)]
        public void ItemsWeightedWithReplacement(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWeightedWithReplacement(wwstrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }

        readonly List<Weighted<string>> wwostrings = Enumerable.Range(0, 1000).Select(_ => new Weighted<string>("Hello there!", 1)).ToList();
        [Test, Performance, TestCase(250)]
        public void ItemsWeightedWithoutReplacement(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWeightedWithoutReplacement(wwostrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }
    }
}