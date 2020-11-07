using System;
using System.IO;
using Newtonsoft.Json;

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
            using (StreamReader stream = new StreamReader(PLAYER_DATA_PATH))
            {
                string str = stream.ReadToEnd();
                Console.WriteLine(str);

                var dataList = JsonConvert.DeserializeObject<PlayerData[]>(str);
                for(int i = 0; i < dataList.Length; ++i)
                {
                    Console.WriteLine(dataList[i].ToString());
                }
            }            
        }
    }
}
