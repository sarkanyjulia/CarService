using CarService.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarService.Admin.Persistence
{
    public interface ICarServicePersistence
    {
        Task<Boolean> LoginAsync(String userName, String userPassword);
        Task<Boolean> LogoutAsync();
        Task<UserDTO> GetUser();
        Task<IEnumerable<AppointmentDTO>> GetAppointments();
    }
}
