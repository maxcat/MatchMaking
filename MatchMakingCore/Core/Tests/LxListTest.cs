using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.TestTools;
using MatchMakingCore;
using Debug = UnityEngine.Debug;

namespace MatchMaking.Tests
{
    public class LxListTest
    {
        private LxList<int> InitList(int count)
        {
            var list = new LxList<int>();
            for (int i = 0; i < count; ++i)
            {
                list.Add(i);
            }

            return list;
        }

        [Test]
        public void AddTest()
        {
            int count = 100;
            LxList<int> list = InitList(count);
            for (int i = 0; i < list.Count; ++i)
            {
                Assert.AreEqual(i, list.Get(i));
            }

            Assert.AreEqual(100, list.Count);

            for(int i = 0; i < count; ++i)
            {
                list.Add(i);
            }

            for (int i = 0; i < count; ++i)
            {
                Assert.AreEqual(i, list.Get(i));
            }

            for (int i = count; i < list.Count; ++i)
            {
                Assert.AreEqual(i - count, list.Get(i));
            }
        }

        [Test]
        public void SetTest()
        {
            int count = 100;
            LxList<int> list = InitList(count);
            Assert.Catch<LxException>(() => { list.Set(-1, -1); });
            Assert.Catch<LxException>(() => { list.Set(100, -1); });

            SetTest(0, -1, count);
            SetTest(count / 2, -1, count);
            SetTest(count - 1, -1, count);
        }

        private void SetTest(int setIndex, int setValue, int count)
        {
            LxList<int> list = InitList(count);
            list.Set(setIndex, setValue);
            for (int i = 0; i < list.Count; ++i)
            {
                int value = list.Get(i);
                if (i == setIndex)
                {
                    Assert.AreEqual(setValue, value);
                }
                else
                {
                    Assert.AreEqual(i, value);
                }
            }
        }

        [Test]
        public void RemoveTest()
        {
            int count = 100;
            var list = InitList(count);

            Assert.Catch<LxException>(() => { list.Remove(-1); });
            Assert.Catch<LxException>(() => { list.Remove(100); });

            list.Remove(0);
            for (int i = 0; i < list.Count; ++i)
            {
                Assert.AreEqual(i + 1, list.Get(i));
            }

            list = InitList(count);
            list.Remove(count - 1);
            for(int i = 0; i < list.Count; ++i)
            {
                Assert.AreEqual(i, list.Get(i));
            }

            list = InitList(count);
            int removeIndex = count / 2;
            list.Remove(removeIndex);
            for(int i = 0; i < list.Count; ++i)
            {
                if(i < removeIndex)
                {
                    Assert.AreEqual(i, list.Get(i));
                }
                else
                {
                    Assert.AreEqual(i + 1, list.Get(i));
                }
            }
        }

        private void InsertTest(int insertIndex, int insertValue, int count)
        {
            var list = InitList(count);
            list.Insert(insertIndex, insertValue);
            for (int i = 0; i < list.Count; ++i)
            {
                int value = list.Get(i);
                if (i < insertIndex)
                {
                    Assert.AreEqual(i, value);
                }
                else if (i == insertIndex)
                {
                    Assert.AreEqual(insertValue, value);
                }
                else
                {
                    Assert.AreEqual(i - 1, value);
                }
            }
        }

        [Test]
        public void InsertTest()
        {
            int count = 100;
            LxList<int> list = InitList(count);

            Assert.Catch<LxException>(() => { list.Insert(-1, -1); });
            Assert.Catch<LxException>(() => { list.Insert(100, -1); });

            list.Insert(0, -1);
            for(int i = 0; i < list.Count; ++i)
            {
                Assert.AreEqual(i - 1, list.Get(i));
            }

            InsertTest(0, -1, count);
            InsertTest(count - 1, -1, count);
            InsertTest(count / 2, -1, count);
        }

        [Test]
        public void BenchmarkTest()
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
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                for (int i = 0; i < count; ++i)
                {
                    array[i] = i;
                }
            }
            watch.Stop();
            Debug.Log($"array assign ticks {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                for (int i = 0; i < count; ++i)
                {
                    list.Add(i);
                }
            }
            watch.Stop();
            Debug.Log($"list assign ticks {watch.ElapsedMilliseconds}");

            // access test
            watch.Restart();
            for (int i = 0; i < iteration; ++i)
            {
                result = array[index];
            }
            watch.Stop();
            Debug.Log($"array access ticks {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int i = 0; i < iteration; ++i)
            {
                result = list[index];
            }
            watch.Stop();
            Debug.Log($"list access ticks {watch.ElapsedMilliseconds}");

            // resize test
            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                Array.Resize(ref array, array.Length + 1);
            }
            watch.Stop();
            Debug.Log($"array resize ticks {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                list.Capacity = list.Count + 1;
            }
            watch.Stop();
            Debug.Log($"list resize ticks {watch.ElapsedMilliseconds}");

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
            Debug.Log($"array insert head {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                list.Insert(0, -1);
            }
            watch.Stop();
            Debug.Log($"list insert head {watch.ElapsedMilliseconds}");

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
            Debug.Log($"array insert tail copy {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = -1;
            }
            watch.Stop();
            Debug.Log($"array insert tail resize {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                list.Add(-1);
            }
            watch.Stop();
            Debug.Log($"list insert tail {watch.ElapsedMilliseconds}");

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
            Debug.Log($"array insert middle {watch.ElapsedMilliseconds}");

            watch.Restart();
            for (int tmp = 0; tmp < iteration; ++tmp)
            {
                list.Insert(middleIndex, -1);
            }
            watch.Stop();
            Debug.Log($"list insert middle ticks {watch.ElapsedMilliseconds}");

        }
    }
}


