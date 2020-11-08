using NUnit.Framework;
using MatchMakingCore;
using System.Text;
using UnityEngine;

namespace MatchMaking.Tests
{
    public class CreateRoomSystemTest
    {
        [Test]
        public void SuccessTest()
        {
            Container container = new Container();
            container.InitComponentComparers();

            container.MmConfig = MatchmakingTest.TestConfig;

            int roomCount = 4;
            for(int i = 0; i < roomCount * 2 * container.MmConfig.PlayerPerTeam; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddMmrComponent(entityId, i * (container.MmConfig.MaxDifferenceAllowed - 1));
                container.AddPlayerInfoComponent(entityId, i);
                container.AddWaitingComponent(entityId, 0);
            }

            CreateRoomSystem system = new CreateRoomSystem();
            system.Execute(container);

            StringBuilder builder = new StringBuilder();
            Assert.AreEqual(roomCount, container.RoomInfoComponentsCount);
            for(int i = 0; i < container.RoomInfoComponentsCount; ++i)
            {
                if(container.TryGetRoomInfoTeamAFromIndex(i, out int[] teamA) &&
                    container.TryGetRoomInfoTeamBFromIndex(i, out int[] teamB))
                {
                    builder.Append("Team A ");
                    for(int j = 0; j < teamA.Length; ++j)
                    {
                        builder.Append(teamA[j]);
                        builder.Append(" ");
                    }

                    builder.Append("Team B ");
                    for (int j = 0; j < teamB.Length; ++j)
                    {
                        builder.Append(teamB[j]);
                        builder.Append(" ");
                    }

                    Debug.Log(builder.ToString());
                    builder.Clear();
                }
            }
        }

        [Test]
        public void FirstAttemptFailTest()
        {
            Container container = new Container();
            container.InitComponentComparers();

            container.MmConfig = MatchmakingTest.TestConfig;

            int roomCount = 4;
            for (int i = 0; i < roomCount * 2 * container.MmConfig.PlayerPerTeam; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddMmrComponent(entityId, i * (container.MmConfig.MaxDifferenceAllowed));
                container.AddPlayerInfoComponent(entityId, i);
                container.AddWaitingComponent(entityId, 0);
            }

            CreateRoomSystem system = new CreateRoomSystem();
            TurnSystem turnSystem = new TurnSystem();
            system.Execute(container);

            Assert.AreEqual(0, container.RoomInfoComponentsCount);

            turnSystem.Execute(container);
            system.Execute(container);
            Assert.AreEqual(roomCount, container.RoomInfoComponentsCount);
        }
    }
}