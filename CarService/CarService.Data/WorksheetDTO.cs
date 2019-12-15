using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Data
{
    public class WorksheetDTO
    {
        public int Id { get; set; }

        public AppointmentDTO Appointment { get; set; }

        public List<WorkItemDTO> Items { get; set; }

        public int FinalPrice { get; set; }

        public bool Closed { get; set; }
    }
}
