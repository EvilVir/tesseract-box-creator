using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesseractBoxCreator.Commands
{
    public class SaveBoxFileAsCommand : BaseCommand<MainWindowViewModel>
    {
        public SaveBoxFileAsCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            this.CanBeExecuted = false;

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Box files|*.box",
                CheckPathExists = true,
                CheckFileExists = false,
                AddExtension = true,
                OverwritePrompt = true,
            };

            if (dialog.ShowDialog() == true)
            {
                this.ViewModel.SaveBoxFile(dialog.FileName);
            }

            this.CanBeExecuted = true;
        }
    }
}
