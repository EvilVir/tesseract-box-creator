using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesseractBoxCreator.Model
{
    public class BoxesList : ObservableCollection<BoxItem>
    {
        public class CollectionItemPropertyChangedEventArgs : EventArgs
        {
            public BoxItem Item { get; private set; }
            public string PropertyName { get; private set; }

            public CollectionItemPropertyChangedEventArgs(BoxItem item, string propertyName)
            {
                Item = item;
                PropertyName = propertyName;
            }
        }

        public delegate void CollectionItemPropertyChangedEventHandler(object sender, CollectionItemPropertyChangedEventArgs e);
        public event CollectionItemPropertyChangedEventHandler CollectionItemPropertyChanged;

        public IEnumerable<BoxItem> GetSorted()
        {
            return SortItems();
        }

        public IEnumerable<BoxItem> GetForPage(int CurrentPage)
        {
            return SortItems(x => x.Page == CurrentPage);
        }

        protected IEnumerable<BoxItem> SortItems(Func<BoxItem, bool> predicate = null)
        {
            IEnumerable<BoxItem> output = predicate == null ? this : this.Where(predicate);
            
            //.OrderBy(x => x.Page).ThenByDescending(x => x.Y).ThenBy(x => x.X).ThenByDescending(x => x.Y2).ThenBy(x => x.X2).ThenBy(x => x.Letter);

            return output;
        }

        public new void Add(BoxItem item)
        {
            item.PropertyChanged += Item_PropertyChanged;
            base.Add(item);
        }

        public new void Remove(BoxItem item)
        {
            item.PropertyChanged -= Item_PropertyChanged;
            base.Remove(item);
        }

        public void RemoveAll(Func<BoxItem, bool> predicate)
        {
            foreach (BoxItem item in this.Where(predicate).ToList())
            {
                this.Remove(item);
            }
        }

        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionItemPropertyChanged((BoxItem)sender, e.PropertyName);
        }

        protected void OnCollectionItemPropertyChanged(BoxItem item, string propertyName)
        {
            if (CollectionItemPropertyChanged != null)
            {
                CollectionItemPropertyChanged.Invoke(this, new CollectionItemPropertyChangedEventArgs(item, propertyName));
            }
        }
    }
}
