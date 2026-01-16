using System.Diagnostics;
using UnityEngine;

namespace LeafRand.Profiling
{
    public class BoolProfiling : MonoBehaviour, IProfileable
    {
        public float chance;


        public string Profile(int iterations, uint seed)
        {
            string output = "Bool()\n";
            int successes = 0;
            double doubleChance = (double)chance;

            Unity.Mathematics.Random mathRand;
            BurstRand burstRand;
            ManagedRand managedRand;
            Stopwatch stopwatch = new Stopwatch();


            mathRand= new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (mathRand.NextBool()) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (burstRand.Bool()) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms BurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (managedRand.Bool()) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms ManagedRand\n";

            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (Rand.Bool()) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms GlobalRand\n";


            output += "\n\n\nBoolUInt(chance)\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (mathRand.NextUInt() < (uint)(chance * uint.MaxValue)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (burstRand.BoolUInt(chance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms BurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (managedRand.BoolUInt(chance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms ManagedRand\n";

            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (Rand.BoolUInt(chance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms GlobalRand\n";


            output += "\n\n\nBoolFloat(chance)\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (mathRand.NextFloat() < chance) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (burstRand.BoolFloat(chance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms BurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (managedRand.BoolFloat(chance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms ManagedRand\n";

            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (Rand.BoolFloat(chance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms GlobalRand\n";


            output += "\n\n\nBoolDouble(chance)\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (mathRand.NextDouble() < doubleChance) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (burstRand.BoolDouble(doubleChance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms BurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (managedRand.BoolDouble(doubleChance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms ManagedRand\n";

            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                if (Rand.BoolDouble(doubleChance)) successes++;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms GlobalRand\n";



            UnityEngine.Debug.Log(successes);

            return output;
        }
    }
}