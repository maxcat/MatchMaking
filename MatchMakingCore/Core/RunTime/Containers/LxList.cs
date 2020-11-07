using System;

namespace MatchMakingCore
{
    public class LxList<T>
    {
        private readonly int START_LENGTH = 64;
        private int _lastIndex;
        private T[] _array;
        public int Count => _lastIndex + 1;

        public LxList(int length)
        {
            _lastIndex = -1;
            int actualLength = GetClosetLength(length);
            _array = new T[actualLength];
        }

        public LxList()
        {
            _lastIndex = -1;
            _array = new T[START_LENGTH];
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
            _lastIndex = 0;
        }

        public T Get(int index)
        {
            if (index >= 0 && index <= _lastIndex)
            {
                return _array[index];
            }

            throw new LxException($"index out of range {index}");
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
            }
            ++_lastIndex;
            _array[_lastIndex] = value;
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

            T[] tmp = new T[_array.Length];
            if(index == 0)
            {
                // remove head
                Array.Copy(_array, 1, tmp, 0, Count - 1);
                _array = tmp;
            }
            else if (index == _lastIndex)
            {
                // remove tail
                _array[_lastIndex] = default;
            }
            else
            {
                // remove middle
                Array.Copy(_array, 0, tmp, 0, index);
                Array.Copy(_array, index + 1, tmp, index, Count - index);
                _array = tmp;
            }

            --_lastIndex;
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
            }
            
            if (index == 0)
            {
                // insert head
                T[] tmp  = new T[arrayLength];
                Array.Copy(_array, 0, tmp, 1, Count);
                _array = tmp;
            }
            else
            {
                // insert middle
                T[] tmp = new T[arrayLength];
                Array.Copy(_array, 0, tmp, 0, index + 1);
                Array.Copy(_array, index, tmp, index + 1, Count - index + 1);
                _array = tmp;
            }

            ++_lastIndex;
            _array[index] = value;
        }
    }
}
