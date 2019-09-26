using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace WM2000.Terminal
{
    public class History : ICollection<string[]>
    {
        private List<string[]> _history = new List<string[]>();

        private int _historyIndex = 0;

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

        public void Clear()
        {
            _history.Clear();
            _historyIndex = 0;
        }

        public bool Contains(string[] item)
        {
            return _history.Contains(item);
        }

        public string[] GetNextItem()
        {
            string[] result = _history.Count == 0 ? new string[0] : _history[_historyIndex];
            
            if (_historyIndex < _history.Count - 1)
                _historyIndex++;

            return result;
        }
        
        public string[] GetPreviousItem()
        {
            string[] result = _history.Count == 0 ? new string[0] : _history[_historyIndex];
            
            if (_historyIndex > 0)
                _historyIndex--;

            return result;
        }

        public void CopyTo(string[][] array, int arrayIndex)
        {
            _history.CopyTo(array, arrayIndex);
        }

        public bool Remove(string[] item)
        {
            bool success = _history.Remove(item);
            if (success)
                _historyIndex = _history.Count - 1;
            
            return success;
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