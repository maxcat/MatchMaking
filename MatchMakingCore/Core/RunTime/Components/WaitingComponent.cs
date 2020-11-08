namespace MatchMakingCore
{
    public class WaitingComponent : IComponent
    {
        public int Turn;

        public void Reset()
        {
            Turn = 0;
        }
    }
}