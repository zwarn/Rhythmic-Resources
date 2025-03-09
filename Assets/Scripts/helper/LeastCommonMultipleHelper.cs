using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace helper
{
    public static class LeastCommonMultipleHelper
    {
        public static int LeastCommonMultiple(int a, int b)
        {
            return Math.Abs(a * b) / (int)BigInteger.GreatestCommonDivisor(a, b);
        }

        public static int FindSmallestCommonMultiple(List<int> durations)
        {
            return durations.Aggregate(LeastCommonMultiple);
        }
    }
}