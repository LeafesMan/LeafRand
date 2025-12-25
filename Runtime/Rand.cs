using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeafRand
{
    /// <summary>
    /// Static access to a global <see cref="RandStream"/> for random values and helpers.
    /// </summary>
    public static class Rand
    {   // This class is effectively a static wrapper, repeates all method signatures from RandStream in static form
        // this allows calls like Rand.Num() for static access rather than something like Rand.Global.Num() or Rand.S.Num()
        // Tradeoff is extra maintenance for nice short global Rand calls
        /// <summary>
        /// The Static RandStream Instance.<br></br> Seed is based on system clock at startup.
        /// </summary>
        public static RandStream Stream { get; } = new(Environment.TickCount);

        /// <include file="Docs.xml" path="Doc/Num/Int"/>
        public static int Num(int max) => Stream.Num(max);
        /// <include file="Docs.xml" path="Doc/Num/IntInt"/>
        public static int Num(int min, int max) => Stream.Num(min, max);
        /// <include file="Docs.xml" path="Doc/Num/Vector2Int"/>
        public static int Num(Vector2Int range) => Stream.Num(range.x, range.y);
        /// <include file="Docs.xml" path="Doc/Num"/>
        public static float Num() => Stream.Num();
        /// <include file="Docs.xml" path="Doc/Num/Float"/>
        public static float Num(float max) => Stream.Num(max);
        /// <include file="Docs.xml" path="Doc/Num/FloatFloat"/>
        public static float Num(float min, float max) => Stream.Num(min, max);
        /// <include file="Docs.xml" path="Doc/Num/Vector2"/>
        public static float Num(Vector2 range) => Stream.Num(range.x, range.y);
        public static double Num(double max) => Stream.Num(max);
        /// <include file="Docs.xml" path="Doc/Angle"/>
        public static float Angle() => Stream.Angle();
        /// <include file="Docs.xml" path="Doc/Bool"/>
        public static bool Bool(float successChance = 0.5f) => Stream.Bool(successChance);
        /// <include file="Docs.xml" path="Doc/Chance"/>
        public static bool Chance(float successChance = 0.5f) => Stream.Bool(successChance);
        /// <include file="Docs.xml" path="Doc/Sign"/>
        public static int Sign() => Stream.Sign();
        /// <include file="Docs.xml" path="Doc/Dir2D"/>
        public static Vector2 Dir2D() => Stream.Dir2D();
        /// <include file="Docs.xml" path="Doc/Dir2D/FloatFloat"/>
        public static Vector2 Dir2D(float minTheta, float maxTheta) => Stream.Dir2D(minTheta, maxTheta);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2"/>
        public static Vector2 Dir2D(Vector2 thetaRange) => Stream.Dir2D(thetaRange.x, thetaRange.y);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2FloatFloat"/>
        public static Vector2 Dir2D(Vector2 basis, float minDegrees, float maxDegrees) => Stream.Dir2D(basis, minDegrees, maxDegrees);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2Vector2"/>
        public static Vector2 Dir2D(Vector2 basis, Vector2 degreesRange) => Stream.Dir2D(basis, degreesRange.x, degreesRange.y);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2Float"/>
        public static Vector2 Dir2D(Vector2 basis, float withinDegrees) => Stream.Dir2D(basis, -withinDegrees, withinDegrees);
        /// <include file="Docs.xml" path="Doc/Dir"/>
        public static Vector3 Dir() => Stream.Dir(0, 360, 0, 180);
        /// <include file="Docs.xml" path="Doc/Dir/FloatFloatFloatFloat"/>
        public static Vector3 Dir(float thetaRangeX, float thetaRangeY, float phiRangeX, float phiRangeY) => Stream.Dir(thetaRangeX, thetaRangeY, phiRangeX, phiRangeY);
        /// <include file="Docs.xml" path="Doc/Dir/Vector2Vector2"/>
        public static Vector3 Dir(Vector2 thetaRange, Vector2 phiRange) => Stream.Dir(thetaRange.x, thetaRange.y, phiRange.x, phiRange.y);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3FloatFloat"/>
        public static Vector3 Dir(Vector3 basis, float minDegrees, float maxDegrees) => Stream.Dir(basis, minDegrees, maxDegrees);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3Vector2"/>
        public static Vector3 Dir(Vector3 basis, Vector2 degreesRange) => Stream.Dir(basis, degreesRange.x, degreesRange.y);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3Float"/>
        public static Vector3 Dir(Vector3 basis, float withinDegrees) => Stream.Dir(basis, withinDegrees);
        /// <include file="Docs.xml" path="Doc/Color"/>
        public static Color Color(Vector2 hueRange, Vector2 saturationRange, Vector2 valueRange) => Stream.Color(hueRange, saturationRange, valueRange);
        /// <include file="Docs.xml" path="Doc/Item/List"/>
        public static T Item<T>(IReadOnlyList<T> items) => Stream.Item(items);
        /// <include file="Docs.xml" path="Doc/Item/ListList"/>
        public static T Item<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights) => Stream.Item(items, weights);
        /// <include file="Docs.xml" path="Doc/Item/WeightedList"/>
        public static T Item<T>(WeightedList<T> weightedList) => Stream.Item(weightedList.Items, weightedList.Weights);
        /// <include file="Docs.xml" path="Doc/Items/WithReplacement/ListInt"/>
        public static T[] ItemsWithReplacement<T>(IReadOnlyList<T> items, int count) => Stream.ItemsWithReplacement(items, count);
        /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/ListInt"/>
        public static T[] ItemsWithoutReplacement<T>(IReadOnlyList<T> items, int count) => Stream.ItemsWithoutReplacement(items, count);
        /// <include file="Docs.xml" path="Doc/Items/WithReplacement/ListListInt"/>
        public static T[] ItemsWithReplacement<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count) => Stream.ItemsWithReplacement(items, weights, count);
        /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/ListListInt"/>
        public static T[] ItemsWithoutReplacement<T>(IReadOnlyList<T> items, IReadOnlyList<float> weights, int count) => Stream.ItemsWithReplacement(items, weights, count);
        /// <include file="Docs.xml" path="Doc/Items/WithReplacement/WeightedListInt"/>
        public static T[] ItemsWithReplacement<T>(WeightedList<T> weightedList, int count) => Stream.ItemsWithReplacement(weightedList.Items, weightedList.Weights, count);
        /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/WeightedListInt"/>
        public static T[] ItemsWithoutReplacement<T>(WeightedList<T> weightedList, int count) => Stream.ItemsWithoutReplacement(weightedList.Items, weightedList.Weights, count);
        /// <include file="Docs.xml" path="Doc/Items/Extract/ListInt"/>
        public static T[] ItemsExtract<T>(List<T> source, int count) => Stream.ItemsExtract(source, count);
        /// <include file="Docs.xml" path="Doc/Index"/>
        public static int Index<T>(IReadOnlyCollection<T> items) => Stream.Index(items);
        /// <include file="Docs.xml" path="Doc/Shuffle"/>
        public static void Shuffle<T>(IList<T> toShuffle) => Stream.Shuffle(toShuffle);
    }
}