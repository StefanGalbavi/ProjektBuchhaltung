using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaushaltsbuchWPF
{
    public class Konto
    {
        [Key]
        public int KontoId { get; set; }

        public string? KontoName { get; set; }

        public decimal KontoAmount { get; set; }


        //Eine Konto kann mehrere Buchungen haben
        public virtual List<Booking>? Booking { get; set; }
    }
}
