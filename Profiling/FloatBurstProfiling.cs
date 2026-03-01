using Unity.Burst;
using UnityEngine;
using System.Diagnostics;
using Unity.Jobs;
using Unity.Collections;

namespace LeafRand.Profiling
{
    public class FloatBurstProfiling : MonoBehaviour, IProfileable
    {
        public float min, max;

        public string Profile(int iterations, uint seed)
        {
            var sw = new Stopwatch();
            var output = new NativeArray<float>(1, Allocator.TempJob);

            string results = "Float()\n";

            
            var job3 = new MathFloatJob { iterations = iterations, seed = seed, output = output };
            sw.Restart();
            job3.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";
            
            var job16 = new MyRandBurstFloat { iterations = iterations, seed = seed, output = output };
            sw.Restart();
            job16.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms MyRand" + output[0] + "\n";



            results += "\n\n\nFloat(max)\n";

            
            var job7 = new MathFloatFloatJob { iterations = iterations, max = max, seed = seed, output = output };
            sw.Restart();
            job7.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";

            var job15 = new MyRandBurstFloatFloat { iterations = iterations, max = max, seed = seed, output = output };
            sw.Restart();
            job15.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms MyRand" + output[0] + "\n";


            results += "\n\n\nFloat(min, max)\n";

            
            var job11 = new MathFloatFloatFloatJob { iterations = iterations, min = min, max = max, seed = seed, output = output };
            sw.Restart();
            job11.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms Math " + output[0] + "\n";
            
            var job14 = new MyRandBurstFloatFloatFloat { iterations = iterations, min = min, max = max, seed = seed, output = output };
            sw.Restart();
            job14.Schedule().Complete();
            sw.Stop();
            results += sw.Elapsed.TotalMilliseconds + "ms MyRand" + output[0] + "\n";

            output.Dispose();

            return results;
        }

        [BurstCompile]
        struct MathFloatJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<float> output;

            public void Execute()
            {
                Unity.Mathematics.Random mathRand = new(seed);

                for (int i = 0; i < iterations; i++) output[0] = mathRand.NextFloat();
            }
        }
        [BurstCompile]
        struct MathFloatFloatJob : IJob
        {
            public int iterations;
            public float max;
            public uint seed;
            public NativeArray<float> output;

            public void Execute()
            {
                Unity.Mathematics.Random mathRand = new(seed);

                for (int i = 0; i < iterations; i++) output[0] = mathRand.NextFloat(max);
            }
        }
        [BurstCompile]
        struct MathFloatFloatFloatJob : IJob
        {
            public int iterations;
            public float min, max;
            public uint seed;
            public NativeArray<float> output;

            public void Execute()
            {
                Unity.Mathematics.Random mathRand = new(seed);

                for (int i = 0; i < iterations; i++) output[0] = mathRand.NextFloat(min, max);
            }
        }



        [BurstCompile]
        struct MyRandBurstFloat : IJob
        {
            public int iterations;
            public uint seed;

            public NativeArray<float> output;

            public void Execute()
            {
                Instanced.BurstRand mathRand = new(seed);

                for (int i = 0; i < iterations; i++) output[0] = mathRand.Float();
            }
        }
        [BurstCompile]
        struct MyRandBurstFloatFloat : IJob
        {
            public int iterations;
            public float max;
            public uint seed;

            public NativeArray<float> output;

            public void Execute()
            {
                Instanced.BurstRand mathRand = new(seed);

                for (int i = 0; i < iterations; i++) output[0] = mathRand.Float(max);
            }
        }
        [BurstCompile]
        struct MyRandBurstFloatFloatFloat : IJob
        {
            public int iterations;
            public float min, max;
            public uint seed;

            public NativeArray<float> output;

            public void Execute()
            {
                Instanced.BurstRand mathRand = new(seed);

                for (int i = 0; i < iterations; i++) output[0] = mathRand.Float(min, max);
            }
        }
    }
}