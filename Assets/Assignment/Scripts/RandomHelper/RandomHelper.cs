using System;
using System.Collections.Generic;

namespace Assignment
{
    public static class RandomHelper
    {
        private static readonly Random Random = new Random();

        public static T GetRandomElementFromList<T>(List<T> list)
        {
            int newIndex = Random.Next(0, list.Count - 1);
            return list[newIndex];
        }

        public static int GetRandomInt(int min, int max)
        {
            return Random.Next(min, max);
        }
    }
}