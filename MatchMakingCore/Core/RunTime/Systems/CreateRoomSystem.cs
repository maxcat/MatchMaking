using System.Collections.Generic;

namespace MatchMakingCore
{
    public class CreateRoomSystem : ISystem
    {
        public CreateRoomSystem()
        {
        }

        public void Execute(Container container)
        {
            int playerPerTeam = container.MmConfig.PlayerPerTeam;
            var teamA = new List<int>(playerPerTeam);
            var teamB = new List<int>(playerPerTeam);

            int i = 0;
            var removeEntityIds = new List<int>(16);
            while(i < container.MmrComponentsCount - 1)
            {
                if(container.TryGetMmrWeightFromIndex(i, out long currentWeight) &&
                    container.TryGetMmrWeightFromIndex(i + 1, out long nextWeight))
                {
                    if(currentWeight - nextWeight < container.MmConfig.MaxDifferenceAllowed)
                    {
                        int currentEntityId = container.GetMMrEntityId(i);
                        int nextEntityId = container.GetMMrEntityId(i + 1);
                        if(container.TryGetPlayerInfoDatabaseKeyFromEntityId(currentEntityId, out int currentPlayerId) &&
                            container.TryGetPlayerInfoDatabaseKeyFromEntityId(nextEntityId, out int nextPlayerId))
                        {
                            teamA.Add(currentPlayerId);
                            teamB.Add(nextPlayerId);

                            if(teamA.Count == playerPerTeam)
                            {
                                int roomEntityId = container.CreateEntity();
                                container.AddRoomInfoComponent(roomEntityId, teamA.ToArray(), teamB.ToArray());

                                teamA = new List<int>(playerPerTeam);
                                teamB = new List<int>(playerPerTeam);


                                removeEntityIds.Add(currentEntityId);
                                removeEntityIds.Add(nextEntityId);
                            }
                            i += 2;
                            continue;
                        }
                    }
                }
                ++i;
            }

            foreach(int entityId in removeEntityIds)
            {
                container.RemoveMmrComponent(entityId);
                container.RemovePlayerInfoComponent(entityId);
                container.RemoveWaitingComponent(entityId);
            }

            removeEntityIds.Clear();
        }
    }
}
