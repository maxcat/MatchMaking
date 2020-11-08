/// <summary>
/// Should be code generated from all components
/// </summary>
namespace MatchMakingCore
{
    public partial class Container
    {
        public static int EMPTY_ID = -1;

        private int _lastEntityId = EMPTY_ID;

        public void ResetEntitiyId()
        {
            _lastEntityId = EMPTY_ID;
        }

        public int CreateEntity()
        {
            ++_lastEntityId;
            // add to all components
            // TODO: need better data structure for entity component mapping.
            _playerInfoEntityMap.Add(EMPTY_ID);
            _mmrEntityMap.Add(EMPTY_ID);
            _roomInfoEntityMap.Add(EMPTY_ID);

            return _lastEntityId;
        }
    }
}
