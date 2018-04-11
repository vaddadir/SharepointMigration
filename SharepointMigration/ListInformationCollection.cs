using System.Collections;
using System.Collections.Generic;

namespace SharepointMigration
{
    public class ListInformationCollection : IEnumerable<ListInformation>
    {
        private Dictionary<string, ListInformation> _items = new Dictionary<string, ListInformation>();

        public ListInformation this[string listTitle]
        {
            get
            {
                ListInformation listInformation;
                if (_items.TryGetValue(listTitle, out listInformation))
                {
                    return listInformation;
                }
                return null;
            }
            set
            {
                if (_items.ContainsKey(listTitle))
                {
                    _items[listTitle] = value;
                }
                else
                {
                    _items.Add(listTitle, value);
                }
            }
        }

        public Dictionary<string, ListInformation>.KeyCollection Keys
        {
            get
            {
                return _items.Keys;
            }
        }

        public IEnumerator<ListInformation> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}