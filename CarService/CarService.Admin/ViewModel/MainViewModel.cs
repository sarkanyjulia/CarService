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
       
        

        public WorksheetViewModel WorksheetUnderEdit { get; set; }
        

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

        

        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand OpenEditorCommand { get; private set; }
        public DelegateCommand CloseWorksheetCommand { get; private set; }
        public DelegateCommand SaveWorksheetCommand { get; private set; }
        public DelegateCommand CancelWorksheetCommand { get; private set; }
        //public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand AddWorkItemCommand { get; private set; }
        public DelegateCommand DeleteWorkItemCommand { get; private set; }



        //public event EventHandler ExitApplication;
        public event EventHandler Logout;
        public event EventHandler EditingStarted;
        public event EventHandler EditingFinished;
        

        public MainViewModel(ICarServiceModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            

            LoadAsync();
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            OpenEditorCommand = new DelegateCommand(param => EditWorksheet(param as AppointmentDTO));
            CloseWorksheetCommand = new DelegateCommand(param => CloseWorksheet());
            SaveWorksheetCommand = new DelegateCommand(param => SaveWorksheet());
            CancelWorksheetCommand = new DelegateCommand(param => CancelWorksheet());
            //ExitCommand = new DelegateCommand(param => OnExitApplication());
            LogoutCommand = new DelegateCommand(param => OnLogout());
            AddWorkItemCommand = new DelegateCommand(param => AddWorkItem());
            DeleteWorkItemCommand = new DelegateCommand(param => DeleteWorkItem());
        }

        private void CloseWorksheet()
        {
            if (MessageBox.Show("A lezárt munkalapot nem fogja tudni tovább szerkeszteni. Biztosan folytatni szeretné?", "Megerősítés",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                WorksheetDTO existingWorksheet = _model.Worksheets.FirstOrDefault(w => w.Appointment.Id == WorksheetUnderEdit.Appointment.Id);
                if (existingWorksheet != null)
                {
                    _model.Worksheets.Remove(existingWorksheet);
                }
                _model.Worksheets.Add(new WorksheetDTO
                {
                    Appointment = WorksheetUnderEdit.Appointment,
                    Items = WorksheetUnderEdit.Items.ToList(),
                    FinalPrice = WorksheetUnderEdit.FinalPrice,
                    Closed = true
                });
                AppointmentDTO appointmentToUpdate = _model.AppointmentList.First(a => a.Id == WorksheetUnderEdit.Appointment.Id);
                appointmentToUpdate.HasWorksheet = true;
                appointmentToUpdate.HasClosedWorksheet = true;
                Appointments = new ObservableCollection<AppointmentDTO>(_model.AppointmentList);
                OnEditingFinished();
            }
            else
            {
                // do nothing
            }
        }

        private void CancelWorksheet()
        {            
            OnEditingFinished();
        }

        private void SaveWorksheet()
        {
            WorksheetDTO existingWorksheet = _model.Worksheets.FirstOrDefault(w => w.Appointment.Id == WorksheetUnderEdit.Appointment.Id);
            if (existingWorksheet!=null)
            {
                _model.Worksheets.Remove(existingWorksheet);
            }
            _model.Worksheets.Add(new WorksheetDTO
            {
                Appointment = WorksheetUnderEdit.Appointment,
                Items = WorksheetUnderEdit.Items.ToList(),
                FinalPrice = WorksheetUnderEdit.FinalPrice
            });
            AppointmentDTO appointmentToUpdate = _model.AppointmentList.First(a => a.Id == WorksheetUnderEdit.Appointment.Id);
            appointmentToUpdate.HasWorksheet = true;
            Appointments = new ObservableCollection<AppointmentDTO>(_model.AppointmentList);
            OnEditingFinished();
        }

        private void AddWorkItem()
        {
            if (WorksheetUnderEdit.SelectedItemListItem != null)
            { 
                WorksheetUnderEdit.Items.Add(new WorkItemDTO
                {
                    Id = WorksheetUnderEdit.SelectedItemListItem.Id,
                    Item = WorksheetUnderEdit.SelectedItemListItem.Item,
                    Price = WorksheetUnderEdit.SelectedItemListItem.Price
                });
                WorksheetUnderEdit.FinalPrice += WorksheetUnderEdit.SelectedItemListItem.Price;
            }
        }

        private void DeleteWorkItem()
        {
            if (WorksheetUnderEdit.SelectedItemListItem != null)
            {
                WorkItemDTO itemToDelete = WorksheetUnderEdit.Items.FirstOrDefault(item => item.Id== WorksheetUnderEdit.SelectedItemListItem.Id);
                if (itemToDelete!=null)
                {
                    WorksheetUnderEdit.Items.Remove(itemToDelete);
                    WorksheetUnderEdit.FinalPrice -= itemToDelete.Price;
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

        private async void SaveAsync()
        {
                try
                {
                    if (await _model.SaveAsync())
                    {
                        OnMessageApplication("Sikeres mentés.");
                    }
                    else
                    {
                        OnMessageApplication("Néhány munkalap mentése nem sikerült.");
                    }
                    LoadAsync();
                }
                catch (PersistenceUnavailableException)
                {
                    OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
                }
            
        }

        private void EditWorksheet(AppointmentDTO appointment)
        {
            if (appointment != null && WorksheetUnderEdit==null && !appointment.HasClosedWorksheet)
            {
                WorksheetUnderEdit = new WorksheetViewModel
                {
                    Appointment = appointment,
                    ItemList = _model.ItemList,
                    FinalPrice = 0
                };
                if (appointment.HasWorksheet)
                {
                    WorksheetDTO worksheet = _model.Worksheets.Find(w => w.Appointment.Id == appointment.Id);
                    WorksheetUnderEdit.Items = new ObservableCollection<WorkItemDTO>(worksheet.Items);
                    WorksheetUnderEdit.FinalPrice = worksheet.FinalPrice;
                }                
                OnEditingStarted();
            }
        }

        private void OnEditingStarted()
        {
            if (EditingStarted != null)
                EditingStarted(this, EventArgs.Empty);
        }

        /*
        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }
        */

        private void OnLogout()
        {
            if (Logout != null)
                Logout(this, EventArgs.Empty);
        }

        private void OnEditingFinished()
        {
            if (EditingFinished != null)
                EditingFinished(this, EventArgs.Empty);
        }

        

    }
}
