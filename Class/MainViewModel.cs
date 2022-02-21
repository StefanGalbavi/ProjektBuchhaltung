using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaushaltsbuchWPF.Class
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            SubCategoryName = new ObservableCollection<SubCategory>();

            CategoryName = new ObservableCollection<Category>();

            Booking = new ObservableCollection<Booking>();
        }

        public ObservableCollection<SubCategory> SubCategoryName { get; set; }

        public ObservableCollection<Category> CategoryName { get; set; }

        public ObservableCollection<Booking> Booking { get; set; }
    }
}
