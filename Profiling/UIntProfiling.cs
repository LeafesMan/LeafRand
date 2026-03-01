using UnityEngine;
using System.Diagnostics;

namespace LeafRand.Profiling
{
    public class UIntProfiling : MonoBehaviour, IProfileable
    {
        public uint min, max;

        public string Profile(int iterations, uint seed)
        {
            string output = "UInt()\n";
            uint result = 0;

            Unity.Mathematics.Random mathRand;
            Instanced.BurstRand burstRand;
            Instanced.Rand managedRand;

            Stopwatch stopwatch = Stopwatch.StartNew();



            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextUInt();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.UInt();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.UInt();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.UInt();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";



            output += "\n\n\nUInt(min)\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextUInt(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.UInt(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.UInt(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.UInt(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";





            output += "\n\n\nUInt(min, max)\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextUInt(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.UInt(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.UInt(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.UInt(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";

            UnityEngine.Debug.Log(result);

            return output;
        }
    }
}