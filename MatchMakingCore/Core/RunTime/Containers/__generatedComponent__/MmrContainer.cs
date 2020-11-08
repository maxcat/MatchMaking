/// <summary>
/// Should be code generated from MmrComponent
/// </summary>
using System.Collections.Generic;

namespace MatchMakingCore
{
    public partial class Container
    {
        private LxList<MmrComponent> _mmrComponents = new LxList<MmrComponent>();
        private LxList<int> _mmrEntityMap = new LxList<int>();
        private LxList<int> _entityMmrMap = new LxList<int>();
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
            if (_mmrEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
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
            if (_mmrEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_mmrComponents.ContainIndex(componentIndex))
                {
                    ReleaseMmrComponent(_mmrComponents.Get(componentIndex));

                    _mmrComponents.Remove(componentIndex);
                    _mmrEntityMap.Set(entityId, Container.EMPTY_ID);
                }
            }
        }

        private bool TryGetMmrComponent(int entityId, out MmrComponent com)
        {
            if (_mmrEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
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


        public void AddMmrComponent(int entityId, long weight)
        {
            if (_mmrEntityMap.TryGet(entityId, out int componentIndex) && componentIndex == Container.EMPTY_ID)
            {
                if (_mmrComponents.ContainIndex(componentIndex))
                {
                    return;
                }
                else
                {
                    var com = GetMmrComponentFromPool();
                    com.Weight = weight;
                    // TODO: need to figure out way of generate the unique flag from code.
                    int insertIndex = _mmrComponents.Add(com, _mmrComponentComparer, false);
                    if(insertIndex >= 0)
                    {
                        _entityMmrMap.Insert(insertIndex, entityId);
                    }

                    // TODO: optimise the entity to component map
                    for(int i = 0; i < _entityMmrMap.Count; ++i)
                    {
                        _mmrEntityMap.Set(_entityMmrMap.Get(i), i);
                    }
                }
            }
        }

        public int GetMMrEntityId(int index)
        {
            if (_entityMmrMap.TryGet(index, out int result))
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
        public bool TryGetMmrWeightFromEntityId(int entityId, out long weight)
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

        public bool TrySetMmrWeightFromEntityId(int entityId, long weight)
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

        public bool TryGetMmrWeightFromIndex(int index, out long weight)
        {
            weight = 0;
            if (_mmrComponents.ContainIndex(index))
            {
                weight = _mmrComponents.Get(index).Weight;
                return true;
            }

            return false;
        }

        public bool TrySetMmrWeightFromIndex(int index, long value)
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