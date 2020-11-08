using System;
namespace MatchMakingCore
{
    public class MmrComponent : IComponent
    {
        public float Ratio = 0f;

        public void Reset()
        {
            Ratio = 0f;
        }
    }
}
