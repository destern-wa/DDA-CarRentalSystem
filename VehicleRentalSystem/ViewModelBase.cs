using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VehicleRentalSystem
{
    /// <summary>
    /// Base for view models that implements the INotifyPropertyChanged interface
    /// (i.e. view models other than the main view model)
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue,
        [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// Base for view models that need to listen to events from other view models
    /// (i.e. the main view model)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ViewModelBase<T> : IListen<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue,
        [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
        public abstract void Handle(T obj);
    }
}