using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CarService.Admin.Model;
using CarService.Admin.Persistence;
using CarService.Data;
using System.Windows;

namespace CarService.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ICarServiceModel _model;
        private ObservableCollection<AppointmentDTO> _appointments;
        private AppointmentDTO _selectedAppointment;

        public MainViewModel(ICarServiceModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;

            LoadAsync();
            //LoadCommand = new DelegateCommand(param => LoadAsync());
        }

        public ObservableCollection<AppointmentDTO> Appointments
        {
            get { return _appointments; }
            private set
            {
                if (_appointments != value)
                {
                    _appointments = value;
                    OnPropertyChanged();
                }
            }           
        }

        public AppointmentDTO SelectedAppointment
        {
            get { return _selectedAppointment; }
            set
            {
                if (_selectedAppointment != value)
                {
                    _selectedAppointment = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                Appointments = new ObservableCollection<AppointmentDTO>(_model.AppointmentList);
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }
    }
}
