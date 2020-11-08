using System;
using System.Diagnostics;

namespace MatchMakingCore
{
    public static class Extensions
    {
        public static T[] Insert<T>(this T[] array, int index, T value)
        {
            var newArray = new T[array.Length + 1];
            if (index == 0)
            {
                Array.Copy(array, 0, newArray, 1, array.Length);
                newArray[0] = value;
            }
            else
            {
                newArray[index] = value;
                Array.Copy(array, 0, newArray, 0, index);
                Array.Copy(array, index, newArray, index + 1, array.Length - index);
                
            }
            return newArray;
        }
    }
}

