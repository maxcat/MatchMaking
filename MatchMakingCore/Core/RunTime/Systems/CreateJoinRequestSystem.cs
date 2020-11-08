using System;
using System.Collections.Generic;


namespace MatchMakingCore
{
    public class CreateJoinRequestSystem : ISystem
    {
        private Random _random;

        public CreateJoinRequestSystem()
        {
            _random = new Random();
        }

        public void Execute(Container container)
        {
            int count = _random.Next(1, container.PlayerDatabaseSize);
            LxLog.Log($"generate {count} requests");
            List<int> requestPlayers = container.GenerateJoinRequestKeys(count);

            for(int i = 0; i < requestPlayers.Count; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddPlayerInfoComponent(entityId);

                container.TrySetPlayerInfoDatabaseKeyFromEntityId(entityId, requestPlayers[i]);
                PlayerData playerData = container.GetPlayerData(requestPlayers[i]);

                playerData.IsAvailable = false;
            }
        }
    }
}