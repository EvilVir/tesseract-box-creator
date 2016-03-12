using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesseractBoxCreator.Commands
{
    public class ChangeZoomCommand : BaseCommand<MainWindowViewModel>
    {
        public ChangeZoomCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Zoom")
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            double? targetZoom = CalculateZoom(parameter);
            return targetZoom != null;
        }

        public override void Execute(object parameter)
        {
            double? targetZoom = CalculateZoom(parameter);
            if (targetZoom != null)
            {
                this.ViewModel.Zoom = targetZoom.Value;
            }
        }

        protected double? CalculateZoom(object parameter)
        {
            string jump = (string)parameter;
            bool relativeJump = jump.StartsWith("+") || jump.StartsWith("-");
            double value = Double.Parse(jump, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);

            double targetZoom = relativeJump ? this.ViewModel.Zoom + value : value;

            return targetZoom >= 0 && targetZoom <= 10 ? (double?)targetZoom : null;
        }
    }
}
