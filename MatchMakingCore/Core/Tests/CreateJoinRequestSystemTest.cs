using NUnit.Framework;
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
            Assert.AreEqual(count, container.MmrComponentsCount);

            for(int i = 0; i < container.MmrComponentsCount; ++i)
            {
                if(container.TryGetMmrWeightFromIndex(i, out ulong weight))
                {
                    int entityId = container.GetMMrEntityId(i);
                    if(container.TryGetPlayerInfoDatabaseKeyFromEntityId(entityId, out int databaseKey))
                    {
                        PlayerData player = container.GetPlayerData(databaseKey);

                        LxLog.Log($"{player.Name} win {player.Wins} lose {player.Loses} weight {weight}");
                    }
                }
            }
        }
    }
}

