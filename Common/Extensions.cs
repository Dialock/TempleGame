using System;
using System.Collections.Generic;
using System.Linq;
//using System.Xml.Linq;

using Microsoft.Xna.Framework;

namespace TempleGardens
{
    public static class Extensions
    {
        //public static IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
        //{
        //    Random rand = new Random();
        //    List<TValue> values = Enumerable.ToList(dict.Values);
        //    int size = dict.Count;
        //    while (true)
        //    {
        //        yield return values[rand.Next(size)];
        //    }
        //}

        //public static T PickRandom<T>(this IEnumerable<T> source)
        //{
        //    return source.PickRandom(1).Single();
        //}

        public static T PickOne<T>(this IEnumerable<T> source, int count)
        {
            return  source.Shuffle().Take(count).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => MasterRandom.FRandom.Next());
        }

        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Point ToPoint(this Vector2 vector2)
        {
            return new Point((int)vector2.X, (int)vector2.Y);
        }

        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (var row =0; row <map.GetLength(0);row++)
                for (var col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
        }

        //public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        //{
        //    return new HashSet<T>(source);
        //}

//        public static string GetAttributeValue(this XElement element, XName name)
//        {
//            var attribute = element.Attribute(name);
//            return attribute != null ? attribute.Value : null;
//        }

        //public static int MaxIndex<T>(this IEnumerable<T> sequence)
        //    where T : IComparable<T>
        //{
        //    int maxIndex = -1;
        //    T maxValue = default(T);

        //    int index = 0;
        //    foreach (T value in sequence)
        //    {
        //        if (value.CompareTo(maxValue) > 0 || maxIndex == -1)
        //        {
        //            maxIndex = index;
        //            maxValue = value;
        //        }
        //        index++;
        //    }
        //    return maxIndex;
        //}

        //public static bool ContainsAny(this string str, params string[] values)
        //{
        //    if (!string.IsNullOrEmpty(str) || values.Length > 0)
        //    {
        //        foreach (string value in values)
        //        {
        //            if (str.Contains(value))
        //                return true;
        //        }
        //    }
        //    return false;
        //}

        public static bool NearlyEqual(float a, float b)
        {
            var epsilon = 0.00001f;

            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var difference = Math.Abs(a - b);

            if (a == b)
                return true;
            else if (a == 0 || b == 0 || difference < float.MinValue)
                return difference < (epsilon * float.MinValue);
            else
                return difference / (absA + absB) < epsilon;
        }

        //public static float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed)
        //{
        //    float x = faceThis.X - position.X;
        //    float y = faceThis.Y - position.Y;

        //    float desiredAngle = (float)Math.Atan2(y, x);

        //    float difference = MathHelper.WrapAngle(desiredAngle - currentAngle);

        //    difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

        //    return MathHelper.WrapAngle(currentAngle + difference);
        //}

    }
}
