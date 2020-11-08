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

        public int MmrComponentsCount => _mmrComponents.Count;

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
                    // TODO: need to figure out way of generate the unique flag from code.
                    _mmrComponents.Add(GetMmrComponentFromPool(), _mmrComponentComparer, false);
                }
            }
        }
        #endregion

        #region Field Access Functions
        public bool TryGetMmrRatioFromEntityId(int entityId, out ulong weight)
        {
            if (TryGetMmrComponent(entityId, out MmrComponent com))
            {
                weight = com.Weight;
                return true;
            }
            else
            {
                weight = 0;
                return false;
            }
        }

        public bool TrySetMmrDatabaseKeyFromEntityId(int entityId, ulong weight)
        {
            if (TryGetMmrComponent(entityId, out MmrComponent com))
            {
                com.Weight = weight;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetMmrDatabaseKeyFromIndex(int index, out ulong weight)
        {
            weight = 0;
            if (_mmrComponents.ContainIndex(index))
            {
                weight = _mmrComponents.Get(index).Weight;
                return true;
            }

            return false;
        }

        public bool TrySetMmrDatabaseKeyFromIndex(int index, ulong value)
        {
            if (_mmrComponents.ContainIndex(index))
            {
                _mmrComponents.Get(index).Weight = value;
                return true;
            }

            return false;
        }
        #endregion
    }
}