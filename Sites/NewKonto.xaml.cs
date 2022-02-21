using HaushaltsbuchWPF.Class;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HaushaltsbuchWPF.Sites
{
    /// <summary>
    /// Interaktionslogik für NewKonto.xaml
    /// </summary>
    public partial class NewKonto : Window
    {
        private readonly HBContext context = new HBContext();

        public List<Konto> KontoList { get; set; }

        public List<Konto>? konto = new List<Konto>();

        private KontoListing parent;

        public NewKonto(KontoListing parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void bt_newKontoClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_newKontoCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateNewKonto();
        }

        private void CreateNewKonto()
        {
            Konto konto = new Konto()
            {
                KontoName = txb_kontoName.Text,
                KontoAmount = Convert.ToInt32(KontoAmount.Text)
            };

            context.Konto?.Add(konto);
            context.SaveChanges();
            parent.ReadKonto();

            if (Convert.ToInt32(KontoAmount.Text) > 0)
            {
                var booking = new Booking();
                booking.Amount = Convert.ToDecimal(KontoAmount.Text);
                booking.Date = DateTime.Now;
                booking.Konto = konto;
                context.Booking.Add(booking);
                context.SaveChanges();
            }


            base.Close();
        }
    }
}
