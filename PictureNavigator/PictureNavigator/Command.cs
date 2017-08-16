using System;
using System.Windows.Input;

namespace PictureNavigator
{
    public class Command : ICommand
    {
        private readonly Action _myExecuteAction;
        private readonly Func<bool> _myCanExecutePredicate;

        public Command(Action executeAction) : this(executeAction, () => true)
        {

        }

        public Command(Action executeAction, Func<bool> canExecutePredicate)
        {
            _myExecuteAction = executeAction;
            _myCanExecutePredicate = canExecutePredicate;
        }

        public bool CanExecute(object parameter)
        {
            return _myCanExecutePredicate();
        }

        public void Execute(object parameter)
        {
            _myExecuteAction();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}
