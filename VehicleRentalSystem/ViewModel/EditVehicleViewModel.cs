using System;

namespace VehicleRentalSystem.ViewModel
{
    class EditVehicleViewModel : ViewModelBase
    {
        private EventAggregator eventAggregator;
        private readonly DelegateCommand<string> _saveCommand;
        private readonly DelegateCommand<string> _cancelCommand;

        // MVVM window closing per Andrew's comment in https://social.msdn.microsoft.com/Forums/en-US/17aabea0-4aca-478f-9205-fcd56080b22a/how-to-close-a-window-by-clicking-the-button-using-mvvm?forum=wpf
        public event EventHandler RequestClose;
        private Vehicle oldVehicle;

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
            }
        }

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
            ErrorMessage = "";
            this.oldVehicle = oldVehicle;
            if (oldVehicle != null)
            {
                MakeName = oldVehicle.Manufacturer;
                ModelName = oldVehicle.Model;
                Year = oldVehicle.Year.ToString();
            }

            this.eventAggregator = eventAggregator;
            _saveCommand = new DelegateCommand<string>(
                (s) =>
                {
                    bool saved = SaveVehicle();
                    if (saved)
                    {
                        OnRequestClose();
                    }
                }, //Execute
                (s) => { return true; } //CanExecute //TODO: should be based upon inputted values
            );
            _cancelCommand = new DelegateCommand<string>(
                (s) => { OnRequestClose(); },
                (s) => { return true; } //CanExecute -- always
            );
        }


        public DelegateCommand<string> SaveCommand
        {
            get => _saveCommand;
        }
        public DelegateCommand<string> CancelCommand
        {
            get => _cancelCommand;
        }

        private bool SaveVehicle()
        {
            try
            {
                Vehicle v = new Vehicle(MakeName, ModelName, int.Parse(Year), "1REG0345", 1200, 50.0); // TODO: use real data instead of placeholders
                this.eventAggregator.Publish(new Message { Vehicle = v, OldVehicle = oldVehicle });
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
