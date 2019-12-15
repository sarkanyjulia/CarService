using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Data
{
    public class AppointmentDTO
    {       
        public int Id { get; set; }
       
        public DateTime Time { get; set; }
        
        public String WorkType { get; set; }                
        
        public UserDTO Partner { get; set; }        

        public bool HasWorksheet { get; set; }

        public bool HasClosedWorksheet { get; set; }
    }
}
