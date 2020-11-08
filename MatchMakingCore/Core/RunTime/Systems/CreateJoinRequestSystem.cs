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
            //int count = 10;
            LxLog.Log($"generate {count} requests");
            List<int> requestPlayers = container.GenerateJoinRequestKeys(count);

            for(int i = 0; i < requestPlayers.Count; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddPlayerInfoComponent(entityId, requestPlayers[i]);

                PlayerData playerData = container.GetPlayerData(requestPlayers[i]);

                ulong weight = GenerateMmrWeight(playerData, container.MmConfig);
                container.AddMmrComponent(entityId, weight);
                
                playerData.IsAvailable = false;
            }
        }

        private ulong GenerateMmrWeight(PlayerData data, MatchmakingConfig config)
        {
            ulong weight;
            ulong totalGames = data.Wins + data.Loses;
            weight = data.Wins > config.WinWeight ? config.WinWeight : data.Wins;
            if(totalGames > 0)
            {
                weight += (data.Wins * config.WinRatioWeight / totalGames) * config.WinWeight;
            }

            return weight;
        }
    }
}