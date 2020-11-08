using Newtonsoft.Json;

namespace MatchMakingCore
{
    public class PlayerData
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("wins")] public ulong Wins;
        [JsonProperty("losses")] public ulong Loses;
        [JsonIgnore] public bool IsAvailable = true;

        public override string ToString()
        {
            return $"Name {Name} Wins {Wins} Loses {Loses}";
        }
    }
}