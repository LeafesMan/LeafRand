using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine.XR;

namespace LeafRand
{
    /// <summary>
    /// Static access to a global <see cref="RandStream"/> for random values and helpers.
    /// </summary>
    public class Rand
    {   // This class is effectively a static wrapper, repeates all method signatures from RandStream in static form
        // this allows calls like Rand.Num() for static access rather than something like Rand.Global.Num() or Rand.S.Num()
        // Tradeoff is extra maintenance for nice short global Rand calls
        /// <summary>
        /// The Static RandStream Instance.<br></br> Seed is based on system clock at startup.
        /// </summary>
        public static RandStream Stream { get; } = new(Environment.TickCount);
    }
}