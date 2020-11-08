namespace MatchMakingCore
{
    public class MatchmakingConfig 
    {
        public ulong WinRatioWeight;
        public ulong WinWeight;
        public ulong MaxDifferenceAllowed;

        public override string ToString()
        {
            return $"win ratio weight {WinRatioWeight} win weight {WinWeight} max difference allowed {MaxDifferenceAllowed}";
        }
    }
}
