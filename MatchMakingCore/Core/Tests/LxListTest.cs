using System.Collections.Generic;
using NUnit.Framework;
using System.Diagnostics;
using System;
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
        public void BenchmarkAssign()
        {
            int count = 128;
            int iteration = 10000;

            int[] array = new int[count];
            List<int> list = new List<int>(count);
            LxList<int> lxList = new LxList<int>(count);

            Stopwatch watch;
            // assign test
            watch = MatchmakingTest.Measure(() =>
            {
                for (int i = 0; i < count; ++i)
                {
                    array[i] = i;
                }
            }, iteration);
            Debug.Log($"array assign ticks {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                for (int i = 0; i < count; ++i)
                {
                    list.Add(i);
                }
                list.Clear();
            }, iteration); 
            Debug.Log($"list assign ticks {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                for (int i = 0; i < count; ++i)
                {
                    lxList.Add(i);
                }
                lxList.Reset();
            }, iteration);
            Debug.Log($"lx list assign ticks {watch.ElapsedMilliseconds}");
        }

        [Test]
        public void BenchmarkGet()
        {
            int count = 128;
            int iteration = 1000000;

            int[] array = new int[count];
            List<int> list = new List<int>(count);
            LxList<int> lxList = new LxList<int>(count);

            Stopwatch watch;
            int result;
            int index = 10;

            // assign test
            for (int i = 0; i < count; ++i)
            {
                array[i] = i;
                list.Add(i);
                lxList.Add(i);
            }

            // access test
            watch = MatchmakingTest.Measure(() =>
            {
                result = array[index];
            }, iteration);
            Debug.Log($"array access ticks {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                result = list[index];
            }, iteration);
            Debug.Log($"list access ticks {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                result = lxList.Get(index);
            }, iteration);
            Debug.Log($"lx list access ticks {watch.ElapsedMilliseconds}");
        }

        [Test]
        public void BenchmarkInsertHead()
        {
            int count = 128;
            int iteration = 10000;

            int[] array = new int[count];
            List<int> list = new List<int>(count);
            LxList<int> lxList = new LxList<int>(count);

            Stopwatch watch;

            // assign test
            for (int i = 0; i < count; ++i)
            {
                array[i] = i;
                list.Add(i);
                lxList.Add(i);
            }

            watch = MatchmakingTest.Measure(() =>
            {
                int[] newArray;
                // insert head test
                newArray = new int[array.Length + 1];
                newArray[0] = -1;
                Array.Copy(array, 0, newArray, 1, array.Length);
                array = newArray;
            }, iteration);
            Debug.Log($"array insert head {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                list.Insert(0, -1);
            }, iteration);
            Debug.Log($"list insert head {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                lxList.Insert(0, -1);
            }, iteration);
            Debug.Log($"lxList insert head {watch.ElapsedMilliseconds}");
        }

        [Test]
        public void BenchmarkInsertTail()
        {
            int count = 128;
            int iteration = 10000;

            int[] array = new int[count];
            List<int> list = new List<int>(count);
            LxList<int> lxList = new LxList<int>(count);

            Stopwatch watch = new Stopwatch();
            
            watch.Start();
            watch.Stop();

            // assign test
            for (int i = 0; i < count; ++i)
            {
                array[i] = i;
                list.Add(i);
                lxList.Add(i);
            }

            watch = MatchmakingTest.Measure(() =>
            {
                int[] newArray;
                newArray = new int[array.Length + 1];
                newArray[array.Length] = -1;
                Array.Copy(array, 0, newArray, 1, array.Length);
                array = newArray;
            }, iteration);
            Debug.Log($"array insert tail copy {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = -1;
            }, iteration);
            Debug.Log($"array insert tail resize {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                list.Add(-1);
            }, iteration);
            Debug.Log($"list insert tail {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                lxList.Add(-1);
            }, iteration);
            Debug.Log($"lxList insert tail {watch.ElapsedMilliseconds}");
        }

        [Test]
        public void BenchmarkInsertMiddle()
        {
            int count = 100;

            int[] array = new int[count];
            List<int> list = new List<int>(count);
            LxList<int> lxList = new LxList<int>(count);

            Stopwatch watch = new Stopwatch();

            watch.Start();
            watch.Stop();

            // assign test
            for (int i = 0; i < count; ++i)
            {
                array[i] = i;
                list.Add(i);
                lxList.Add(i);
            }

            int iteration = 10000;

            int[] newArray;
            // insert middle
            int middleIndex = count / 2;

            watch = MatchmakingTest.Measure(() =>
            {
                lxList.Insert(middleIndex, -1);
            }, iteration);
            Debug.Log($"lxlist insert middle ticks {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                newArray = new int[array.Length + 1];
                newArray[middleIndex] = -1;
                Array.Copy(array, 0, newArray, 0, middleIndex);
                Array.Copy(array, middleIndex, newArray, middleIndex + 1, array.Length - middleIndex);
                array = newArray;
            }, iteration);
            Debug.Log($"array insert middle {watch.ElapsedMilliseconds}");

            watch = MatchmakingTest.Measure(() =>
            {
                list.Insert(middleIndex, -1);
            }, iteration);
            Debug.Log($"list insert middle ticks {watch.ElapsedMilliseconds}");
        }
    }
}


