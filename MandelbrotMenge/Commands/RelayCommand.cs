using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MandelbrotMenge.Commands
{
    public class RelayCommand : ICommand
    {
        private Func<bool> canExecute;

        private Action action;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Func<bool> canExecute, Action action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute();
        }

        public void Execute(object parameter)
        {
            this.action();
        }

        public void FireCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
