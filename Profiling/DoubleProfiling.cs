using UnityEngine;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LeafRand.Profiling
{
    public class DoubleProfiling : MonoBehaviour, IProfileable
    {
        public double min, max;
        public string Profile(int iterations, uint seed)
        {
            string output = "Double()\n";
            double result = 0;

            System.Random sysRand;
            Unity.Mathematics.Random mathRand;
            BurstRand burstRand;
            ManagedRand managedRand;
            Stopwatch stopwatch = Stopwatch.StartNew();


            sysRand = new((int)seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.NextDouble();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextDouble();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.Double();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms BurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.Double();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms ManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Double();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms GlobalRand\n";



            output += "\n\n\nDouble(max)\n";

            sysRand = new((int)seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.NextDouble() * max;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextDouble(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.Double(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms BurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.Double(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms ManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Double(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms GlobalRand\n";




            output += "\n\n\nDouble(min, max)\n";

            sysRand = new((int)seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.NextDouble() * (max - min) + min;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextDouble(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";


            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.Double(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms BurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.Double(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms ManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Double(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms GlobalRand\n";


            UnityEngine.Debug.Log(result);


            return output;
        }
    }

    public struct SysRandWrapper
    {
        public System.Random rand;

        public SysRandWrapper(uint seed) => rand = new((int)seed);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Float() => (float)rand.NextDouble();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Float(float max) => (float)rand.NextDouble() * max;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Float(float min, float max) => (float)rand.NextDouble() * (max - min) + min;
    }
}