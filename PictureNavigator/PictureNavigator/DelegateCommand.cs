using System;
using System.Windows.Input;

namespace PictureNavigator
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _myExecuteAction;
        private readonly Func<bool> _myCanExecutePredicate;

        public DelegateCommand(Action executeAction) : this(executeAction, () => true)
        {

        }

        public DelegateCommand(Action executeAction, Func<bool> canExecutePredicate)
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
