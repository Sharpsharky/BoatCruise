using System;
using System.Collections.Generic;

namespace SailingBoat.Infrastructure.Utils
{
    public class PriorityQueue<T>
    {
        private readonly List<(T item, int priority)> _elements = new();

        public int Count => _elements.Count;

        public void Enqueue(T item, int priority)
        {
            _elements.Add((item, priority));
            SiftUp(_elements.Count - 1);
        }

        public T Dequeue()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("The priority queue is empty.");

            var result = _elements[0].item;
            var last = _elements[^1];
            _elements.RemoveAt(_elements.Count - 1);
            if (_elements.Count > 0)
            {
                _elements[0] = last;
                SiftDown(0);
            }
            return result;
        }

        private void SiftUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_elements[index].priority >= _elements[parent].priority)
                    break;
                Swap(index, parent);
                index = parent;
            }
        }

        private void SiftDown(int index)
        {
            int lastIndex = _elements.Count - 1;
            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int smallest = index;

                if (left <= lastIndex && _elements[left].priority < _elements[smallest].priority)
                    smallest = left;
                if (right <= lastIndex && _elements[right].priority < _elements[smallest].priority)
                    smallest = right;

                if (smallest == index)
                    break;

                Swap(index, smallest);
                index = smallest;
            }
        }

        private void Swap(int i, int j)
        {
            (_elements[j], _elements[i]) = (_elements[i], _elements[j]);
        }
    }
}
