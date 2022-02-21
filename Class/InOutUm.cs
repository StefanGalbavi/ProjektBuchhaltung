using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaushaltsbuchWPF.Class
{
    public class InOutUm
    {
        [Key]
        public int InOutUmId { get; set; }

        public string? InOutUmName { get; set; }

        public virtual List<Booking>? Booking { get; set; }


    }
}
