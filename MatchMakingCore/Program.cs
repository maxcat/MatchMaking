using System;
using System.Text;

namespace MatchMakingCore
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var container = new Container();
            container.InitComponentComparers();
            container.InitMatchmakingCofig();
            container.InitPlayerDataBase();

            var createJoinRequestSystem = new CreateJoinRequestSystem(container.PlayerDatabaseSize);
            var turnSystem = new TurnSystem();
            var createRoomSystem = new CreateRoomSystem();

            createJoinRequestSystem.Execute(container);
            int roomStartIndex = 0;

            bool running = true;
            while(running)
            {
                Console.WriteLine("enter next to find more, enter quit to quit:  ");
                string input = Console.ReadLine();
                if (input == "next" || input == "n")
                {
                    createRoomSystem.Execute(container);
                    turnSystem.Execute(container);

                    var builder = new StringBuilder();
                    for (int i = roomStartIndex; i < container.RoomInfoComponentsCount; ++i)
                    {
                        if (container.TryGetRoomInfoTeamAFromIndex(i, out int[] teamA) &&
                            container.TryGetRoomInfoTeamBFromIndex(i, out int[] teamB))
                        {
                            int entityId = container.GetRoomInfoEntityId(i);
                            builder.Append($"room {entityId} team A ");
                            foreach (int key in teamA)
                            {
                                PlayerData data = container.GetPlayerData(key);
                                builder.Append($"{data.Name}; ");
                            }

                            builder.Append("team B ");
                            foreach (int key in teamB)
                            {
                                PlayerData data = container.GetPlayerData(key);
                                builder.Append($"{data.Name}; ");
                            }
                        }
                        Console.WriteLine(builder.ToString());
                        builder.Clear();
                    }

                    roomStartIndex = container.RoomInfoComponentsCount;
                }
                else if(input == "quit" || input == "q")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine($"invalid input {input}");
                }
            }
        }
    }
}
