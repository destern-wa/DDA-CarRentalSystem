using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// MVVM messaging (between windows) based on https://www.c-sharpcorner.com/UploadFile/20c06b/messaging-system-in-wpf/

namespace VehicleRentalSystem
{
    public interface IListen { }
    public interface IListen<T> : IListen
    {
        void Handle(T obj);
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

    class Messaging
    {
        public Vehicle Vehicle { get; set; }
    }
}
