using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPad.DataModel
{
    public class UniqueStringCollectionDuplicateException : Exception
    {
        public UniqueStringCollectionDuplicateException(string msg) : base(msg)
        {

        }
    }

    public class UniqueStringCollection : IList<string>
    {
        private List<string> container = new List<string>();

        public UniqueStringCollection()
        {

        }

        public UniqueStringCollection(IEnumerable<string> arr)
        {
            AddRange(arr);
        }

        public void AddRange(IEnumerable<string> arr)
        {
            foreach (var item in arr)
            {
                this.Add(item);
            }
        }

        public string this[int index]
        {
            get
            {
                return container[index];
            }

            set
            {
                if (container[index] == value)
                    return;

                if (container.Contains(value))
                    throw new UniqueStringCollectionDuplicateException("Value already exists on a different index.");

                container[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return container.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(string item)
        {
            if (container.Contains(item))
                return;

            container.Add(item);
        }

        public void Clear()
        {
            container.Clear();
        }

        public bool Contains(string item)
        {
            return container.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            container.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return container.GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return container.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            if (container.Contains(item))
                return;

            container.Insert(index, item);
        }

        public bool Remove(string item)
        {
            return container.Remove(item);
        }

        public void RemoveAt(int index)
        {
            container.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return container.GetEnumerator();
        }
    }
}
