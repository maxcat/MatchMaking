using NUnit.Framework;
using MatchMakingCore;

namespace MatchMaking.Tests
{
    public class TurnSystemTest
    {
        [Test]
        public void Test()
        {
            Container container = new Container();
            container.InitComponentComparers();

            container.MmConfig = MatchmakingTest.TestConfig;

            int count = 100;
            for(int i = 0; i < count; ++i)
            {
                int entityId = container.CreateEntity();
                container.AddWaitingComponent(entityId, 0);
            }

            var system = new TurnSystem();
            system.Execute(container);

            for(int i = 0; i < container.WaitingComponentsCount; ++i)
            {
                if(container.TryGetWaitingTurnFromIndex(i, out int turn))
                {
                    Assert.AreEqual(1, turn);
                }
            }
        }
    }
}
