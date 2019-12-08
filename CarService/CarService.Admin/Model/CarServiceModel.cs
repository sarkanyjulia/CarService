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

        public UserDTO RecentUser { get; private set; }

        public bool IsUserLoggedIn { get; private set; }     
        
        public List<AppointmentDTO> AppointmentList
        {
            get { return _appointmentList; }
        }

        public async Task<bool> LoginAsync(string userName, string userPassword)
        {
            IsUserLoggedIn = await _persistence.LoginAsync(userName, userPassword);
            if (IsUserLoggedIn)
            {
                RecentUser = await _persistence.GetUser();
            }
            return IsUserLoggedIn;
        }

        public async Task<bool> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;

            IsUserLoggedIn = !(await _persistence.LogoutAsync());
            RecentUser = null;
            return IsUserLoggedIn;
        }

        public async void Model_LoginSuccessAsync(object sender, EventArgs e)
        {
            UserDTO user = await _persistence.GetUser();
            RecentUser = user;
        }

        public async Task LoadAsync()
        {
            _appointmentList = (await _persistence.GetAppointments()).ToList();
        }
    }
}
