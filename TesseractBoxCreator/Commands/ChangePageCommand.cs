using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesseractBoxCreator.Commands
{
    public class ChangePageCommand : BaseCommand<MainWindowViewModel>
    {
        public ChangePageCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            int? targetPage = CalculatePageToJump(parameter);
            return targetPage != null;
        }

        public override void Execute(object parameter)
        {
            int? targetPage = CalculatePageToJump(parameter);
            if (targetPage != null)
            {
                this.ViewModel.CurrentImage.Page = targetPage.Value;
            }
        }

        protected int? CalculatePageToJump(object parameter)
        {
            if (this.ViewModel.CurrentImage != null)
            {
                string jump = (string)parameter;
                bool relativeJump = jump.StartsWith("+") || jump.StartsWith("-");
                int value = Int32.Parse(jump);

                int targetPage = relativeJump ? this.ViewModel.CurrentImage.Page + value : value;

                return targetPage >= 0 && targetPage <= this.ViewModel.CurrentImage.LastPage ? (int?)targetPage : null;
            }

            return null;
        }
    }
}
