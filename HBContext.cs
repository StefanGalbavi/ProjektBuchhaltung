
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
            Database.EnsureCreated();
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
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=HaushaltsbuchNewIII;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 1, CategoryName = "Einnahme" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 2, CategoryName = "Ausgabe" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 3, CategoryName = "Umbuchung" });

            modelBuilder.Entity<Konto>().HasData(new Konto { KontoId = 1, KontoName = "Bank" });
            modelBuilder.Entity<Konto>().HasData(new Konto { KontoId = 2, KontoName = "Bar" });

            modelBuilder.Entity<SubCategory>().HasData(new SubCategory { CategoryId = 1, SubCategoryId = 1, SubCategoryName = "Lohn" });

            modelBuilder.Entity<Booking>().HasData(new Booking { BookingId = 1, Amount = 2500, Date = DateTime.UtcNow });
        }
    }
}
