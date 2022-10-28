using System;

namespace Utilities
{
    public class RingBuffer<T>
    {
        private readonly T[] _elements;
        private readonly int _size;
        private int _begin;
        private int _end;

        public bool IsEmpty => _begin == _end;
        public bool IsFull => NextSlot(_end) == _begin;

        public RingBuffer(int size)
        {
            var actualSize = size + 1; // reserve one slot for sentinel
            _elements = new T[actualSize];
            _size = actualSize;
        }

        private int PrevSlot(int index)
        {
            return (index + _size - 1) % _size;
        }

        private int NextSlot(int index)
        {
            return (index + 1) % _size;
        }

        public bool PeekFront(out T element)
        {
            if (IsEmpty)
            {
                element = default;
                return false;
            }

            element = _elements[_begin];
            return true;
        }

        public bool PopFront()
        {
            if (IsEmpty)
            {
                return false;
            }

            _begin = NextSlot(_begin);
            return true;
        }

        public bool TryPushBack(T element)
        {
            if (IsFull) return false;

            _elements[_end] = element;
            _end = NextSlot(_end);
            return true;
        }

        public bool TryPopBack(out T element)
        {
            if (IsEmpty)
            {
                element = default;
                return false;
            }

            _end = PrevSlot(_end);
            element = _elements[_end];
            return true;
        }
    }
}