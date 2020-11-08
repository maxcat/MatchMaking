using System;
using System.Collections.Generic;

namespace MatchMakingCore
{
    public class LxList<T>
    {
        private readonly int START_LENGTH = 64;
        private int _lastIndex;
        private T[] _array;
        private T[] _tmp;
        public int Count => _lastIndex + 1;

        public LxList(int length)
        {
            _lastIndex = -1;
            int actualLength = GetClosetLength(length);
            _array = new T[actualLength];
            _tmp = new T[actualLength];
        }

        public LxList()
        {
            _lastIndex = -1;
            _array = new T[START_LENGTH];
            _tmp = new T[START_LENGTH];
        }

        private int GetClosetLength(int length)
        {
            uint result = (uint)START_LENGTH;
            while(result < length)
            {
                result = result << 1;
            }
            return (int)result;
        }

        public void Reset()
        {
            _lastIndex = -1;
        }

        public T Get(int index)
        {
            if (index >= 0 && index <= _lastIndex)
            {
                return _array[index];
            }

            throw new LxException($"index out of range {index}");
        }

        public bool TryGet(int index, out T value)
        {
            if (index >= 0 && index <= _lastIndex)
            {
                value = _array[index];
                return true;
            }

            value = default;
            return false;
        }

        public bool ContainIndex(int index)
        {
            return index >= 0 && index < Count;
        }

        public void Add(T value)
        {
            if (_lastIndex + 1 >= _array.Length)
            {
                Array.Resize(ref _array, _array.Length * 2);
                Array.Resize(ref _tmp, _tmp.Length * 2);
            }
            ++_lastIndex;
            _array[_lastIndex] = value;
        }

        public void Add(T value, IComparer<T> comparer, bool unique)
        {
            if(comparer == null)
            {
                throw new LxException($"comparer can not be null.");
            }

            if(Count <= 0)
            {
                Add(value);
                return;
            }

            int compareResult = Array.BinarySearch(_array, 0, Count, value, comparer);
            if(compareResult < 0)
            {
                int insertIndex = ~compareResult;
                if(insertIndex > _lastIndex)
                {
                    Add(value);
                }
                else
                {
                    Insert(insertIndex, value);
                }
            }
            else if(compareResult >= 0 && !unique)
            {
                int insertIndex = compareResult;
                if (insertIndex > _lastIndex)
                {
                    Add(value);
                }
                else
                {
                    Insert(insertIndex, value);
                }
            }
        }

        public void Set(int index, T value)
        {
            if(index >= 0 && index <= _lastIndex)
            {
                _array[index] = value;
            }
            else
            {
                throw new LxException($"invalid index for set {index}");
            }
        }

        public void Remove(int index)
        {
            if (index < 0 || index > _lastIndex)
            {
                throw new LxException($"invalid remove index {index}");
            }

            if(index == 0)
            {
                // remove head
                Array.Copy(_array, 1, _tmp, 0, Count - 1);
                Swap();
            }
            else if (index == _lastIndex)
            {
                // remove tail
                _array[_lastIndex] = default;
            }
            else
            {
                // remove middle
                Array.Copy(_array, 0, _tmp, 0, index);
                Array.Copy(_array, index + 1, _tmp, index, Count - index);
                Swap();
            }

            --_lastIndex;
        }

        private void Swap()
        {
            var tmp = _array;
            _array = _tmp;
            _tmp = tmp;
        }

        public void Insert(int index, T value)
        {
            if (index < 0 || index > _lastIndex)
            {
                throw new LxException($"invalid insert index {index}");
            }

            int arrayLength = _array.Length;
            if (_lastIndex + 1 >= _array.Length)
            {
                arrayLength *= 2;
                Array.Resize(ref _tmp, arrayLength);
                Array.Resize(ref _array, arrayLength);
            }

            if (index == 0)
            {
                // insert head
                Array.Copy(_array, 0, _tmp, 1, Count);
                Swap();
            }
            else
            {
                // insert middle
                Array.Copy(_array, 0, _tmp, 0, index + 1);
                Array.Copy(_array, index, _tmp, index + 1, Count - index);
                Swap();
            }

            ++_lastIndex;
            _array[index] = value;
        }
    }
}
