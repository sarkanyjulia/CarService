using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Data
{
    class WorksheetDTO
    {
        public int Id { get; set; }

        public AppointmentDTO Appointment { get; set; }

        public List<WorkItemDTO> Items { get; set; }

        public int FinalPrice { get; set; }
    }
}
