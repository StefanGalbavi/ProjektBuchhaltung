using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaushaltsbuchWPF
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }

        public string? SubCategoryName { get; set; }


        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

    }
}
