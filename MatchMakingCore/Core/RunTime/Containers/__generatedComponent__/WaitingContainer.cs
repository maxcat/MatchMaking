/// <summary>
/// Should be code generated from WaitingComponent
/// </summary>
using System.Collections.Generic;

namespace MatchMakingCore
{
    public partial class Container
    {
        private LxList<WaitingComponent> _waitingComponents = new LxList<WaitingComponent>();
        private LxList<int> _waitingEntityMap = new LxList<int>();
        private LxList<int> _entityWaitingMap = new LxList<int>();
        private Queue<WaitingComponent> _waitingPool = new Queue<WaitingComponent>(COMPONENT_POOL_START_SIZE);

        public int WaitingComponentsCount => _waitingComponents.Count;

        #region Pooling
        private WaitingComponent GetWaitingComponentFromPool()
        {
            WaitingComponent result;
            if (_waitingPool.Count > 0)
            {
                result = _waitingPool.Dequeue();
            }
            else
            {
                result = new WaitingComponent();
            }

            result.Reset();
            return result;
        }

        public void ReleaseWaitingComponent(WaitingComponent com)
        {
            com.Reset();
            _waitingPool.Enqueue(com);
        }
        #endregion

        #region Help Functions
        public bool HasWaitingComponent(int entityId)
        {
            if (_waitingEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_waitingComponents.ContainIndex(componentIndex))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveWaitingComponent(int entityId)
        {
            if (_waitingEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_waitingComponents.ContainIndex(componentIndex))
                {
                    ReleaseWaitingComponent(_waitingComponents.Get(componentIndex));

                    _waitingComponents.Remove(componentIndex);
                    _waitingEntityMap.Set(entityId, Container.EMPTY_ID);
                    _entityWaitingMap.Remove(componentIndex);
                }
            }
        }

        private bool TryGetWaitingComponent(int entityId, out WaitingComponent com)
        {
            if (_waitingEntityMap.TryGet(entityId, out int componentIndex) && componentIndex != Container.EMPTY_ID)
            {
                if (_waitingComponents.ContainIndex(componentIndex))
                {
                    com = _waitingComponents.Get(componentIndex);
                    return true;
                }
            }

            com = null;
            return false;
        }


        public void AddWaitingComponent(int entityId, int turn)
        {
            if (_waitingEntityMap.TryGet(entityId, out int componentIndex) && componentIndex == Container.EMPTY_ID)
            {
                if (_waitingComponents.ContainIndex(componentIndex))
                {
                    return;
                }
                else
                {
                    var com = GetWaitingComponentFromPool();
                    com.Turn = turn;

                    _waitingEntityMap.Set(entityId, _waitingComponents.Count);
                    _waitingComponents.Add(com);
                    _entityWaitingMap.Add(entityId);
                }
            }
        }

        public int GetWaitingEntityId(int index)
        {
            if (_entityWaitingMap.TryGet(index, out int result))
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
        public bool TryGetWaitingTurnFromEntityId(int entityId, out int turn)
        {
            if (TryGetWaitingComponent(entityId, out WaitingComponent com))
            {
                turn = com.Turn;
                return true;
            }
            else
            {
                turn = -1;
                return false;
            }
        }

        public bool TrySetWaitingTurnFromEntityId(int entityId, int turn)
        {
            if (TryGetWaitingComponent(entityId, out WaitingComponent com))
            {
                com.Turn = turn;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetWaitingTurnFromIndex(int index, out int turn)
        {
            turn = -1;
            if (_waitingComponents.ContainIndex(index))
            {
                turn = _waitingComponents.Get(index).Turn;
                return true;
            }

            return false;
        }

        public bool TrySetWaitingTurnFromIndex(int index, int value)
        {
            if (_waitingComponents.ContainIndex(index))
            {
                _waitingComponents.Get(index).Turn = value;
                return true;
            }

            return false;
        }
        #endregion

    }

}

