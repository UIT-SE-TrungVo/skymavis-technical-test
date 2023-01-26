using System;
using System.Collections.Generic;

namespace Assignment
{
    public static class RandomHelper
    {
        private static readonly Random Random = new Random();

        public static T GetRandomElementFromList<T>(List<T> list)
        {
            int newIndex = Random.Next(0, list.Count);
            return list[newIndex];
        }

        public static int GetRandomInt(int min, int max)
        {
            return Random.Next(min, max);
        }

        public static float GetRandomFloat(float min, float max)
        {
            return (float)(Random.NextDouble() * (max - min) + min);
        }
    }
}