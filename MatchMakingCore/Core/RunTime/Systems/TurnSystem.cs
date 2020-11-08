namespace MatchMakingCore
{
    public class TurnSystem : ISystem
    {
        public void Execute(Container container)
        {
            for(int i = 0; i < container.WaitingComponentsCount; ++i)
            {
                if(container.TryGetWaitingTurnFromIndex(i, out int turn))
                {
                    container.TrySetWaitingTurnFromIndex(i, turn + 1);
                }
            }
        }
    }
}


