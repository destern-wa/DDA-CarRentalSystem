using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalSystem.ViewModel
{
    class AddVehicleViewModel// : IListen<Messaging>
    {
        private EventAggregator eventAggregator;
        private readonly DelegateCommand<string> _saveCommand;

        // MVVM window closing per Andrew's comment in https://social.msdn.microsoft.com/Forums/en-US/17aabea0-4aca-478f-9205-fcd56080b22a/how-to-close-a-window-by-clicking-the-button-using-mvvm?forum=wpf
        public event EventHandler RequestClose;

        public AddVehicleViewModel(ref EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            _saveCommand = new DelegateCommand<string>(
                (s) =>
                {
                    SaveVehicle();
                    OnRequestClose();
                }, //Execute
                (s) => { return true; } //CanExecute //TODO: should be based upon inputted values
            );
        }

        public DelegateCommand<string> SaveCommand
        {
            get { return _saveCommand; }
        }

        private void SaveVehicle()
        {
                Vehicle v = new Vehicle("man", "mod", 2000);
                this.eventAggregator.Publish(new Messaging { Vehicle = v });
        }

        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
