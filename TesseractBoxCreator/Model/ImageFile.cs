using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TesseractBoxCreator.Model
{
    public class ImageFile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected int _page, _lastPage, _loadingProgress;
        protected ImageSource _currentPageImage;
        protected string _filePath;
        protected string _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        protected List<string> _pageFiles = new List<string>();
        protected FileStream _currentPageStream = null;
        protected bool _isLoading;
        protected BackgroundWorker _loadingWorker;
        protected AutoResetEvent _resetEvent = new AutoResetEvent(false);

        public int Page { get { return _page; } set { _page = value; OnPropertyChanged("Page"); LoadCurrentPageImage(); } }
        public int LastPage { get { return _lastPage; } protected set { _lastPage = value; OnPropertyChanged("LastPage"); } }
        public ImageSource CurrentPageImage { get { return _currentPageImage; } protected set { _currentPageImage = value; OnPropertyChanged("CurrentPageImage"); } }
        public string FilePath { get { return _filePath; } protected set { _filePath = value; OnPropertyChanged("FilePath"); } }
        public bool IsLoading { get { return _isLoading; } protected set { _isLoading = value; OnPropertyChanged("IsLoading"); } }
        public int LoadingProgress { get { return _loadingProgress; } protected set { _loadingProgress = value; OnPropertyChanged("LoadingProgress"); } }

        public ImageFile(string path)
        {
            IsLoading = true;

            _loadingWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

            _loadingWorker.DoWork += (s, e) =>
                {
                    CreateTempFiles(path, _loadingWorker, e);
                };

            _loadingWorker.ProgressChanged += (s, e) =>
                {
                    LoadingProgress = e.ProgressPercentage;
                };

            _loadingWorker.RunWorkerCompleted += (s, e) =>
                {
                    if (!e.Cancelled)
                    {
                        IsLoading = false;
                        LastPage = _pageFiles.Count - 1;
                        FilePath = path;
                        Page = 0;
                    }
                };

            _loadingWorker.RunWorkerAsync();
        }

        public void Close()
        {
            if (_currentPageStream != null) { _currentPageStream.Close(); }
            if (_loadingWorker != null)
            {
                _loadingWorker.CancelAsync();
                _resetEvent.WaitOne();
            }

            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }

        protected void LoadCurrentPageImage()
        {
            if (_currentPageStream != null){ _currentPageStream.Close(); }
            _currentPageStream = new FileStream(_pageFiles[Page], FileMode.Open, FileAccess.Read, FileShare.Read);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = _currentPageStream;
            bi.EndInit();
            bi.Freeze();

            CurrentPageImage = bi;
        }

        protected void CreateTempFiles(string path, BackgroundWorker worker, DoWorkEventArgs e)
        {
            string ext = Path.GetExtension(path).ToLower();

            if (ext == ".tiff" || ext == ".tif")
            {
                Directory.CreateDirectory(_tempDir);

                using (FileStream tiffStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    TiffBitmapDecoder tiffDecoder = new TiffBitmapDecoder(tiffStream, BitmapCreateOptions.IgnoreImageCache | BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);

                    int total = tiffDecoder.Frames.Count;
                    for (int i = 0; i < total; i++)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }

                        string filePath = Path.Combine(_tempDir, i.ToString() + ".png");
                        
                        using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            PngBitmapEncoder tiffEncoder = new PngBitmapEncoder();
                            tiffEncoder.Frames.Add(tiffDecoder.Frames[i]);
                            tiffEncoder.Save(outputFileStream);
                        }

                        _pageFiles.Add(filePath);
                        worker.ReportProgress((int)Math.Ceiling(((double)i / (double)total) * 100d));
                    }
                }
            }
            else
            {
                _pageFiles.Add(path);
                worker.ReportProgress(100);
            }

            _resetEvent.Set();
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
