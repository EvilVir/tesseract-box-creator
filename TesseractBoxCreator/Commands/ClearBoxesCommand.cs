using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesseractBoxCreator.Commands
{
    public class ClearBoxesCommand : BaseCommand<MainWindowViewModel>
    {
        public ClearBoxesCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }
        public override void Execute(object parameter)
        {
            //this.ViewModel.DeleteBoxes(parameter != null ? (int?)parameter : null);
        }
    }
}
