
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaushaltsbuchWPF.Class
{
    public class HBContext : DbContext
    {
        public HBContext()
        {

        }

        public virtual DbSet<Booking> Booking { get; set; } = null!;

        public virtual DbSet<Category> Category { get; set; } = null!;

        public virtual DbSet<Konto> Konto { get; set; } = null!;

        public virtual DbSet<StandingOrder> StandingOrder { get; set; } = null!;

        public virtual DbSet<SubCategory> SubCategory { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=HaushaltsbuchVVIII;Trusted_Connection=True;");
            }
        }
    }
}
