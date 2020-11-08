/// <summary>
/// Should be code generated from MmrComponent
/// </summary>
using System.Collections.Generic;

namespace MatchMakingCore
{
    public partial class Container
    {
        private LxList<MmrComponent> _mmrComponents = new LxList<MmrComponent>();
        private LxList<int> _mmrEntityComponentMap = new LxList<int>();
        private Queue<MmrComponent> _mmrPool = new Queue<MmrComponent>(COMPONENT_POOL_START_SIZE);

        #region Pooling
        private MmrComponent GetMmrComponentFromPool()
        {
            MmrComponent result;
            if (_mmrPool.Count > 0)
            {
                result = _mmrPool.Dequeue();
            }
            else
            {
                result = new MmrComponent();
            }

            result.Reset();
            return result;
        }

        public void ReleaseMmrComponent(MmrComponent com)
        {
            com.Reset();
            _mmrPool.Enqueue(com);
        }
        #endregion

        #region Help Functions
        public bool HasMmrComponent(int entityId)
        {
            if (_mmrEntityComponentMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_mmrComponents.ContainIndex(componentIndex))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveMmrComponent(int entityId)
        {
            if (_mmrEntityComponentMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_mmrComponents.ContainIndex(componentIndex))
                {
                    ReleaseMmrComponent(_mmrComponents.Get(componentIndex));

                    _mmrComponents.Remove(componentIndex);
                    _mmrEntityComponentMap.Set(entityId, Container.EMPTY_ID);
                }
            }
        }

        private bool TryGetMmrComponent(int entityId, out MmrComponent com)
        {
            if (_mmrEntityComponentMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_mmrComponents.ContainIndex(componentIndex))
                {
                    com = _mmrComponents.Get(componentIndex);
                    return true;
                }
            }

            com = null;
            return false;
        }


        public void AddMmrComponent(int entityId)
        {
            if (_mmrEntityComponentMap.TryGet(entityId, out int componentIndex) && componentIndex == Container.EMPTY_ID)
            {
                if (_mmrComponents.ContainIndex(componentIndex))
                {
                    return;
                }
                else
                {
                    _mmrEntityComponentMap.Set(entityId, _mmrComponents.Count);
                    _mmrComponents.Add(GetMmrComponentFromPool());
                }
            }
        }
        #endregion

        #region Field Access Functions
        public bool TryGetMmrRatioFromEntityId(int entityId, out float ratio)
        {
            if (TryGetMmrComponent(entityId, out MmrComponent com))
            {
                ratio = com.Ratio;
                return true;
            }
            else
            {
                ratio = 0f;
                return false;
            }
        }

        public bool TrySetMmrDatabaseKeyFromEntityId(int entityId, float ratio)
        {
            if (TryGetMmrComponent(entityId, out MmrComponent com))
            {
                com.Ratio = ratio;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetMmrDatabaseKeyFromIndex(int index, out float ratio)
        {
            ratio = 0f;
            if (_mmrComponents.ContainIndex(index))
            {
                ratio = _mmrComponents.Get(index).Ratio;
                return true;
            }

            return false;
        }

        public bool TrySetMmrDatabaseKeyFromIndex(int index, float value)
        {
            if (_mmrComponents.ContainIndex(index))
            {
                _mmrComponents.Get(index).Ratio = value;
                return true;
            }

            return false;
        }
        #endregion
    }
}