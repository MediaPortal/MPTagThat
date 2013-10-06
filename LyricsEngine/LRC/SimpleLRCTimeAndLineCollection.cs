using System;
using System.Collections;

namespace LyricsEngine.LRC
{
    public class SimpleLRCTimeAndLineCollection : ICollection
    {
        private readonly object[] _items;

        public SimpleLRCTimeAndLineCollection()
        {
        }

        public SimpleLRCTimeAndLineCollection(object[] array)
        {
            _items = array;
            Sort();
        }

        public SimpleLRCTimeAndLine this[int index]
        {
            get
            {
                if (index < _items.Length)
                {
                    return (SimpleLRCTimeAndLine) _items[index];
                }
                return null;
            }
        }

        #region IEnumerable implementation

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(_items);
        }

        #endregion

        #region ICollection implementation

        public int Count
        {
            get { return _items.Length; }
        }

        public bool IsSynchronized
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public object SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void CopyTo(Array array, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region Enumerator class, IEnumerator implementation

        private class Enumerator : IEnumerator
        {
            private int _cursor;
            private readonly object[] _elements;

            public Enumerator(object[] items)
            {
                _elements = new object[items.Length];
                Array.Copy(items, _elements, items.Length);
                _cursor = -1;
            }

            #region IEnumerator Members

            public bool MoveNext()
            {
                ++_cursor;
                if (_cursor > (_elements.Length - 1))
                {
                    return false;
                }
                return true;
            }

            public void Reset()
            {
                _cursor = -1;
            }

            public object Current
            {
                get
                {
                    if (_cursor > (_elements.Length - 1))
                    {
                        throw new InvalidOperationException("Enumration already finished");
                    }
                    if (_cursor == -1)
                    {
                        throw new InvalidOperationException("Enumeration not started");
                    }
                    return _elements[_cursor];
                }
            }

            #endregion
        }

        #endregion

        #region SortAfterTimeClass class, IComparer implementation

        private class SortAfterTimeClass : IComparer
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.

            #region IComparer Members

            int IComparer.Compare(Object x, Object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(y, x));
            }

            #endregion
        }

        #endregion

        public int GetSimpleLRCTimeAndLineIndex(long time)
        {
            if (time <= ((SimpleLRCTimeAndLine) _items[0]).Time)
            {
                return 0;
            }

            for (var i = 1; i < _items.Length; i++)
            {
                if (((SimpleLRCTimeAndLine) _items[i - 1]).Time < time &&
                    time <= ((SimpleLRCTimeAndLine) _items[i]).Time)
                {
                    return i;
                }
            }

            if (time > ((SimpleLRCTimeAndLine) _items[_items.Length - 1]).Time)
            {
                return _items.Length - 1;
            }
            throw (new IndexOutOfRangeException("IndexOutOfRangeException in GetSimpleLRCTimeAndLineIndex"));
        }

        public string[] Copy()
        {
            var array = new string[Count];
            for (var i = 0; i < Count; i++)
            {
                var timeLine = (SimpleLRCTimeAndLine) _items[i];
                array.SetValue(timeLine.Line, i);
            }
            return array;
        }

        private void Sort()
        {
            IComparer myComparer = new SortAfterTimeClass();
            Array.Sort(_items, myComparer);
        }
    }
}