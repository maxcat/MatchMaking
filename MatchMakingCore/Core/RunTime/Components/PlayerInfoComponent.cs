namespace MatchMakingCore
{
    public class PlayerInfoComponent : IComponent
    {
        public int DatabaseKey;

        public void Reset()
        {
            DatabaseKey = Container.EMPTY_ID;
        }
    }
}
