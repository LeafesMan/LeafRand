using Unity.Burst;
using UnityEngine;
using System.Diagnostics;
using Unity.Jobs;
using Unity.Collections;

namespace LeafRand.Profiling
{
    public class BoolBurstProfiling : MonoBehaviour, IProfileable
    {
        public float chance = 0.5f;



        public string Profile(int iterations, uint seed)
        {
            var output = new NativeArray<int>(1, Allocator.TempJob);
            var sw = new Stopwatch();


            string results = "Bool()\n";

            var job1 = new MathBoolJob { iterations = iterations, seed = seed, result = output };
            sw.Restart();
            job1.Schedule().Complete();
            sw.Stop();
            results += $"{sw.Elapsed.TotalMilliseconds} ms MathRand\n";

            var job2 = new BurstRandBoolJob { iterations = iterations, seed = seed, result = output };
            sw.Restart();
            job2.Schedule().Complete();
            sw.Stop();
            results += $"{sw.Elapsed.TotalMilliseconds} ms BurstRand\n";


            results += "\n\n\nBoolUInt(chance)\n";

            var job3 = new MathBoolUIntJob { iterations = iterations, chance = chance, seed = seed, result = output };
            sw.Restart();
            job3.Schedule().Complete();
            sw.Stop();
            results += $"{sw.Elapsed.TotalMilliseconds} ms MathRand\n";

            var job4 = new BurstRandBoolUIntJob { iterations = iterations, chance = chance, seed = seed, result = output };
            sw.Restart();
            job4.Schedule().Complete();
            sw.Stop();
            results += $"{sw.Elapsed.TotalMilliseconds} ms BurstRand\n";


            results += "\n\n\nBoolFloat(chance)\n";

            var job5 = new MathBoolFloatJob { iterations = iterations, chance = chance, seed = seed, result = output };
            sw.Restart();
            job5.Schedule().Complete();
            sw.Stop();
            results += $"{sw.Elapsed.TotalMilliseconds} ms BurstRand\n";

            var job6 = new BurstRandBoolFloatJob { iterations = iterations, chance = chance, seed = seed, result = output };
            sw.Restart();
            job6.Schedule().Complete();
            sw.Stop();
            results += $"{sw.Elapsed.TotalMilliseconds} ms BurstRand\n";


            results += "\n\n\nBoolDouble(chance)\n";

            var job7 = new MathBoolDoubleJob { iterations = iterations, chance = chance, seed = seed, result = output };
            sw.Restart();
            job7.Schedule().Complete();
            sw.Stop();
            results += $"{sw.Elapsed.TotalMilliseconds} ms MathRand\n";

            var job8 = new BurstRandBoolDoubleJob { iterations = iterations, chance = chance, seed = seed, result = output };
            sw.Restart();
            job8.Schedule().Complete();
            sw.Stop();
            results += $"{sw.Elapsed.TotalMilliseconds} ms BurstRand\n";

            
            // Use Output so that its not optimized away
            UnityEngine.Debug.Log("Sum of burst trues: " + output);
            output.Dispose();


            return results;
        }
        [BurstCompile]
        struct MathBoolJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<int> result;

            public void Execute()
            {
                var mathRand = new Unity.Mathematics.Random(seed);
                int successes = 0;
                for (int i = 0; i < iterations; i++)
                    if (mathRand.NextBool()) successes++;

                result[0] = successes;
            }
        }
        [BurstCompile]
        struct MathBoolUIntJob : IJob
        {
            public int iterations;
            public float chance;
            public uint seed;
            public NativeArray<int> result;

            public void Execute()
            {
                var mathRand = new Unity.Mathematics.Random(seed);
                int successes = 0;
                for (int i = 0; i < iterations; i++)
                    if (mathRand.NextUInt() < (uint)(chance * uint.MaxValue)) successes++;

                result[0] = successes;
            }
        }
        [BurstCompile]
        struct MathBoolFloatJob : IJob
        {
            public int iterations;
            public float chance;
            public uint seed;
            public NativeArray<int> result;

            public void Execute()
            {
                var mathRand = new Unity.Mathematics.Random(seed);
                int successes = 0;
                for (int i = 0; i < iterations; i++)
                    if (mathRand.NextFloat() < chance) successes++;

                result[0] = successes;
            }
        }
        [BurstCompile]
        struct MathBoolDoubleJob : IJob
        {
            public int iterations;
            public double chance;
            public uint seed;
            public NativeArray<int> result;

            public void Execute()
            {
                var mathRand = new Unity.Mathematics.Random(seed);
                int successes = 0;
                for (int i = 0; i < iterations; i++)
                    if (mathRand.NextDouble() < chance) successes++;

                result[0] = successes;
            }
        }

        [BurstCompile]
        struct BurstRandBoolJob : IJob
        {
            public int iterations;
            public uint seed;
            public NativeArray<int> result;

            public void Execute()
            {
                var rand = new BurstRand(seed);
                int successes = 0;
                for (int i = 0; i < iterations; i++)
                    if (rand.Bool()) successes++;

                result[0] = successes;
            }
        }
        [BurstCompile]
        struct BurstRandBoolUIntJob : IJob
        {
            public int iterations;
            public float chance;
            public uint seed;
            public NativeArray<int> result;

            public void Execute()
            {
                var rand = new BurstRand(seed);
                int successes = 0;
                for (int i = 0; i < iterations; i++)
                    if (rand.BoolUInt(chance)) successes++;

                result[0] = successes;
            }
        }
        [BurstCompile]
        struct BurstRandBoolFloatJob : IJob
        {
            public int iterations;
            public float chance;
            public uint seed;
            public NativeArray<int> result;

            public void Execute()
            {
                var rand = new BurstRand(seed);
                int successes = 0;
                for (int i = 0; i < iterations; i++)
                    if (rand.BoolFloat(chance)) successes++;

                result[0] = successes;
            }
        }
        [BurstCompile]
        struct BurstRandBoolDoubleJob : IJob
        {
            public int iterations;
            public double chance;
            public uint seed;
            public NativeArray<int> result;

            public void Execute()
            {
                var rand = new BurstRand(seed);
                int successes = 0;
                for (int i = 0; i < iterations; i++)
                    if (rand.BoolDouble(chance)) successes++;

                result[0] = successes;
            }
        }
    }
}