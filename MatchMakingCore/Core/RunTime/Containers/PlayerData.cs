using Newtonsoft.Json;

namespace MatchMakingCore
{
    public class PlayerData
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("wins")] public string Wins;
        [JsonProperty("loses")] public string Loses;

        public override string ToString()
        {
            return $"Name {Name} Wins {Wins} Loses {Loses}";
        }
    }
}