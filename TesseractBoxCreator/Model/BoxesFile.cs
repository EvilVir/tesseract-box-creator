using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesseractBoxCreator.Helpers;

namespace TesseractBoxCreator.Model
{
    public class BoxesFile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool _reactOnCurrentPageBoxesChange, _isUnsaved;
        protected int _page, _lastPage;
        protected string _filePath;
        protected ObservableCollection<BoxItem> _currentPageBoxes = new ObservableCollection<BoxItem>();
        protected BoxItem _selectedBox = null;
        protected BoxesList _allBoxes;

        public bool ReactOnCurrentPageBoxesChange { get { return _reactOnCurrentPageBoxesChange; } set { _reactOnCurrentPageBoxesChange = value; OnPropertyChanged("ReactOnCurrentPageBoxesChange"); } }
        public int Page { get { return _page; } set { _page = value; OnPropertyChanged("Page"); LoadBoxesForPage(value); } }
        public int LastPage { get { return _lastPage; } protected set { _lastPage = value; OnPropertyChanged("LastPage"); } }
        public string FilePath { get { return _filePath; } protected set { _filePath = value; OnPropertyChanged("FilePath"); } }

        public ObservableCollection<BoxItem> CurrentPageBoxes { get { return _currentPageBoxes; } protected set { _currentPageBoxes = value; OnPropertyChanged("CurrentPageBoxes"); } }
        public BoxItem SelectedBox { get { return _selectedBox; } set { _selectedBox = value; OnPropertyChanged("SelectedBox"); } }
        public BoxesList AllBoxes { get { return _allBoxes; } set { _allBoxes = value; OnPropertyChanged("AllBoxes"); } }
        public bool IsUnsaved { get { return _isUnsaved; } protected set { _isUnsaved = value; OnPropertyChanged("IsUnsaved"); } }

        public BoxesFile()
        {
            IsUnsaved = true;
        }

        public BoxesFile(string path)
        {
            _allBoxes = BoxFileHelper.LoadFromFile(path);
            _allBoxes.CollectionItemPropertyChanged += AllBoxes_CollectionItemPropertyChanged;
            _allBoxes.CollectionChanged += AllBoxes_CollectionChanged;
            _currentPageBoxes.CollectionChanged += CurrentPageBoxes_CollectionChanged;

            FilePath = path;
            LastPage = _allBoxes.Count > 0 ? _allBoxes.Max(x => x.Page) : 0;
            Page = 0;

            IsUnsaved = false;
        }

        public void Save()
        {
            Save(FilePath);
        }

        public void Save(string path)
        {
            BoxFileHelper.SaveToFile(path, AllBoxes);
            this.FilePath = path;
            this.IsUnsaved = false;
        }

        protected void LoadBoxesForPage()
        {
            LoadBoxesForPage(Page);
        }

        protected void LoadBoxesForPage(int page)
        {
            ReactOnCurrentPageBoxesChange = false;

            CurrentPageBoxes.Clear();
            SelectedBox = null;

            if (_allBoxes != null)
            {
                foreach (BoxItem item in _allBoxes.GetForPage(page))
                {
                    CurrentPageBoxes.Add(item);
                }
            }

            ReactOnCurrentPageBoxesChange = true;
        }

        public void ClearBoxes(int? pageNumber)
        {
            if (pageNumber != null)
            {
                _allBoxes.RemoveAll(x => x.Page == pageNumber.Value);
            }
            else
            {
                _allBoxes.Clear();
            }

            LoadBoxesForPage();
        }


        #region Event triggers

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Event handlers

        protected void CurrentPageBoxes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ReactOnCurrentPageBoxesChange)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (object item in e.NewItems)
                    {
                        if (item is BoxItem)
                        {
                            _allBoxes.Add((BoxItem)item);
                        }
                    }
                }

                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (object item in e.OldItems)
                    {
                        if (item is BoxItem)
                        {
                            _allBoxes.Remove((BoxItem)item);
                        }
                    }
                }
            }

            IsUnsaved = true;
        }


        void AllBoxes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsUnsaved = true;
        }

        void AllBoxes_CollectionItemPropertyChanged(object sender, BoxesList.CollectionItemPropertyChangedEventArgs e)
        {
            IsUnsaved = true;
        }

        #endregion
    }
}
