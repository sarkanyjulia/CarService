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

        void Model_LoginSuccessAsync(object sender, EventArgs e);

        Boolean IsUserLoggedIn { get; }

        List<AppointmentDTO> AppointmentList { get; }

        Task LoadAsync();
    }
}
