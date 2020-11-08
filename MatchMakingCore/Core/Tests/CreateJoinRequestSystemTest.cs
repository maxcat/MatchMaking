﻿using NUnit.Framework;
using MatchMakingCore;

namespace MatchMaking.Tests
{
    public class CreateJoinRequestSystemTest
    {
        [Test]
        public void Test()
        {
            LxLog.Init(new UnityLog());

            Container container = new Container();
            container.InitComponentComparers();
            container.InitPlayerDataBase();
            container.InitMatchmakingCofig();

            var createJoinRequestSystem = new CreateJoinRequestSystem();

            createJoinRequestSystem.Execute(container);

            LxLog.Log(container.MmConfig.ToString());

            int count = 0;
            for (int i = 0; i < container.PlayerDatabaseSize; ++i)
            {
                PlayerData playerData = container.GetPlayerData(i);
                if (!playerData.IsAvailable)
                {
                    ++count;
                }
            }

            LxLog.Log($"player info component count is {container.PlayerInfoComponentsCount} not availabe player count {count}");
            Assert.AreEqual(count, container.PlayerInfoComponentsCount);

        }
    }
}
