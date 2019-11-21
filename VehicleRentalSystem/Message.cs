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
    /// <summary>
    /// Base interface for  IListen<T>
    /// </summary>
    public interface IListen { }
    /// <summary>
    /// Interface for view models that need to listen to events from other view models
    /// </summary>
    public interface IListen<T> : IListen
    {
        void Handle(T obj);
    }

    /// <summary>
    /// Handles messages that need to be passed between view models
    /// </summary>
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

    /// <summary>
    /// Message to be passed between view models
    /// </summary>
    class Message
    {
        /// <summary>
        /// New/edited/update vehicle
        /// </summary>
        public Vehicle Vehicle { get; set; }
        /// <summary>
        /// Original vehicle that was edited
        /// </summary>
        public Vehicle OldVehicle { get; set; }
        /// <summary>
        /// If the vehicle has been updated without basic details being edited (e.g. it was rented out)
        /// </summary>
        public bool Updated { get; set; }
    }
}
