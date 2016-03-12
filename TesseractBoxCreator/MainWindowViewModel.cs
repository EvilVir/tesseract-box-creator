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

        protected double _zoom = 1d;
        protected ImageFile _currentImage = null;
        protected BoxesFile _currentBoxes = new BoxesFile();

        public double Zoom { get { return _zoom; } set { _zoom = value; OnPropertyChanged("Zoom"); } }
        public ImageFile CurrentImage { get { return _currentImage; } set { UnbindCurrentImage(_currentImage); _currentImage = value; OnPropertyChanged("CurrentImage"); BindCurrentImage(value); } }
        public BoxesFile CurrentBoxes { get { return _currentBoxes; } set { UnbindCurrentBoxes(_currentBoxes); _currentBoxes = value; OnPropertyChanged("CurrentBoxes"); BindCurrentBoxes(value); } }

        #endregion

        #region Initialization and finalization

        public MainWindowViewModel Initialize()
        {
            return this;
        }


        public MainWindowViewModel Finalize()
        {
            return this;
        }

        #endregion

        #region Cross bindings

        protected void BindCurrentImage(ImageFile image)
        {
            if (image != null)
            {
                image.PropertyChanged += CurrentImage_PropertyChanged;
            }
        }

        protected void UnbindCurrentImage(ImageFile image)
        {
            if (image != null)
            {
                image.PropertyChanged -= CurrentImage_PropertyChanged;
            }
        }

        protected void BindCurrentBoxes(BoxesFile boxes)
        {
            if (boxes != null)
            {
                boxes.PropertyChanged += Boxes_PropertyChanged;
            }
        }

        protected void UnbindCurrentBoxes(BoxesFile boxes)
        {
            if (boxes != null)
            {
                boxes.PropertyChanged -= Boxes_PropertyChanged;
            }
        }

        protected void CurrentImage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Page")
            {
                this.CurrentBoxes.Page = this.CurrentImage.Page;
            }
        }

        protected void Boxes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Event triggers

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
