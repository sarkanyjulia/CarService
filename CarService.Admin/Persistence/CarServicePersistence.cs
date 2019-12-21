using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CarService.Data;
using Newtonsoft.Json;

namespace CarService.Admin.Persistence
{
    public class CarServicePersistence : ICarServicePersistence
    {
        private HttpClient _client;

        public CarServicePersistence(String baseAddress)
        {
            _client = new HttpClient(); // a szolgáltatás kliense
            _client.BaseAddress = new Uri(baseAddress); // megadjuk neki a címet
        }



        public async Task<bool> LoginAsync(string userName, string userPassword)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/accounts/login/" + userName + "/" + userPassword);
                return response.IsSuccessStatusCode; // a művelet eredménye megadja a bejelentkezés sikeressségét
            }
            catch (Exception ex)
            {               
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/account/logout");
                return !response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {                
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<UserDTO> GetUser()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/users/user");
                if (response.IsSuccessStatusCode)
                {
                    UserDTO user = await response.Content.ReadAsAsync<UserDTO>();
                    return user;
                }
                else
                {                    
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointments()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/appointments/");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<AppointmentDTO> appointments = await response.Content.ReadAsAsync<IEnumerable<AppointmentDTO>>();
                    return appointments;
                }
                else
                {                    
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {                
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> SaveWorksheetAsync(WorksheetDTO worksheet)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/worksheets/", worksheet);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<WorkItemDTO>> GetWorkItems()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/workitems/");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<WorkItemDTO> workItems = await response.Content.ReadAsAsync<IEnumerable<WorkItemDTO>> ();
                    return workItems;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
    }
}
