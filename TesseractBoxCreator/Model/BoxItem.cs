using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesseractBoxCreator.Model
{
    public class BoxItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private char _letter;
        private int _x, _y, _x2, _y2, _page;
        private HashSet<string> _changedPropertiesCache = new HashSet<string>();
        private bool _notifyPropertyChangedEventDisabled, _isSelected;

        public char Letter { get { return _letter; } set { _letter = value; NotifyPropertyChanged("Letter"); } }
        public int X { get { return _x; } set { _x = value; NotifyPropertyChanged("X"); NotifyPropertyChanged("Width"); } }
        public int Y { get { return _y; } set { _y = value; NotifyPropertyChanged("Y"); NotifyPropertyChanged("Height"); } }
        public int X2 { get { return _x2; } set { _x2 = value; NotifyPropertyChanged("X2"); NotifyPropertyChanged("Width"); } }
        public int Y2 { get { return _y2; } set { _y2 = value; NotifyPropertyChanged("Y2"); NotifyPropertyChanged("Height"); } }
        public int Page { get { return _page; } set { _page = value; NotifyPropertyChanged("Page"); } }
        public int Width { get { return X2 - X; } set { X2 = X + value; NotifyPropertyChanged("Width"); } }
        public int Height { get { return Y2 - Y; } set { Y2 = Y + value; NotifyPropertyChanged("Height"); } }

        public bool NotifyPropertyChangedEventDisabled
        {
            get
            {
                return _notifyPropertyChangedEventDisabled;
            }

            set
            {
                _notifyPropertyChangedEventDisabled = value;

                if (value == false)
                {
                    NotifyPropertyChanged(_changedPropertiesCache);
                    _changedPropertiesCache.Clear();
                }
            }
        }

        protected void NotifyPropertyChanged(ICollection<string> propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                NotifyPropertyChanged(propertyName);
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (NotifyPropertyChangedEventDisabled)
            {
                if (!_changedPropertiesCache.Contains(propertyName))
                {
                    _changedPropertiesCache.Add(propertyName);
                }
            }
            else
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        
        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4} {5}", Letter, X, Y, X2, Y2, Page);
        }
    }
}
