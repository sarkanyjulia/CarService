using System;
using System.ComponentModel.DataAnnotations;

namespace CarService.Persistence
{
    public class WorksheetWorkItem
    {
        [Key]
        public int Id { get; set; }

        public String Item { get; set; }

        public int Price { get; set; }

        public Worksheet Worksheet { get; set; }
    }
}
