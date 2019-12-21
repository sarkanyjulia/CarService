using CarService.Admin.Model;
using CarService.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace CarService.Admin.ViewModel
{
    public class WorksheetViewModel : ViewModelBase
    {
        
        private int _finalPrice;
        private ObservableCollection<WorkItemDTO> _items;
        //private WorkItemDTO _selectedWorkItem;
        private WorkItemDTO _selectedItemListItem;

        public AppointmentDTO Appointment { get; set; }

        public List<WorkItemDTO> ItemList { get; set; }

        public ObservableCollection<WorkItemDTO> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged();
                }
            }
        }
        /*
        public WorkItemDTO SelectedWorkItem
        {
            get { return _selectedWorkItem; }
            set
            {
                if (_selectedWorkItem != value)
                {
                    _selectedWorkItem = value;
                    OnPropertyChanged();
                }
            }
        }*/

        public WorkItemDTO SelectedItemListItem
        {
            get { return _selectedItemListItem; }
            set
            {
                if (_selectedItemListItem != value)
                {
                    _selectedItemListItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public int FinalPrice
        {
            get { return _finalPrice; }
            set
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
 