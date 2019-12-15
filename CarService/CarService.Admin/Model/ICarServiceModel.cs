using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Data;

namespace CarService.Admin.Model
{
    public interface ICarServiceModel
    {
        Task<Boolean> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();

        Boolean IsUserLoggedIn { get; }

        List<AppointmentDTO> AppointmentList { get; }
        List<WorkItemDTO> ItemList { get; }

        Task LoadAsync();

        Task SaveAsync();

        List<WorksheetDTO> Worksheets { get; set; }
        
    }
}
