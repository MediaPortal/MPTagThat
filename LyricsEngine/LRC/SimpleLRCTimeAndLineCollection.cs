using System;
using System.Collections;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;

namespace LyricsEngine.LRC
{
    public class SimpleLRCTimeAndLineCollection : IEnumerable, ICollection
    {

        private object[] items;

        public SimpleLRCTimeAndLineCollection(object[] array)
        {
            items = array;
            Sort(items);
        }

        public int GetSimpleLRCTimeAndLineIndex(long time)
        {
            if (time <= ((SimpleLRCTimeAndLine)items[0]).Time)
            {
                return 0;
            }

            for (int i = 1; i < items.Length; i++)
            {
                if (((SimpleLRCTimeAndLine)items[i - 1]).Time < time && time <= ((SimpleLRCTimeAndLine)items[i]).Time)
                {
                    return i;
                }
            }

            if (time > ((SimpleLRCTimeAndLine)items[items.Length - 1]).Time)
            {
                return items.Length - 1;
            }
            else
            {
                throw (new IndexOutOfRangeException("IndexOutOfRangeException in GetSimpleLRCTimeAndLineIndex"));
            }
        }

        public SimpleLRCTimeAndLine this[int index]
        {
            get
            {
                if (index < items.Length)
                {
                    return (SimpleLRCTimeAndLine)items[index];
                }
                else
                    return null;
            }
        }

        #region IEnumerable implementation
        public IEnumerator GetEnumerator()
        {
            return new Enumerator(items);
        }
        #endregion

        #region ICollection implementation
        public int Count
        {
            get { return items.Length; }
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
            private int cursor;
            private object[] elements;

            public Enumerator(object[] items)
            {
                elements = new object[items.Length];
                Array.Copy(items, elements, items.Length);
                cursor = -1;
            }

            public bool MoveNext()
            {
                ++cursor;
                if (cursor > (elements.Length - 1))
                {
                    return false;
                }
                return true;
            }

            public void Reset()
            {
                cursor = -1;
            }

            public object Current
            {
                get
                {
                    if (cursor > (elements.Length - 1))
                    {
                        throw new InvalidOperationException("Enumration already finished");
                    }
                    if (cursor == -1)
                    {
                        throw new InvalidOperationException("Enumeration not started");
                    }
                    return elements[cursor];
                }
            }
        }
        #endregion

        #region SortAfterTimeClass class, IComparer implementation
        private class SortAfterTimeClass : IComparer
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(y, x));
            }

        }
        #endregion

        public string[] Copy()
        {
            string[] array = new string[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                SimpleLRCTimeAndLine timeLine = (SimpleLRCTimeAndLine)items[i];
                array.SetValue(timeLine.Line, i);
            }
            return array;
        }

        private void Sort(object obj)
        {
            IComparer myComparer = new SortAfterTimeClass();
            Array.Sort(items, myComparer);
        }
    }
}

