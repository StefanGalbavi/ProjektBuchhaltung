using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaushaltsbuchWPF
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }


        //Eine Kategorie kann mehrere Unter Kategorien haben
        public virtual List<SubCategory>? SubCategories { get; set; }

        public virtual List<Booking>? Booking { get; set; }
    }
}
