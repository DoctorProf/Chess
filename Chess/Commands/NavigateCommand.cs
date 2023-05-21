using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using Chess.ViewModels.Base;

namespace Chess.Commands
{
    internal class NavigateCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
        private readonly Uri uri;
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public NavigateCommand(Action<object> execute,  Uri uri, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.uri = uri;
        }
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }
        public void Execute(object parameter)
        {
            execute(new MyCommandParameters((Page)parameter, uri));
        }
    }
}
