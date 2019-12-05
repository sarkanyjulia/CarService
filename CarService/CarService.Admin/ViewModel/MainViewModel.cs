using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CarService.Admin.Model;
using CarService.Admin.Persistence;
using CarService.Data;
using System.Windows;

namespace CarService.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ICarServiceModel _model;

        public MainViewModel(ICarServiceModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
        }
    }
}
