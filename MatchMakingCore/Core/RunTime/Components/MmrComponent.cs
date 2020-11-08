using System;
using System.Collections.Generic;

namespace MatchMakingCore
{
    public class MmrComponent : IComponent, IComparable<MmrComponent>
    {
        public long Weight = 0;
        public int CompareTo(MmrComponent other)
        {
            if (other == null)
            {
                return 1;
            }

            return Weight.CompareTo(other.Weight);
        }

        public void Reset()
        {
            Weight = 0;
        }
    }

    public class MmrComponentComparer : IComparer<MmrComponent>
    {
        public int Compare(MmrComponent x, MmrComponent y)
        {
            return y.Weight.CompareTo(x.Weight);
        }
    }
}
