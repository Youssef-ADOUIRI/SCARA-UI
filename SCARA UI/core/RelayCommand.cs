using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SCARA_UI.core
{
    class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<Object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<Object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parametre)
        {
            return _canExecute == null || _canExecute(parametre);
        }

        public void Execute(object parametre)
        {
            _execute(parametre);
        }
    }
}
