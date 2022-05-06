using System;
using System.Collections.Generic;



namespace VadenStock.Tools
{
    public static class Arr
    {
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            if (end < 0)
            {
                end = source.Length + end;
            }

            int len = end - start;

            T[] res = new T[len];

            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }

            return res;
        }
    }
}
