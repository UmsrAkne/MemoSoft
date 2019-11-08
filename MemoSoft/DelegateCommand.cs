using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoSoft
{
    public class DelegateCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        public DelegateCommand(Action execute, Func<bool> func) {
            this.execute = execute;
            this.canExecute = func;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return canExecute();
        }

        public void Execute(object parameter) {
            execute();
        }
    }
}
