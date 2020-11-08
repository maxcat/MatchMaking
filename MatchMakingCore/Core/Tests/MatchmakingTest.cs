﻿using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine.TestTools;
using MatchMakingCore;

namespace MatchMaking.Tests
{
    public class MatchmakingTest
    {
        public static Stopwatch Measure(Action action, int iteration = 1)
        {
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < iteration; ++i)
            {
                action.Invoke();
            }
            watch.Stop();

            return watch;
        }

        [Test]
        public void Test()
        {
            LxLog.Init(new UnityLog());

            Container container = new Container();
            container.InitComponentComparers();
            container.InitPlayerDataBase();

            var createJoinRequestSystem = new CreateJoinRequestSystem();

            createJoinRequestSystem.Execute(container);

            int count = 0;
            for(int i = 0; i < container.PlayerDatabaseSize; ++i)
            {
                PlayerData playerData = container.GetPlayerData(i);
                if(!playerData.IsAvailable)
                {
                    ++count;
                }
            }

            LxLog.Log($"player info component count is {container.PlayerInfoComponentsCount} not availabe player count {count}");
            Assert.AreEqual(count, container.PlayerInfoComponentsCount);

        }

        [UnityTest]
        public IEnumerator MatchmakingTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}