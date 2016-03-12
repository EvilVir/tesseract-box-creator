using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TesseractBoxCreator.Commands;
using TesseractBoxCreator.Helpers;
using TesseractBoxCreator.Model;

namespace TesseractBoxCreator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Commands

        private ICommand _openImageFileCommand, _openBoxFileCommand, _saveBoxFileCommand, _saveBoxFileAsCommand, _changePageCommand, _clearBoxesCommand, _changeZoomCommand;

        public ICommand OpenImageFileCommand { get { return _openImageFileCommand ?? (_openImageFileCommand = new OpenImageFileCommand(this)); } }
        public ICommand OpenBoxFileCommand { get { return _openBoxFileCommand ?? (_openBoxFileCommand = new OpenBoxFileCommand(this)); } }
        public ICommand SaveBoxFileCommand { get { return _saveBoxFileCommand ?? (_saveBoxFileCommand = new SaveBoxFileCommand(this)); } }
        public ICommand SaveBoxFileAsCommand { get { return _saveBoxFileAsCommand ?? (_saveBoxFileAsCommand = new SaveBoxFileAsCommand(this)); } }
        public ICommand ChangePageCommand { get { return _changePageCommand ?? (_changePageCommand = new ChangePageCommand(this)); } }
        public ICommand ClearBoxesCommand { get { return _clearBoxesCommand ?? (_clearBoxesCommand = new ClearBoxesCommand(this)); } }
        public ICommand ChangeZoomCommand { get { return _changeZoomCommand ?? (_changeZoomCommand = new ChangeZoomCommand(this)); } }

        #endregion

        #region Data

        private string _currentImageFilePath, _currentBoxFilePath;
        private int _currentPage, _totalPages;
        private ObservableCollection<BoxItem> _currentPageBoxes = new ObservableCollection<BoxItem>();
        private BoxesList _currentBoxes = new BoxesList();
        private bool _reactOnCurrentPageBoxesChange = true;
        private double _zoom = 1d;
        private BitmapImage _currentImage = null;
        private BoxItem _selectedBox = null;

        public string CurrentImageFilePath { get { return _currentImageFilePath; } protected set { _currentImageFilePath = value; NotifyPropertyChanged("CurrentImageFilePath"); } }
        public string CurrentBoxFilePath { get { return _currentBoxFilePath; } set { _currentBoxFilePath = value; NotifyPropertyChanged("CurrentBoxFilePath"); } }
        public int CurrentPage { get { return _currentPage; } protected set { _currentPage = value; NotifyPropertyChanged("CurrentPage"); } }
        public int LastPage { get { return _totalPages; } protected set { _totalPages = value; NotifyPropertyChanged("LastPage"); } }
        public ObservableCollection<BoxItem> CurrentPageBoxes { get { return _currentPageBoxes; } protected set { _currentPageBoxes = value; } }
        public double Zoom { get { return _zoom; } set { _zoom = value; NotifyPropertyChanged("Zoom"); } }
        public BitmapImage CurrentImage { get { return _currentImage; } set { _currentImage = value; NotifyPropertyChanged("CurrentImage"); } }
        public BoxItem SelectedBox { get { return _selectedBox; } set { _selectedBox = value; NotifyPropertyChanged("SelectedBox"); } }

        #endregion

        #region File management

        public MainWindowViewModel Initialize()
        {
            _currentPageBoxes.CollectionChanged += CurrentPageBoxes_CollectionChanged;

            return this;
        }

        protected void CurrentPageBoxes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_reactOnCurrentPageBoxesChange)
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (object item in e.NewItems)
                    {
                        if (item is BoxItem)
                        {
                            _currentBoxes.Add((BoxItem)item);
                        }
                    }
                }

                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    foreach (object item in e.OldItems)
                    {
                        if (item is BoxItem)
                        {
                            _currentBoxes.Remove((BoxItem)item);
                        }
                    }
                }
            }
        }

        public MainWindowViewModel Finalize()
        {
            return this;
        }

        public void LoadImage(string path)
        {
            this.CurrentImageFilePath = path;
            this.CurrentImage = new BitmapImage(new Uri(path));
            this.LastPage = 0;
            this.Zoom = 1d;
            GoToImagePage(0);
        }

        public void LoadBoxFile(string path)
        {
            try
            {
                _currentBoxes = BoxFileHelper.LoadFromFile(path);

                DisplayBoxesForCurrentPage();
                this.CurrentBoxFilePath = path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);

                _currentBoxes = null;
                CurrentBoxFilePath = null;
            }
        }

        public void SaveBoxFile(string path)
        {
            try
            {
                BoxFileHelper.SaveToFile(path, _currentBoxes);
                CurrentBoxFilePath = path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void GoToImagePage(int pageNumber)
        {
            this.CurrentPage = pageNumber;
            DisplayBoxesForCurrentPage();
        }

        public void DisplayBoxesForCurrentPage()
        {
            _reactOnCurrentPageBoxesChange = false;

            CurrentPageBoxes.Clear();
            SelectedBox = null;

            if (_currentBoxes != null)
            {
                foreach (BoxItem item in _currentBoxes.GetForPage(CurrentPage))
                {
                    CurrentPageBoxes.Add(item);
                }
            }

            _reactOnCurrentPageBoxesChange = true;
        }

        public void DeleteBoxes(int? pageNumber)
        {
            if (pageNumber != null)
            {
                _currentBoxes.RemoveAll(x => x.Page == pageNumber.Value);
            }
            else
            {
                _currentBoxes.Clear();
            }

            DisplayBoxesForCurrentPage();
        }

        public BoxItem AddBox(double x, double y, double x2, double y2)
        {
            return AddBox((int)x, (int)y, (int)x2, (int)y2);
        }

        public BoxItem AddBox(int x, int y, int x2, int y2)
        {
            BoxItem output = new BoxItem()
            {
                X2 = x2,
                Y2 = y2,
                X = x,
                Y = y,
                Letter = 'A',
                Page = CurrentPage,
            };

            _currentBoxes.Add(output);
            DisplayBoxesForCurrentPage();

            return output;
        }

        #endregion

        #region Event triggers

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
