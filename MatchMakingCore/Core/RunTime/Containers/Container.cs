using System.IO;
using Newtonsoft.Json;

namespace MatchMakingCore
{
    public partial class Container
    {
        private const int COMPONENT_POOL_START_SIZE = 32;
        public static readonly string PLAYER_DATA_PATH = "../../../Assets/PlayerData/sample-data.json";

        private PlayerData[] _playerDataBase;
        public Container()
        {
        }

        public void InitPlayerDataBase()
        {
            using (StreamReader stream = new StreamReader(PLAYER_DATA_PATH))
            {
                string str = stream.ReadToEnd();
                _playerDataBase = JsonConvert.DeserializeObject<PlayerData[]>(str);
            }
        }

        #region Entity
        
        
        #endregion

    }
}
