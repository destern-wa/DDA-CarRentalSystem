using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// MVVM messaging to allow data to be passed between windows. Based on
// https://www.c-sharpcorner.com/UploadFile/20c06b/messaging-system-in-wpf/

namespace VehicleRentalSystem
{
    public interface IListen { }
    public interface IListen<T> : IListen
    {
        void Handle(T obj);
    }
    public abstract class ViewModelBase
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

    public class EventAggregator
    {
        private List<IListen> subscribers = new List<IListen>();

        public void Subscribe(IListen model)
        {
            this.subscribers.Add(model);
        }

        public void Unsubscribe(IListen model)
        {
            this.subscribers.Remove(model);
        }

        public void Publish<T>(T message)
        {
            foreach (var item in this.subscribers.OfType<IListen<T>>())
            {
                item.Handle(message);
            }
        }
    }

    class Message
    {
        public Vehicle Vehicle { get; set; }
    }
}
