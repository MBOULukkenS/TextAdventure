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
            ResetIndex();
        }

        public void Clear()
        {
            _history.Clear();
            ResetIndex();
        }

        public bool Contains(string[] item)
        {
            return _history.Contains(item);
        }

        public void ResetIndex()
        {
            _historyIndex = _history.Count == 0 ? 0 : _history.Count;
        }

        public string[] GetNextItem()
        {
            if (_historyIndex < _history.Count)
                _historyIndex++;
            
            string[] result = (_history.Count == 0 || _historyIndex == _history.Count) 
                ? new string[0] 
                : _history[_historyIndex];

            return result;
        }
        
        public string[] GetPreviousItem()
        {
            if (_historyIndex > 0)
                _historyIndex--;
            
            string[] result = (_history.Count == 0 || _historyIndex == _history.Count) 
                ? new string[0] 
                : _history[_historyIndex];

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
                ResetIndex();
            
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