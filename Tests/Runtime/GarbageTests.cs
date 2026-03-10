using LeafRand.Instanced;
using LeafRand.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;


namespace LeafRand.Tests.Runtime
{
    public class GarbageTests
    {
        const int MEASUREMENTCOUNT = 1;
        const int NUMPICKSPERSAMPLE = 100;
        const int LISTSIZE = 500;
        readonly List<string> strings = new(new string[LISTSIZE].Select(_ => "Hello there!"));
        readonly List<Weighted<string>> weightedStrings = new(new Weighted<string>[LISTSIZE].Select(_ => new Weighted<string>( "Hello there!", 1)));
        BurstRand rand = new(1);



        #region Uniform With Replacement
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithReplacement(int pickCount)
        {
            Measure.Method(() => rand.ItemsWithReplacement(strings.AsReadOnlySpan(), pickCount)).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithReplacementCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWithReplacement(strings.AsReadOnlySpan(), outputCached)).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }
        #endregion
        #region Uniform Without Replacement
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithoutReplacementReservoirMethod(int pickCount)
        {
            Measure.Method(() => rand.SampleUniformWithoutReplacementReservoirMethod<string, string, BurstRand.ItemSelector<string>>(strings.AsReadOnlySpan(), new string[pickCount])).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithoutReplacementReservoirMethodCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.SampleUniformWithoutReplacementReservoirMethod<string, string, BurstRand.ItemSelector<string>>(strings.AsReadOnlySpan(), outputCached)).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithoutReplacementRetryMethod(int pickCount)
        {
            BurstRand rand = new(1);
            Measure.Method(() => rand.SampleUniformWithoutReplacementRetryMethod<string, string, BurstRand.ItemSelector<string>>(strings.AsReadOnlySpan(), new string[pickCount])).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithoutReplacementRetryMethodCachedOutput(int pickCount)
        {
            BurstRand rand = new(1);
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.SampleUniformWithoutReplacementRetryMethod<string, string, BurstRand.ItemSelector<string>>(strings.AsReadOnlySpan(), outputCached)).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }
        #endregion
        #region Weighted With Replacement
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithReplacementBinarySearch(int pickCount)
        {
            Measure.Method(() => rand.SampleWeightedWithReplacementBinarySearchMethod<string, string, BurstRand.WeightedItemSelector<string>>(weightedStrings.AsReadOnlySpan(), new string[pickCount])).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithReplacementBinarySearchCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.SampleWeightedWithReplacementBinarySearchMethod<string, string, BurstRand.WeightedItemSelector<string>>(weightedStrings.AsReadOnlySpan(), outputCached)).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithReplacementAliasMethod(int pickCount)
        {
            Measure.Method(() => rand.SampleWeightedWithReplacementAliasMethod<string, string, BurstRand.WeightedItemSelector<string>>(weightedStrings.AsReadOnlySpan(), new string[pickCount])).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithReplacementAliasMethodCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.SampleWeightedWithReplacementAliasMethod<string, string, BurstRand.WeightedItemSelector<string>>(weightedStrings.AsReadOnlySpan(), outputCached)).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }
        #endregion
        #region Weighted Without Replacement
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithoutReplacement(int pickCount)
        {
            Measure.Method(() => rand.ItemsWeightedWithoutReplacement(weightedStrings.AsReadOnlySpan(), pickCount)).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithoutReplacementCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWeightedWithoutReplacement(weightedStrings.AsReadOnlySpan(), outputCached)).GC().MeasurementCount(MEASUREMENTCOUNT).Run();
        }
        #endregion
    }
}