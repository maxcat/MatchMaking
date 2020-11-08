/// <summary>
/// Should be code generated from PlayerInfoComponent
/// </summary>
using System.Collections.Generic;

namespace MatchMakingCore
{
    public partial class Container
    {
        private LxList<PlayerInfoComponent> _playerInfoComponents = new LxList<PlayerInfoComponent>();
        private LxList<int> _playerInfoEntityComponentMap = new LxList<int>();
        private Queue<PlayerInfoComponent> _playerInfoPool = new Queue<PlayerInfoComponent>(COMPONENT_POOL_START_SIZE);

        #region Pooling
        private PlayerInfoComponent GetPlayerInfoComponentFromPool()
        {
            PlayerInfoComponent result;
            if(_playerInfoPool.Count > 0)
            {
                result = _playerInfoPool.Dequeue();
            }
            else
            {
                result = new PlayerInfoComponent();
            }

            result.Reset();
            return result;
        }

        public void ReleasePlayerInfoComponent(PlayerInfoComponent com)
        {
            com.Reset();
            _playerInfoPool.Enqueue(com);
        }
        #endregion

        #region Help Functions
        public bool HasPlayerInfoComponent(int entityId)
        {
            if (_playerInfoEntityComponentMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_playerInfoComponents.ContainIndex(componentIndex))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemovePlayerInfoComponent(int entityId)
        {
            if(_playerInfoEntityComponentMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_playerInfoComponents.ContainIndex(componentIndex))
                {
                    ReleasePlayerInfoComponent(_playerInfoComponents.Get(componentIndex));

                    _playerInfoComponents.Remove(componentIndex);
                    _playerInfoEntityComponentMap.Set(entityId, Container.EMPTY_ID);
                }
            }
        }

        private bool TryGetPlayerInfoComponent(int entityId, out PlayerInfoComponent com)
        {
            if (_playerInfoEntityComponentMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_playerInfoComponents.ContainIndex(componentIndex))
                {
                    com = _playerInfoComponents.Get(componentIndex);
                    return true;
                }
            }

            com = null;
            return false;
        }


        public void AddPlayerInfoComponent(int entityId)
        {
            if (_playerInfoEntityComponentMap.TryGet(entityId, out int componentIndex) && componentIndex == Container.EMPTY_ID)
            {
                if (_playerInfoComponents.ContainIndex(componentIndex))
                {
                    return;
                }
                else
                {
                    _playerInfoEntityComponentMap.Set(entityId, _playerInfoComponents.Count);
                    _playerInfoComponents.Add(GetPlayerInfoComponentFromPool());
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
            if (_playerInfoComponents.ContainIndex(index))
            {
                databaseKey = _playerInfoComponents.Get(index).DatabaseKey;
                return true;
            }

            return false;
        }

        public bool TrySetPlayerInfoDatabaseKeyFromIndex(int index, int value)
        {
            if(_playerInfoComponents.ContainIndex(index))
            {
                _playerInfoComponents.Get(index).DatabaseKey = value;
                return true;
            }

            return false;
        }
        #endregion

    }

}

