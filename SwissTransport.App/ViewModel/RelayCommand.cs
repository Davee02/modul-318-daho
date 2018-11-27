using System;
using System.Windows.Input;

namespace SwissTransport.App.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> m_execute;
        private readonly Func<object, bool> m_canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.m_execute = execute;
            this.m_canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.m_canExecute == null || this.m_canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.m_execute(parameter);
        }
    }
}
