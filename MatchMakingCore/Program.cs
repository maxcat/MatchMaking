using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MatchMakingCore
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            uint tmp = 1;
            for(int i = 0; i < 10; ++i)
            {
                Console.WriteLine(tmp);
                tmp = tmp << 1;
            }
        }

        private static void Test()
        {
            int count = 128;
            int iteration = 10000;

            int[] array = new int[count];
            List<int> list = new List<int>(count);

            Stopwatch watch = new Stopwatch();
            int result;
            int index = 10;

            watch.Start();
            watch.Stop();

            // assign test
            watch.Restart();
            for(int tmp = 0; tmp < iteration; ++tmp)
            {
                for (int i = 0; i < count; ++i)
                {
                    array[i] = i;
                }
            }
            watch.Stop();
            Console.WriteLine($"array assign ticks {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                for (int i = 0; i < count; ++i)
                {
                    list.Add(i);
                }
            }
            watch.Stop();
            Console.WriteLine($"list assign ticks {watch.ElapsedMilliseconds}");

            // access test
            watch.Restart();
            for (int i = 0; i < iteration; ++i)
            {
                result = array[index];
            }
            watch.Stop();
            Console.WriteLine($"array access ticks {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int i = 0; i < iteration; ++i)
            {
                result = list[index];
            }
            watch.Stop();
            Console.WriteLine($"list access ticks {watch.ElapsedMilliseconds}");

            // resize test
            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                Array.Resize(ref array, array.Length + 1);
            }
            watch.Stop();
            Console.WriteLine($"array resize ticks {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                list.Capacity = list.Count + 1;
            }
            watch.Stop();
            Console.WriteLine($"list resize ticks {watch.ElapsedMilliseconds}");

            int[] newArray;
            // insert head test
            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                newArray = new int[array.Length + 1];
                newArray[0] = -1;
                Array.Copy(array, 0, newArray, 1, array.Length);
                array = newArray;
            }
            watch.Stop();
            Console.WriteLine($"array insert head {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                list.Insert(0, -1);
            }
            watch.Stop();
            Console.WriteLine($"list insert head {watch.ElapsedMilliseconds}");

            // insert tail test
            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                newArray = new int[array.Length + 1];
                newArray[array.Length] = -1;
                Array.Copy(array, 0, newArray, 1, array.Length);
                array = newArray;
            }
            watch.Stop();
            Console.WriteLine($"array insert tail copy {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = -1;
            }
            watch.Stop();
            Console.WriteLine($"array insert tail resize {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                list.Add(-1);
            }
            watch.Stop();
            Console.WriteLine($"list insert tail {watch.ElapsedMilliseconds}");

            // insert middle
            int middleIndex = array.Length / 2;

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                newArray = new int[array.Length + 1];
                newArray[middleIndex] = -1;
                Array.Copy(array, 0, newArray, 0, middleIndex);
                Array.Copy(array, middleIndex, newArray, middleIndex + 1, array.Length - middleIndex);
                array = newArray;
            }
            watch.Stop();
            Console.WriteLine($"array insert middle {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                list.Insert(middleIndex, -1);
            }
            watch.Stop();
            Console.WriteLine($"list insert middle ticks {watch.ElapsedMilliseconds}");
        }
    }
}
