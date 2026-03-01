using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LeafRand
{
    /// <summary>
    /// Global RNG class with high-level helper methods.<br></br> 
    /// </summary>
    public static class Rand
    {   // Just a BurstRand wrapped in a static class
        // This allows for calls like Rand.Num() for global access rather than something like Rand.Global.Num()
        #region Core
        static Instanced.BurstRand state = new((uint)Environment.TickCount);

        /// <include file="Docs.xml" path="Doc/Seed"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSeed(uint seed) => state.SetSeed(seed);
        #endregion
        #region Wrapped
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UInt() => state.UInt();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UInt(uint max) => state.UInt(max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UInt(uint min, uint max) => state.UInt(min, max);

        /// <include file="Docs.xml" path="Doc/Num/Int"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Int() => state.Int();
        /// <include file="Docs.xml" path="Doc/Num/Int"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Int(int max) => state.Int(max);
        /// <include file="Docs.xml" path="Doc/Num/IntInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Int(int min, int max) => state.Int(min, max);
        /// <include file="Docs.xml" path="Doc/Num/Vector2Int"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Int(Vector2Int range) => state.Int(range.x, range.y);

        /// <include file="Docs.xml" path="Doc/Num"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Float() => state.Float();
        /// <include file="Docs.xml" path="Doc/Num/Float"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Float(float max) => state.Float(max);
        /// <include file="Docs.xml" path="Doc/Num/FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Float(float min, float max) => state.Float(min, max);
        /// <include file="Docs.xml" path="Doc/Num/Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Float(Vector2 range) => state.Float(range.x, range.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Double() => state.Double();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Double(double max) => state.Double(max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Double(double min, double max) => state.Double(min, max);
        /// <include file="Docs.xml" path="Doc/Angle"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle() => state.Angle();


        /// <include file="Docs.xml" path="Doc/Bool"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Bool() => state.Bool();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BoolFloat(float chance) => state.BoolFloat(chance);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BoolDouble(double chance) => state.BoolDouble(chance);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BoolUInt(float chance) => state.BoolUInt(chance);


        /// <include file="Docs.xml" path="Doc/Bool"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Bool(float successChance) => state.Bool(successChance);
        /// <include file="Docs.xml" path="Doc/Chance"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(float successChance) => state.Bool(successChance);
        /// <include file="Docs.xml" path="Doc/Sign"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign() => state.Sign();
        /// <include file="Docs.xml" path="Doc/Dir2D"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Dir2D() => state.Dir2D();
        /// <include file="Docs.xml" path="Doc/Dir2D/FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Dir2D(float minTheta, float maxTheta) => state.Dir2D(minTheta, maxTheta);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Dir2D(Vector2 thetaRange) => state.Dir2D(thetaRange.x, thetaRange.y);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Dir2D(Vector2 basis, float minDegrees, float maxDegrees) => state.Dir2D(basis, minDegrees, maxDegrees);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Dir2D(Vector2 basis, Vector2 degreesRange) => state.Dir2D(basis, degreesRange.x, degreesRange.y);
        /// <include file="Docs.xml" path="Doc/Dir2D/Vector2Float"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Dir2D(Vector2 basis, float withinDegrees) => state.Dir2D(basis, -withinDegrees, withinDegrees);
        /// <include file="Docs.xml" path="Doc/Dir"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Dir() => state.Dir(0, 360, 0, 180);
        /// <include file="Docs.xml" path="Doc/Dir/FloatFloatFloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Dir(float thetaRangeX, float thetaRangeY, float phiRangeX, float phiRangeY) => state.Dir(thetaRangeX, thetaRangeY, phiRangeX, phiRangeY);
        /// <include file="Docs.xml" path="Doc/Dir/Vector2Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Dir(Vector2 thetaRange, Vector2 phiRange) => state.Dir(thetaRange.x, thetaRange.y, phiRange.x, phiRange.y);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3FloatFloat"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Dir(Vector3 basis, float minDegrees, float maxDegrees) => state.Dir(basis, minDegrees, maxDegrees);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3Vector2"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Dir(Vector3 basis, Vector2 degreesRange) => state.Dir(basis, degreesRange.x, degreesRange.y);
        /// <include file="Docs.xml" path="Doc/Dir/Vector3Float"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Dir(Vector3 basis, float withinDegrees) => state.Dir(basis, withinDegrees);
        /// <include file="Docs.xml" path="Doc/Color"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Color(Vector2 hueRange, Vector2 saturationRange, Vector2 valueRange) => state.Color(hueRange, saturationRange, valueRange);
        /// <include file="Docs.xml" path="Doc/Item/List"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Item<T>(IReadOnlyList<T> items) => state.Item(items);
        /// <include file="Docs.xml" path="Doc/Item/ListList"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Item<T>(IReadOnlyList<Weighted<T>> weightedItems) => state.Item(weightedItems);
        /// <include file="Docs.xml" path="Doc/Items/WithReplacement/ListInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ItemsWithReplacement<T>(IReadOnlyList<T> items, int count) => state.ItemsWithReplacement(items, count);
        /// <include file="Docs.xml" path="Doc/Items/WithoutReplacement/ListInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ItemsWithoutReplacement<T>(IReadOnlyList<T> items, int count) => state.ItemsWithoutReplacement(items, count);
        /// <include file="../Docs.xml" path="Doc/Items/WithReplacement/ListListInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ItemsWithReplacement<T>(IReadOnlyList<Weighted<T>> weightedItems, int count) => state.ItemsWithReplacement(weightedItems, count);
        /// <include file="../Docs.xml" path="Doc/Items/WithoutReplacement/ListListInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ItemsWithoutReplacement<T>(IReadOnlyList<Weighted<T>> weightedItems, int count) => state.ItemsWithReplacement(weightedItems, count);
        /// <include file="Docs.xml" path="Doc/Items/Extract/ListInt"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ItemsExtract<T>(List<T> source, int count) => state.ItemsExtract(source, count);
        /// <include file="Docs.xml" path="Doc/Index"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Index<T>(IReadOnlyCollection<T> items) => state.Index(items);
        /// <include file="Docs.xml" path="Doc/Shuffle"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(IList<T> items) => state.Shuffle(items);
        #endregion
    }
}