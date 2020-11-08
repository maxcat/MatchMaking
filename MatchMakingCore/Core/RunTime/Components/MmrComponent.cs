using System;
using System.Collections.Generic;

namespace MatchMakingCore
{
    public class MmrComponent : IComponent, IComparable<MmrComponent>
    {
        public float Ratio = 0f;
        public int CompareTo(MmrComponent other)
        {
            return Ratio.CompareTo(other.Ratio);
        }

        public void Reset()
        {
            Ratio = 0f;
        }
    }

    public class MmrComponentComparer : IComparer<MmrComponent>
    {
        public int Compare(MmrComponent x, MmrComponent y)
        {
            return y.Ratio.CompareTo(x.Ratio);
        }
    }
}
