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

        public CarServiceModel(ICarServicePersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));

            IsUserLoggedIn = false;
            _persistence = persistence;
            Worksheets = new List<WorksheetDTO>();
        }

        public bool IsUserLoggedIn { get; private set; }     
        
        public List<AppointmentDTO> AppointmentList { get; private set; }
        public List<WorkItemDTO> ItemList { get; private set; }

        public List<WorksheetDTO> Worksheets { get; set; }

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
            AppointmentList = (await _persistence.GetAppointments()).ToList();
            ItemList = (await _persistence.GetWorkItems()).ToList();
            if (Worksheets.Any())
            {
                foreach (WorksheetDTO worksheet in Worksheets)
                {
                    // TODO set flags
                    // set view App list to read flags from model
                }
            }
        }

        public async Task<Boolean> SaveAsync()
        {
            Boolean OK = true;
            foreach (WorksheetDTO worksheet in Worksheets)
            {
                if (worksheet.Closed)
                { 
                    if (await _persistence.SaveWorksheetAsync(worksheet))
                    {
                        Worksheets.Remove(worksheet);
                    }
                    else
                    {
                        OK = false;
                        // TODO error message
                    }
                }
            }
            return OK;
        }
    }
}
