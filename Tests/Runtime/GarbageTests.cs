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
        const int SAMPLECOUNT = 30;
        const int NUMPICKSPERSAMPLE = 500;
        readonly List<string> strings = new(new string[NUMPICKSPERSAMPLE].Select(_ => "Hello there!"));
        readonly List<Weighted<string>> weightedStrings = new(new Weighted<string>[NUMPICKSPERSAMPLE].Select(_ => new Weighted<string>( "Hello there!", 1)));
        BurstRand rand = new(1);



        void FillStrings() 
        {
            while (strings.Count < NUMPICKSPERSAMPLE) strings.Add("Hello there!");
        }


        #region Uniform With Replacement
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithReplacement(int pickCount)
        {
            Measure.Method(() => rand.ItemsWithReplacement(strings.AsReadOnlySpan(), pickCount)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithReplacementCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWithReplacement(strings.AsReadOnlySpan(), outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        #endregion
        #region Uniform Without Replacement
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithoutReplacementReservoirMethod(int pickCount)
        {
            Measure.Method(() => rand.ItemsUniformWithoutReplacementReservoirMethodOld(strings.AsReadOnlySpan(), new string[pickCount])).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithoutReplacementReservoirMethodCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsUniformWithoutReplacementReservoirMethodOld(strings.AsReadOnlySpan(), outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithoutReplacementRetryMethod(int pickCount)
        {
            Measure.Method(() => rand.ItemsUniformWithoutReplacementRetryMethodOld(strings.AsReadOnlySpan(), new string[pickCount])).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformWithoutReplacementRetryMethodCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsUniformWithoutReplacementRetryMethodOld(strings.AsReadOnlySpan(), outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        #endregion
        #region Uniform Extract
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformExtract(int pickCount)
        {
            Measure.Method(() => rand.ItemsExtract(strings, pickCount)).CleanUp(FillStrings).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsUniformExtractCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsExtract(strings, outputCached)).CleanUp(FillStrings).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        #endregion
        #region Weighted With Replacement
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithReplacementBinarySearch(int pickCount)
        {
            Measure.Method(() => rand.ItemsWeightedWithReplacementBinarySearchOld(weightedStrings.AsReadOnlySpan(), new string[pickCount])).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithReplacementBinarySearchCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWeightedWithReplacementBinarySearchOld(weightedStrings.AsReadOnlySpan(), outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithReplacementAliasMethod(int pickCount)
        {
            Measure.Method(() => rand.ItemsWeightedWithReplacementAliasMethodOld(weightedStrings.AsReadOnlySpan(), new string[pickCount])).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithReplacementAliasMethodCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWeightedWithReplacementAliasMethodOld(weightedStrings.AsReadOnlySpan(), outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        #endregion
        #region Weighted Without Replacement
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithoutReplacement(int pickCount)
        {
            Measure.Method(() => rand.ItemsWeightedWithoutReplacement(weightedStrings.AsReadOnlySpan(), pickCount)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWeightedWithoutReplacementCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWeightedWithoutReplacement(weightedStrings.AsReadOnlySpan(), outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        #endregion
    }
}