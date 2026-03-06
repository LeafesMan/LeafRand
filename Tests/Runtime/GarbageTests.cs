using LeafRand.Instanced;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;


namespace LeafRand.Tests.Editor
{
    public class GarbageTests
    {
        const int SAMPLECOUNT = 30;
        const int NUMPICKSPERSAMPLE = 500;
        readonly List<string> pickFrom = new(new string[NUMPICKSPERSAMPLE].Select(_ => "Hello there!"));
        BurstRand rand = new BurstRand(1);


        void RefillPickFrom() 
        {
            while (pickFrom.Count < NUMPICKSPERSAMPLE) pickFrom.Add("Hello there!");
        }



        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWithReplacement(int pickCount)
        {
            Measure.Method(() => rand.ItemsWithReplacement(pickFrom, pickCount)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWithReplacementCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWithReplacement(pickFrom, outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }


        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWithoutReplacement(int pickCount)
        {
            Measure.Method(() => rand.ItemsWithoutReplacement(pickFrom, pickCount)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }

        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWithoutReplacementCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsWithoutReplacement(pickFrom, outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        [Test, Performance, TestCase(NUMPICKSPERSAMPLE)]
        public void ItemsWithoutReplacementRetryMethodCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsUniformWithoutReplacementRetryMethod(pickFrom, outputCached)).MeasurementCount(SAMPLECOUNT).GC().Run();
        }
        [Test, Performance, TestCase(10)]
        public void ItemsExtract(int pickCount)
        {
            Measure.Method(() => rand.ItemsExtract(pickFrom, pickCount)).CleanUp(RefillPickFrom).MeasurementCount(SAMPLECOUNT).GC().Run();
        }

        [Test, Performance, TestCase(10)]
        public void ItemsExtractCachedOutput(int pickCount)
        {
            string[] outputCached = new string[pickCount];

            Measure.Method(() => rand.ItemsExtract(pickFrom, outputCached)).CleanUp(RefillPickFrom).MeasurementCount(SAMPLECOUNT).GC().Run();
        }

    }
}