/// <summary>
/// Should be code generated from all components
/// </summary>
namespace MatchMakingCore
{
    public partial class Container
    {
        private MmrComponentComparer _mmrComponentComparer;

        public void InitComponentComparers()
        {
            _mmrComponentComparer = new MmrComponentComparer();
        }
    }
}
