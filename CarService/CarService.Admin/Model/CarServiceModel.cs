using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarService.Data;
using CarService.Admin.Persistence;

namespace CarService.Admin.Model
{
    public class CarServiceModel : ICarServiceModel
    {
        private ICarServicePersistence _persistence;
        private List<AppointmentDTO> _appointmentList;

        public CarServiceModel(ICarServicePersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));

            IsUserLoggedIn = false;
            _persistence = persistence;
        }

        public bool IsUserLoggedIn { get; private set; }     
        
        public List<AppointmentDTO> AppointmentList
        {
            get { return _appointmentList; }
        }

        public async Task<bool> LoginAsync(string userName, string userPassword)
        {
            IsUserLoggedIn = await _persistence.LoginAsync(userName, userPassword);            
            return IsUserLoggedIn;
        }

        public async Task<bool> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;
            IsUserLoggedIn = !(await _persistence.LogoutAsync());            
            return IsUserLoggedIn;
        }


        public async Task LoadAsync()
        {
            _appointmentList = (await _persistence.GetAppointments()).ToList();
        }

        public async Task SaveAsync(List<WorksheetDTO> worksheets)
        {
            foreach (WorksheetDTO worksheet in worksheets)
            {
                await _persistence.SaveWorksheetAsync(worksheet);
            }
        }
    }
}
