using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesseractBoxCreator.Model;

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
                if (this.ViewModel.CurrentBoxes == null)
                {
                    this.ViewModel.CurrentBoxes = new BoxesFile();
                }

                this.ViewModel.CurrentBoxes.Save(dialog.FileName);
            }

            this.CanBeExecuted = true;
        }
    }
}
