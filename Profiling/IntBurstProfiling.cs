using Unity.Burst;
using UnityEngine;
using System.Diagnostics;
using Unity.Jobs;
using Unity.Collections;

namespace LeafRand.Profiling
{
    public class IntBurstProfiling : MonoBehaviour, IProfileable
    {
        public int min, max;


        public string Profile(int iterations, uint seed)
        {
            var sw = new Stopwatch();
            var output = new NativeArray<int>(1, Allocator.TempJob);

            string results = "Int()\n";

            var job3 = new MathIntJob { iterations = iterations, seed = seed, output = output };
            sw.Restart();
            job3.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job16 = new BurstRandIntJob { iterations = iterations, seed = seed, output = output };
            sw.Restart();
            job16.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";



            results += "\n\n\nInt(min)\n";

            
            var job7 = new MathIntMaxJob { iterations = iterations, max = max, seed = seed, output = output };
            sw.Restart();
            job7.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job15 = new BurstRandIntMaxJob { iterations = iterations, max = max, seed = seed, output = output };
            sw.Restart();
            job15.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";


            results += "\n\n\nInt(min, max)\n";

            
            var job11 = new MathIntMinMaxJob { iterations = iterations, min = min, max = max, seed = seed, output = output };
            sw.Restart();
            job11.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job14 = new BurstRandIntMinMaxJob { iterations = iterations, min = min, max = max, seed = seed, output = output };
            sw.Restart();
            job14.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";

            output.Dispose();

            return results;
        }




        [BurstCompile]
        struct MathIntJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<int> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                int sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextInt();

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct MathIntMaxJob : IJob
        {
            public int iterations;
            public int max;
            public uint seed;
            public NativeArray<int> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                int sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextInt(max);

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct MathIntMinMaxJob : IJob
        {
            public int iterations;
            public int min;
            public int max;
            public uint seed;
            public NativeArray<int> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                int sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextInt(min, max);

                output[0] = sum;
            }
        }

        [BurstCompile]
        struct BurstRandIntJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<int> output;

            public void Execute()
            {
                var rand = new Instanced.BurstRand(seed);
                int sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.Int();

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct BurstRandIntMaxJob : IJob
        {
            public int iterations;
            public int max;
            public uint seed;
            public NativeArray<int> output;

            public void Execute()
            {
                var rand = new Instanced.BurstRand(seed);
                int sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.Int(max);

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct BurstRandIntMinMaxJob : IJob
        {
            public int iterations;
            public int min;
            public int max;
            public uint seed;
            public NativeArray<int> output;

            public void Execute()
            {
                var rand = new Instanced.BurstRand(seed);
                int sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.Int(min, max);

                output[0] = sum;
            }
        }
    }
}