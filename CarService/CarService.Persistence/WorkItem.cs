using System;
using System.ComponentModel.DataAnnotations;

namespace CarService.Persistence
{
    public class WorkItem
    {
        [Key]
        public int Id { get; set; }

        public String Item { get; set; }

        public int Price { get; set; }
    }
}
