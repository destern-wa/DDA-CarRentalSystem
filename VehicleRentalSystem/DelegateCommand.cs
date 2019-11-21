using System;

namespace VehicleRentalSystem
{

    /// <summary>
    /// DelegateCommand class for MVVM. Based on: https://blog.magnusmontin.net/2013/06/30/handling-events-in-an-mvvm-wpf-application/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : System.Windows.Input.ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Action to execute</param>
        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Action to execute</param>
        /// <param name="canExecute">Predicate to check if the command can be executed</param>
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// >Predicate to check if a command can be executed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
        }

        /// <summary>
        /// Action to execute for a command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
        }

        /// <summary>
        /// Event handler for when CanExecute is changed
        /// </summary>
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Method to call CanExecuteChanged if it is not null
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
