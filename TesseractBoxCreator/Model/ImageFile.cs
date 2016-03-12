using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TesseractBoxCreator.Model
{
    public class ImageFile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected int _page, _lastPage;
        protected BitmapImage _currentPageImage;
        protected string _filePath;

        public int Page { get { return _page; } set { _page = value; OnPropertyChanged("Page"); LoadCurrentPageImage(); } }
        public int LastPage { get { return _lastPage; } protected set { _lastPage = value; OnPropertyChanged("LastPage"); } }
        public BitmapImage CurrentPageImage { get { return _currentPageImage; } protected set { _currentPageImage = value; OnPropertyChanged("CurrentPageImage"); } }
        public string FilePath { get { return _filePath; } protected set { _filePath = value; OnPropertyChanged("FilePath"); } }

        public ImageFile(string path)
        {
            FilePath = path;
            Page = 0;
            LastPage = 0;
        }

        protected void LoadCurrentPageImage()
        {
            CurrentPageImage = new BitmapImage(new Uri(FilePath));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
