using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TesseractBoxCreator.Commands
{
    public class OpenImageFileCommand : BaseCommand<MainWindowViewModel>
    {
        public OpenImageFileCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            this.CanBeExecuted = false;

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "All image files|*.tif;*.tiff;*.jpg;*.jpeg;*.png;*.bmp|TIFF files|*.tif;*.tiff|JPEG files|*.jpg;*.jpeg|PNG files|*.png|BMP files|*.bmp|All files|*.*",
                CheckPathExists = true,
                CheckFileExists = true,
                AddExtension = true,
                ShowReadOnly = true,
            };

            if (dialog.ShowDialog() == true)
            {
                if (ViewModel.CurrentImage != null) { ViewModel.CurrentImage.Close(); }
                ViewModel.CurrentImage = new Model.ImageFile(dialog.FileName);
                ViewModel.Zoom = 1d;
            }

            this.CanBeExecuted = true;
        }
    }
}
