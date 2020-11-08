using NUnit.Framework;
using MatchMakingCore;

namespace MatchMaking.Tests
{
    public class CreateJoinRequestSystemTest
    {
        public struct JoinRequestResult
        {
            public string Name;
            public long Weight;
        }

        private JoinRequestResult[] _result = new JoinRequestResult[]
        {
            new JoinRequestResult
            {
                Name = "player4",
                Weight = 961,
            },
            new JoinRequestResult
            {
                Name = "player1",
                Weight = 961,
            },
            new JoinRequestResult
            {
                Name = "player5",
                Weight = 440,
            },
            new JoinRequestResult
            {
                Name = "player3",
                Weight = 320,
            },
            new JoinRequestResult
            {
                Name = "player6",
                Weight = 0,
            },
            new JoinRequestResult
            {
                Name = "player2",
                Weight = -12,
            }
        };

        [Test]
        public void Test()
        {
            LxLog.Init(new UnityLog());

            Container container = new Container();
            container.InitComponentComparers();
            container.SetPlayerDatabase(MatchmakingTest.TestPlayers);
            container.MmConfig = MatchmakingTest.TestConfig;

            var createJoinRequestSystem = new CreateJoinRequestSystem(container.PlayerDatabaseSize);

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
                if(container.TryGetMmrWeightFromIndex(i, out long weight))
                {
                    int entityId = container.GetMMrEntityId(i);
                    if(container.TryGetPlayerInfoDatabaseKeyFromEntityId(entityId, out int databaseKey))
                    {
                        PlayerData player = container.GetPlayerData(databaseKey);

                        LxLog.Log($"{player.Name} win {player.Wins} lose {player.Loses} weight {weight}");
                        JoinRequestResult result = _result[i];
                        Assert.AreEqual(result.Name, player.Name);
                        Assert.AreEqual(result.Weight, weight);
                    }
                }
            }
        }
    }
}

