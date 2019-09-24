using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WM2000.Terminal
{
    public class History : ICollection<string[]>
    {
        private List<string[]> _history = new List<string[]>();

        private int _historyIndex = 0;

        private int HistoryIndex
        {
            get => _historyIndex;
            set
            {
                if (value < 0 || value > _history.Count - 1)
                    _historyIndex = (value < 0 ? 0 : _history.Count - 1);

                _historyIndex = value;
            }
        } 
        
        public IEnumerator<string[]> GetEnumerator()
        {
            return _history.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string[] item)
        {
            _history.Add(item);
            _historyIndex = _history.Count - 1;
        }

        public string[] Previous()
        {
            return _history[HistoryIndex];
        }

        public string[] Next()
        {
            return _history.First();
        }

        public void Clear()
        {
            _history.Clear();
        }

        public bool Contains(string[] item)
        {
            return _history.Contains(item);
        }

        public void CopyTo(string[][] array, int arrayIndex)
        {
            _history.CopyTo(array, arrayIndex);
        }

        public bool Remove(string[] item)
        {
            return _history.Remove(item);
        }

        public string[] this[int index]
        {
            get => _history[index];
            set => _history[index] = value;
        }

        public int Count => _history.Count;
        public bool IsReadOnly => true;
    }
}