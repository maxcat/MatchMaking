﻿namespace MatchMakingCore
{
    public class MatchmakingConfig 
    {
        public int PlayerPerTeam;
        public long LoseWeight;
        public long WinWeight;
        public long MaxDifferenceAllowed;
        public long WaitBonusWeight;

        public override string ToString()
        {
            return $"lose weight {LoseWeight} win weight {WinWeight} max difference allowed {MaxDifferenceAllowed} wait bonus weight {WaitBonusWeight} player per team {PlayerPerTeam}";
        }
    }
}
