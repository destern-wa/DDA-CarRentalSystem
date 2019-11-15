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
        public Vehicle OldVehicle { get; set; }
        public bool Updated { get; set; }
    }
}
