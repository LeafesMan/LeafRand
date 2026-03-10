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
        [Test, Performance, TestCase(1000000)]
        public void ItemOld(int pickCount)
        {
            BurstRand rand = new(1);

            Measure.Method(() =>
            {
                var cached = strings.AsReadOnlySpan();
                for (int i = 0; i < pickCount; i++) rand.ItemOld(cached);
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
        [Test, Performance, TestCase(2000)]
        public void ItemWeightedOld(int pickCount)
        {
            BurstRand rand = new(1);

            Measure.Method(() =>
            {
                var cached = weightedStrings.AsReadOnlySpan();
                for (int i = 0; i < pickCount; i++) rand.ItemWeightedOld(cached);
            }).MeasurementCount(SAMPLECOUNT).Run();
        }


        readonly List<string> wstrings = new(new string[1000].Select(_ => "Hello there!"));
        [Test, Performance, TestCase(1000000)]
        public void ItemsWithReplacement(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWithReplacement(wstrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }
        [Test, Performance, TestCase(1000000)]
        public void ItemsWithReplacementOld(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWithReplacementOld(wstrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }


        readonly List<string> wostrings = new(new string[100000].Select(_ => "Hello there!"));
        [Test, Performance, TestCase(80000)]
        public void ItemsWithoutReplacement(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWithoutReplacement(wostrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }
        [Test, Performance, TestCase(80000)]
        public void ItemsWithoutReplacementOld(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWithoutReplacementOld(wostrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }

        readonly List<Weighted<string>> wwstrings = Enumerable.Range(0, 1000).Select(_ => new Weighted<string>("Hello there!", 1)).ToList();
        [Test, Performance, TestCase(100000)]
        public void ItemsWeightedWithReplacement(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWeightedWithReplacement(wwstrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }
        [Test, Performance, TestCase(100000)]
        public void ItemsWeightedWithReplacementOld(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWeightedWithReplacementOld(wwstrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }

        readonly List<Weighted<string>> wwostrings = Enumerable.Range(0, 1000).Select(_ => new Weighted<string>("Hello there!", 1)).ToList();
        [Test, Performance, TestCase(250)]
        public void ItemsWeightedWithoutReplacement(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWeightedWithoutReplacement(wwostrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }
        [Test, Performance, TestCase(250)]
        public void ItemsWeightedWithoutReplacementOld(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => { rand.ItemsWeightedWithoutReplacementOld(wwostrings.AsReadOnlySpan(), pickCount); }).MeasurementCount(SAMPLECOUNT).Run();
        }
    }
}