using System;

namespace VehicleRentalSystem.ViewModel
{
    class EditVehicleViewModel : ViewModelBase
    {
        private EventAggregator eventAggregator;
        private readonly DelegateCommand<string> _saveCommand;

        // MVVM window closing per Andrew's comment in https://social.msdn.microsoft.com/Forums/en-US/17aabea0-4aca-478f-9205-fcd56080b22a/how-to-close-a-window-by-clicking-the-button-using-mvvm?forum=wpf
        public event EventHandler RequestClose;
        private Vehicle oldVehicle;

        private string _makeName;
        public string MakeName
        {
            get => _makeName;
            set
            {
                SetProperty(ref _makeName, value);
            }
        }
        private string _modelName;
        public string ModelName
        {
            get => _modelName;
            set
            {
                SetProperty(ref _modelName, value);
            }
        }
        private string _year;
        public string Year
        {
            get => _year;
            set
            {
                SetProperty(ref _year, value);
            }
        }

        public EditVehicleViewModel(Vehicle oldVehicle, ref EventAggregator eventAggregator)
        {
            this.oldVehicle = oldVehicle;
            MakeName = oldVehicle.Manufacturer;
            ModelName = oldVehicle.Model;
            Year = oldVehicle.Year.ToString();


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
            Vehicle v = new Vehicle(MakeName, ModelName, int.Parse(Year));
            this.eventAggregator.Publish(new Message { Vehicle = v, OldVehicle = oldVehicle });
        }

        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
