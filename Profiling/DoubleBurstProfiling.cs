using Unity.Burst;
using UnityEngine;
using System.Diagnostics;
using Unity.Jobs;
using Unity.Collections;

namespace LeafRand.Profiling
{
    public class DoubleBurstProfiling : MonoBehaviour, IProfileable
    {
        public double min, max;



        public string Profile(int iterations, uint seed)
        {
            var sw = new Stopwatch();
            var output = new NativeArray<double>(1, Allocator.TempJob);

            string results = "Double()\n";

            var job3 = new MathDoubleJob { iterations = iterations, seed = seed, output = output };
            sw.Restart();
            job3.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job16 = new BurstRandDoubleJob { iterations = iterations, seed = seed, output = output };
            sw.Restart();
            job16.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";



            results += "\n\n\nDouble(min)\n";

            var job7 = new MathDoubleMaxJob { iterations = iterations, max = max, seed = seed, output = output };
            sw.Restart();
            job7.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job15 = new BurstRandDoubleMaxJob { iterations = iterations, max = max, seed = seed, output = output };
            sw.Restart();
            job15.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";


            results += "\n\n\nDouble(min, max)\n";

            var job11 = new MathDoubleMinMaxJob { iterations = iterations, min = min, max = max, seed = seed, output = output };
            sw.Restart();
            job11.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job14 = new BurstRandDoubleMinMaxJob { iterations = iterations, min = min, max = max, seed = seed, output = output };
            sw.Restart();
            job14.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";

            output.Dispose();

            return results;
        }










        [BurstCompile]
        struct MathDoubleJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<double> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                double sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextDouble();

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct MathDoubleMaxJob : IJob
        {
            public int iterations;
            public double max;
            public uint seed;
            public NativeArray<double> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                double sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextDouble(max);

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct MathDoubleMinMaxJob : IJob
        {
            public int iterations;
            public double min;
            public double max;
            public uint seed;
            public NativeArray<double> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                double sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextDouble(min, max);

                output[0] = sum;
            }
        }



        [BurstCompile]
        struct BurstRandDoubleJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<double> output;

            public void Execute()
            {
                var rand = new Instanced.BurstRand(seed);
                double sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.Double();

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct BurstRandDoubleMaxJob : IJob
        {
            public int iterations;
            public double max;
            public uint seed;
            public NativeArray<double> output;

            public void Execute()
            {
                var rand = new Instanced.BurstRand(seed);
                double sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.Double(max);

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct BurstRandDoubleMinMaxJob : IJob
        {
            public int iterations;
            public double min;
            public double max;
            public uint seed;
            public NativeArray<double> output;

            public void Execute()
            {
                var rand = new Instanced.BurstRand(seed);
                double sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.Double(min, max);

                output[0] = sum;
            }
        }
    }
}