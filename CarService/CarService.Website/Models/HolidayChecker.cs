using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Website.Models
{
    public static class HolidayChecker
    {
        public static Boolean IsHoliday(DateTime date)
        {
            bool isHoliday = false;
            if (date.DayOfWeek.Equals(DayOfWeek.Saturday)) isHoliday = true;
            if (date.DayOfWeek.Equals(DayOfWeek.Sunday)) isHoliday = true;
            foreach (DateTime holiday in fixedHolidays)
            {
                if (holiday.Month.Equals(date.Month) && holiday.Day.Equals(date.Day))
                    isHoliday = true;
            }
            return isHoliday;
        }

        private static List<DateTime> fixedHolidays = new List<DateTime>()
        {
            new DateTime(2000, 1, 1),
            new DateTime(2000, 3, 15),
            new DateTime(2000, 10, 23),
            new DateTime(2000, 11, 1),
            new DateTime(2000, 12, 25),
            new DateTime(2000, 12, 26)
        };
    }
}
