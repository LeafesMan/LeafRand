using Unity.Burst;
using UnityEngine;
using System.Diagnostics;
using Unity.Jobs;
using Unity.Collections;

namespace LeafRand.Profiling
{
    public class UIntBurstProfiling : MonoBehaviour, IProfileable
    {
        public uint min, max;

        public string Profile(int iterations, uint seed)
        {
            var sw = new Stopwatch();
            var output = new NativeArray<uint>(1, Allocator.TempJob);

            string results = "UInt()\n";

            var job3 = new MathUIntJob { iterations = iterations, seed = seed, output = output };
            sw.Restart();
            job3.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job16 = new BurstRandUIntJob { iterations = iterations, seed = seed, output = output };
            sw.Restart();
            job16.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";



            results += "\n\n\nUInt(min)\n";


            var job7 = new MathUIntMaxJob { iterations = iterations, max = max, seed = seed, output = output };
            sw.Restart();
            job7.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job15 = new BurstRandUIntMaxJob { iterations = iterations, max = max, seed = seed, output = output };
            sw.Restart();
            job15.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";


            results += "\n\n\nUInt(min, max)\n";


            var job11 = new MathUIntMinMaxJob { iterations = iterations, min = min, max = max, seed = seed, output = output };
            sw.Restart();
            job11.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job14 = new BurstRandUIntMinMaxJob { iterations = iterations, min = min, max = max, seed = seed, output = output };
            sw.Restart();
            job14.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms BurstRand" + output[0] + "\n";

            output.Dispose();

            return results;
        }




        [BurstCompile]
        struct MathUIntJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<uint> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                uint sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextUInt();

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct MathUIntMaxJob : IJob
        {
            public int iterations;
            public uint max;
            public uint seed;
            public NativeArray<uint> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                uint sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextUInt(max);

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct MathUIntMinMaxJob : IJob
        {
            public int iterations;
            public uint min;
            public uint max;
            public uint seed;
            public NativeArray<uint> output;

            public void Execute()
            {
                var rand = new Unity.Mathematics.Random(seed);
                uint sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.NextUInt(min, max);

                output[0] = sum;
            }
        }

        [BurstCompile]
        struct BurstRandUIntJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<uint> output;

            public void Execute()
            {
                var rand = new BurstRand(seed);
                uint sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.UInt();

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct BurstRandUIntMaxJob : IJob
        {
            public int iterations;
            public uint max;
            public uint seed;
            public NativeArray<uint> output;

            public void Execute()
            {
                var rand = new BurstRand(seed);
                uint sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.UInt(max);

                output[0] = sum;
            }
        }
        [BurstCompile]
        struct BurstRandUIntMinMaxJob : IJob
        {
            public int iterations;
            public uint min;
            public uint max;
            public uint seed;
            public NativeArray<uint> output;

            public void Execute()
            {
                var rand = new BurstRand(seed);
                uint sum = 0;
                for (int i = 0; i < iterations; i++) sum += rand.UInt(min, max);

                output[0] = sum;
            }
        }
    }
}