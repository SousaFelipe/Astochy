using System;
using System.Windows.Input;



namespace VadenStock.Core
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }



        #pragma warning disable CS8618
        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        #pragma warning restore CS8618
        {
            _execute = execute;
            #pragma warning disable CS8601
            _canExecute = canExecute;
            #pragma warning restore CS8601
        }



        public bool CanExecute(object? parameter)
        {
            #pragma warning disable CS8604
            return _canExecute == null || _canExecute(parameter);
            #pragma warning restore CS8604
        }



        public void Execute(object? parameter)
        {
            #pragma warning disable CS8604
            _execute?.Invoke(parameter);
        }
    }
}
