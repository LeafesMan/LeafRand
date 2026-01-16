using UnityEngine;
using System.Diagnostics;

namespace LeafRand.Profiling
{
    public class IntProfiling : MonoBehaviour, IProfileable
    {

        public int min, max;

        public string Profile(int iterations, uint seed)
        {
            string output = "Int()\n";
            int result = 0;

            System.Random sysRand;
            Unity.Mathematics.Random mathRand;
            BurstRand burstRand;
            ManagedRand managedRand;

            Stopwatch stopwatch = Stopwatch.StartNew();


            sysRand = new((int)seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.Next();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextInt();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.Int();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.Int();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Int();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";



            output += "\n\n\nInt(min)\n";

            sysRand = new((int)seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.Next(max) * max;
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextInt(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.Int(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.Int(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Int(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";





            output += "\n\n\nInt(min, max)\n";

            sysRand = new((int)seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.Next(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextInt(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            burstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += burstRand.Int(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";

            managedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += managedRand.Int(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            Rand.SetSeed(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Int(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";

            Random.InitState((int)seed);
            stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                result += Random.Range(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Unity\n";

            UnityEngine.Debug.Log(result);

            return output;
        }
    }
}