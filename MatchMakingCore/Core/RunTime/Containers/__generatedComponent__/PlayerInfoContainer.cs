/// <summary>
/// Should be code generated from PlayerInfoComponent
/// </summary>
using System.Collections.Generic;

namespace MatchMakingCore
{
    public partial class Container
    {
        private LxList<PlayerInfoComponent> _playerInfoComs = new LxList<PlayerInfoComponent>();
        private LxList<int> _playerInfoEntityComMap = new LxList<int>();
        private Queue<PlayerInfoComponent> _playerInfoPool = new Queue<PlayerInfoComponent>(COMPONENT_POOL_START_SIZE);

        #region Pooling
        private PlayerInfoComponent GetPlayerInfoComFromPool()
        {
            if(_playerInfoPool.Count > 0)
            {
                return _playerInfoPool.Dequeue();
            }
            else
            {
                return new PlayerInfoComponent();
            }
        }

        public void ReleasePlayerInfoCom(PlayerInfoComponent com)
        {
            _playerInfoPool.Enqueue(com);
        }
        #endregion

        #region Help Functions
        public bool HasPlayerInfoCom(int entityId)
        {
            if (_playerInfoEntityComMap.ContainIndex(entityId))
            {
                int componentIndex = _playerInfoEntityComMap.Get(entityId);

                if (_playerInfoComs.ContainIndex(componentIndex))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemovePlayerInfoCom(int entityId)
        {
            if (_playerInfoEntityComMap.ContainIndex(entityId))
            {
                int componentIndex = _playerInfoEntityComMap.Get(entityId);

                if (_playerInfoComs.ContainIndex(componentIndex))
                {
                    ReleasePlayerInfoCom(_playerInfoComs.Get(componentIndex));

                    _playerInfoComs.Remove(componentIndex);
                    _playerInfoEntityComMap.Remove(entityId);
                }
            }
        }

        private bool TryGetPlayerInfoComponent(int entityId, out PlayerInfoComponent com)
        {
            if (_playerInfoEntityComMap.ContainIndex(entityId))
            {
                int componentIndex = _playerInfoEntityComMap.Get(entityId);

                if (_playerInfoComs.ContainIndex(componentIndex))
                {
                    com = _playerInfoComs.Get(componentIndex);
                    return true;
                }
            }

            com = null;
            return false;
        }


        public void AddPlayerInfoCom(int entityId)
        {
            if (_playerInfoEntityComMap.ContainIndex(entityId))
            {
                int componentIndex = _playerInfoEntityComMap.Get(entityId);
                if (_playerInfoComs.ContainIndex(componentIndex))
                {
                    return;
                }
                else
                {
                    _playerInfoEntityComMap.Set(entityId, _playerInfoComs.Count);
                    _playerInfoComs.Add(GetPlayerInfoComFromPool());
                }
            }
        }
        #endregion

        #region Field Access Functions
        public bool TryGetPlayerInfoDatabaseKeyFromEntityId(int entityId, out int databaseKey)
        {
            if(TryGetPlayerInfoComponent(entityId, out PlayerInfoComponent com))
            {
                databaseKey = com.DatabaseKey;
                return true;
            }
            else
            {
                databaseKey = -1;
                return false;
            }
        }

        public bool TrySetPlayerInfoDatabaseKeyFromEntityId(int entityId, int databaseKey)
        {
            if (TryGetPlayerInfoComponent(entityId, out PlayerInfoComponent com))
            {
                com.DatabaseKey = databaseKey;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetPlayerInfoDatabaseKeyFromIndex(int index, out int databaseKey)
        {
            databaseKey = -1;
            if (_playerInfoComs.ContainIndex(index))
            {
                databaseKey = _playerInfoComs.Get(index).DatabaseKey;
                return true;
            }

            return false;
        }

        public bool TrySetPlayerInfoDatabaseKeyFromIndex(int index, int value)
        {
            if(_playerInfoComs.ContainIndex(index))
            {
                _playerInfoComs.Get(index).DatabaseKey = value;
                return true;
            }

            return false;
        }
        #endregion

    }

}

