using System;
using System.Collections.Generic;


namespace MatchMakingCore
{
    public class CreateJoinRequestSystem : ISystem
    {
        private Random _random;
        private int _count = 0;

        public CreateJoinRequestSystem(int count = 0)
        {
            _random = new Random();
            _count = count;
        }

        public void Execute(Container container)
        {
            int count;
            if(_count > 0)
            {
                count = _count;
            }
            else
            {
                count = _random.Next(1, container.PlayerDatabaseSize);
            }

            LxLog.Log($"generate {count} requests");
            List<int> requestPlayers = container.GenerateJoinRequestKeys(count);

            for(int i = 0; i < requestPlayers.Count; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddPlayerInfoComponent(entityId, requestPlayers[i]);

                PlayerData playerData = container.GetPlayerData(requestPlayers[i]);

                long weight = GenerateMmrWeight(playerData, container.MmConfig);
                container.AddMmrComponent(entityId, weight);
                
                playerData.IsAvailable = false;
            }
        }

        private long GenerateMmrWeight(PlayerData data, MatchmakingConfig config)
        {
            long weight;
            weight = (long)data.Wins * config.WinWeight - (long)data.Loses * config.LoseWeight;

            return weight;
        }
    }
}