using UnityEngine;
using System.Diagnostics;
using LeafRand;

namespace LeafRand.Profiling
{
    public class FloatProfiling : MonoBehaviour, IProfileable
    {
        public float min, max;



        public string Profile(int iterations, uint seed)
        {
            string output = "Float()\n";
            Stopwatch stopwatch = Stopwatch.StartNew();
            SysRandWrapper sysRand;
            Unity.Mathematics.Random mathRand;
            Instanced.BurstRand myBurstRand;
            Instanced.Rand myManagedRand;
            float result = 0;



            sysRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.Float();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextFloat();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Float();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";

            myManagedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += myManagedRand.Float();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            myBurstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += myBurstRand.Float();
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";






            output += "\n\n\nFloat(max)\n";

            sysRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.Float(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextFloat(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Float(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";

            myManagedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += myManagedRand.Float(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            myBurstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += myBurstRand.Float(max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";






            output += "\n\n\nFloat(min, max)\n";

            sysRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += sysRand.Float(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms System\n";

            mathRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += mathRand.NextFloat(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms Math\n";

            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += Rand.Float(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyGlobalRand\n";

            myManagedRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += myManagedRand.Float(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyManagedRand\n";

            myBurstRand = new(seed);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result += myBurstRand.Float(min, max);
            }
            stopwatch.Stop();
            output += stopwatch.Elapsed.TotalMilliseconds + "ms MyBurstRand\n";

            stopwatch.Restart();
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