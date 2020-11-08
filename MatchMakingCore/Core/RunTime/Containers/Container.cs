using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatchMakingCore
{
    public partial class Container
    {
        private const int COMPONENT_POOL_START_SIZE = 32;
        public static readonly string PLAYER_DATA_PATH = "../../../Assets/PlayerData/sample-data.json";
        public static readonly string UNITY_DATA_PATH = "Assets/PlayerData/sample-data.json";

        public static readonly string MM_CONFIG_PATH = "../../../Assets/PlayerData/matchmakin-config.json";
        public static readonly string UNITY_MM_CONFIG_PATH = "Assets/PlayerData/matchmakin-config.json";

        private PlayerData[] _playerDataBase;
        public int PlayerDatabaseSize => _playerDataBase.Length;
        public MatchmakingConfig MmConfig;

        public Container()
        {
        }

        public void InitPlayerDataBase()
        {
            string path;
#if UNITY
            path = UNITY_DATA_PATH;
#else
            path = MM_CONFIG_PATH;
#endif
            using (StreamReader stream = new StreamReader(path))
            {
                string str = stream.ReadToEnd();
                _playerDataBase = JsonConvert.DeserializeObject<PlayerData[]>(str);
            }
        }

        public void SetPlayerDatabase(PlayerData[] players)
        {
            _playerDataBase = players;
        }

        public void InitMatchmakingCofig()
        {
            string path;
#if UNITY
            path = UNITY_MM_CONFIG_PATH;
#else
            path = PLAYER_DATA_PATH;
#endif
            using (StreamReader stream = new StreamReader(path))
            {
                string str = stream.ReadToEnd();
                MmConfig = JsonConvert.DeserializeObject<MatchmakingConfig>(str);
            }
        }



        public PlayerData GetPlayerData(int index)
        {
            if(index < _playerDataBase.Length && index >= 0)
            {
                return _playerDataBase[index];
            }
            else
            {
                throw new LxException($"can not get player data with index {index}");
            }
        }

        public List<int> GenerateJoinRequestKeys(int count)
        {
            if(count <= 0 || count > _playerDataBase.Length)
            {

                throw new LxException($"invalid request count {count} since total player count is {_playerDataBase.Length}");
            }

            var result = new List<int>(count);
            for(int i = 0; i < _playerDataBase.Length; ++i)
            {
                if(result.Count >= count)
                {
                    break;
                }

                var playerData = _playerDataBase[i];
                if(playerData.IsAvailable)
                {
                    result.Add(i);
                    playerData.IsAvailable = false;
                }
            }

            return result;
        }
    }
}
