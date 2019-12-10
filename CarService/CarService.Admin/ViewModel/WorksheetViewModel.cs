using CarService.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace CarService.Admin.ViewModel
{
    public class WorksheetViewModel : ViewModelBase
    {
        private int _finalPrice;
        private ObservableCollection<WorkItemDTO> _items;        

        public AppointmentDTO Appointment { get; set; }        

        public ObservableCollection<WorkItemDTO> Items
        {
            get { return _items; }
            private set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged();
                }
            }
        }

        public int FinalPrice
        {
            get { return _finalPrice; }
            private set
            {
                if (_finalPrice != value)
                {
                    _finalPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        

        public WorksheetViewModel()
        {
            Items = new ObservableCollection<WorkItemDTO>();           
        }

    }
}
 