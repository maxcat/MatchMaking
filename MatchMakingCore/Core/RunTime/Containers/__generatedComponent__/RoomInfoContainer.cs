/// <summary>
/// Should be code generated from RoomInfoComponent
/// </summary>
using System.Collections.Generic;

namespace MatchMakingCore
{
    public partial class Container
    {
        private LxList<RoomInfoComponent> _roomInfoComponents = new LxList<RoomInfoComponent>();
        private LxList<int> _roomInfoEntityMap = new LxList<int>();
        private LxList<int> _entityRoomInfoMap = new LxList<int>();
        private Queue<RoomInfoComponent> _roomInfoPool = new Queue<RoomInfoComponent>(COMPONENT_POOL_START_SIZE);

        public int RoomInfoComponentsCount => _roomInfoComponents.Count;

        #region Pooling
        private RoomInfoComponent GetRoomInfoComponentFromPool()
        {
            RoomInfoComponent result;
            if (_roomInfoPool.Count > 0)
            {
                result = _roomInfoPool.Dequeue();
            }
            else
            {
                result = new RoomInfoComponent();
            }

            result.Reset();
            return result;
        }

        public void ReleaseRoomInfoComponent(RoomInfoComponent com)
        {
            com.Reset();
            _roomInfoPool.Enqueue(com);
        }
        #endregion

        #region Help Functions
        public bool HasRoomInfoComponent(int entityId)
        {
            if (_roomInfoEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_roomInfoComponents.ContainIndex(componentIndex))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveRoomInfoComponent(int entityId)
        {
            if (_roomInfoEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_roomInfoComponents.ContainIndex(componentIndex))
                {
                    ReleaseRoomInfoComponent(_roomInfoComponents.Get(componentIndex));

                    _roomInfoComponents.Remove(componentIndex);
                    _roomInfoEntityMap.Set(entityId, Container.EMPTY_ID);
                }
            }
        }

        private bool TryGetRoomInfoComponent(int entityId, out RoomInfoComponent com)
        {
            if (_roomInfoEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_roomInfoComponents.ContainIndex(componentIndex))
                {
                    com = _roomInfoComponents.Get(componentIndex);
                    return true;
                }
            }

            com = null;
            return false;
        }


        public void AddRoomInfoComponent(int entityId, int[] teamA, int[] teamB)
        {
            if (_roomInfoEntityMap.TryGet(entityId, out int componentIndex) && componentIndex == Container.EMPTY_ID)
            {
                if (_roomInfoComponents.ContainIndex(componentIndex))
                {
                    return;
                }
                else
                {
                    var com = GetRoomInfoComponentFromPool();
                    com.TeamA = teamA;
                    com.TeamB = teamB;

                    _roomInfoEntityMap.Set(entityId, _roomInfoComponents.Count);
                    _roomInfoComponents.Add(com);
                    _entityRoomInfoMap.Add(entityId);
                }
            }
        }

        public int GetRoomInfoEntityId(int index)
        {
            if (_entityRoomInfoMap.TryGet(index, out int result))
            {
                return result;
            }
            else
            {
                throw new LxException($"invalid index {index}");
            }
        }

        #endregion

        #region Field Access Functions
        public bool TryGetRoomInfoTeamAFromEntityId(int entityId, out int[] teamA)
        {
            if (TryGetRoomInfoComponent(entityId, out RoomInfoComponent com))
            {
                teamA = com.TeamA;
                return true;
            }
            else
            {
                teamA = null;
                return false;
            }
        }

        public bool TrySetRoomInfoTeamAFromEntityId(int entityId, int[] teamA)
        {
            if (TryGetRoomInfoComponent(entityId, out RoomInfoComponent com))
            {
                com.TeamA = teamA;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetRoomInfoTeamAFromIndex(int index, out int[] teamA)
        {
            teamA = null;
            if (_roomInfoComponents.ContainIndex(index))
            {
                teamA = _roomInfoComponents.Get(index).TeamA;
                return true;
            }

            return false;
        }

        public bool TrySetRoomInfoTeamAFromIndex(int index, int[] value)
        {
            if (_roomInfoComponents.ContainIndex(index))
            {
                _roomInfoComponents.Get(index).TeamA = value;
                return true;
            }

            return false;
        }

        public bool TryGetRoomInfoTeamBFromEntityId(int entityId, out int[] teamB)
        {
            if (TryGetRoomInfoComponent(entityId, out RoomInfoComponent com))
            {
                teamB = com.TeamB;
                return true;
            }
            else
            {
                teamB = null;
                return false;
            }
        }

        public bool TrySetRoomInfoTeamBFromEntityId(int entityId, int[] teamB)
        {
            if (TryGetRoomInfoComponent(entityId, out RoomInfoComponent com))
            {
                com.TeamB = teamB;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetRoomInfoTeamBFromIndex(int index, out int[] teamB)
        {
            teamB = null;
            if (_roomInfoComponents.ContainIndex(index))
            {
                teamB = _roomInfoComponents.Get(index).TeamB;
                return true;
            }

            return false;
        }

        public bool TrySetRoomInfoTeamBFromIndex(int index, int[] value)
        {
            if (_roomInfoComponents.ContainIndex(index))
            {
                _roomInfoComponents.Get(index).TeamB = value;
                return true;
            }

            return false;
        }
        #endregion

    }

}

