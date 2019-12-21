using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CarService.Admin.ViewModel.Converters
{
    public class WorkTypeConverter : IValueConverter
    {
        private Dictionary<String, String> _workTypes = new Dictionary<string, string>
        {         
            { "Maintenance", "Kötelező szervíz" },
            { "Inspection", "Műszaki vizsga" },
            { "Failure", "Meghibásodás" }
        };


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is String))
                return Binding.DoNothing;                       
            
            String workTypeKey = value as String;

            if (!_workTypes.ContainsKey(workTypeKey))
                return Binding.DoNothing;

            return _workTypes[workTypeKey];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is String))
                return DependencyProperty.UnsetValue;          
            
            Dictionary<String, String> workTypesInverted = new Dictionary<String, String>();
            foreach(String key in _workTypes.Keys)
            {
                workTypesInverted.Add(_workTypes[key], key);
            }
            String workTypeValue = value as String;

            if (!_workTypes.ContainsValue(workTypeValue))          
                return DependencyProperty.UnsetValue;

            return workTypesInverted[workTypeValue];
        }
    }
}
