using System;
using System.IO;

namespace MatchMakingCore
{
    public class Container
    {
        public static readonly string PLAYER_DATA_PATH = "../../../Assets/PlayerData/sample-data.json";

        public Container()
        {
        }

        public void Test()
        {
            Console.WriteLine(File.Exists(PLAYER_DATA_PATH));

            Console.WriteLine(Directory.GetCurrentDirectory());
        }
    }
}
