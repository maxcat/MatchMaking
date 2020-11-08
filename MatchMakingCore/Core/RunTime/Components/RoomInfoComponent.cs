using System;
namespace MatchMakingCore
{
    public class RoomInfoComponent : IComponent
    {
        public int[] TeamA; // database key
        public int[] TeamB; // database key

        public void Reset()
        {
            if(TeamA != null)
            {
                Array.Clear(TeamA, 0, TeamA.Length);
            }
            if(TeamB != null)
            {
                Array.Clear(TeamB, 0, TeamB.Length);
            }
        }
    }
}
