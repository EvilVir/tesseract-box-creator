using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TesseractBoxCreator.Helpers;

namespace TesseractBoxCreator.Commands
{
    public class SaveBoxFileCommand : BaseCommand<MainWindowViewModel>
    {
        public SaveBoxFileCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.CanBeExecuted = false;
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentBoxFilePath")
            {
                this.CanBeExecuted = this.ViewModel.CurrentBoxFilePath != null;
            }
        }

        public override void Execute(object parameter)
        {
            this.ViewModel.SaveBoxFile((string)parameter);
        }
    }
}
