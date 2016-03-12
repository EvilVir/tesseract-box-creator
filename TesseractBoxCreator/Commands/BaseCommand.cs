using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TesseractBoxCreator.Commands
{
    public abstract class BaseCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public T ViewModel { get; set; }


        private bool _canExecute = true;
        protected bool CanBeExecuted
        {
            get
            {
                return _canExecute;
            }

            set
            {
                _canExecute = value;
                OnCanExecuteChanged();
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return CanBeExecuted;
        }

        public abstract void Execute(object parameter);

        protected void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, new EventArgs() { });
            }
        }
    }
}
