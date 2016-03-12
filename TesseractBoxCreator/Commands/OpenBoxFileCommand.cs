using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TesseractBoxCreator.Commands
{
    public class OpenBoxFileCommand : BaseCommand<MainWindowViewModel>
    {
        public OpenBoxFileCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            this.CanBeExecuted = false;

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Box files|*.box",
                CheckPathExists = true,
                CheckFileExists = true,
                AddExtension = true,
                ShowReadOnly = true,
            };

            if (dialog.ShowDialog() == true)
            {
                this.ViewModel.CurrentBoxes = new Model.BoxesFile(dialog.FileName);
            }

            this.CanBeExecuted = true;
        }
    }
}
