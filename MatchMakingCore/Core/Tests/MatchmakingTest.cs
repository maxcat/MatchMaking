using System;
using System.Diagnostics;
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

        public static PlayerData[] TestPlayers = new PlayerData[]
        {
            new PlayerData
            {
                Name = "player1",
                Wins = 100,
                Loses = 39
            },
            new PlayerData
            {
                Name = "player2",
                Wins = 0,
                Loses = 12
            },
            new PlayerData
            {
                Name = "player3",
                Wins = 32,
                Loses = 0
            },
            new PlayerData
            {
                Name = "player4",
                Wins = 100,
                Loses = 39
            },
            new PlayerData
            {
                Name = "player5",
                Wins = 44,
                Loses = 0
            },
            new PlayerData
            {
                Name = "player6",
                Wins = 0,
                Loses = 0
            },
        };

        public static MatchmakingConfig TestConfig = new MatchmakingConfig
        {
            PlayerPerTeam = 3,
            LoseWeight = 1,
            WinWeight = 10,
            MaxDifferenceAllowed = 30,
            WaitBonusWeight = 2
        };
    }
}