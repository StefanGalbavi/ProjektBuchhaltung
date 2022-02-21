using HaushaltsbuchWPF.Class;
using HaushaltsbuchWPF.Sites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HaushaltsbuchWPF
{
    /// <summary>
    /// Interaktionslogik für NewBooking.xaml
    /// </summary>
    public partial class NewBooking : Window
    {
        HBContext hbContext = new HBContext();

        public List<Category> categories = new List<Category>();

        public List<SubCategory> subCategoriesEinnahmen = new List<SubCategory>();

        public List<SubCategory> subCategoriesAusgaben = new List<SubCategory>();

        public List<SubCategory> subCategoriesUmbuchen = new List<SubCategory>();

        public List<Konto> konto = new List<Konto>();

        private BookingListing parent;

        public NewBooking(BookingListing parent)
        {
            InitializeComponent();
            ListComboboxKontoEinnahme();
            ListComboboxSubCategoryEinnahme();
            ListComboboxKontoAusgabe();
            ListComboboxSubCategoryAusgabe();
            ListComboboxKontoUmbuchenEin();
            ListComboboxKontoUmbuchenAus();
            ListComboboxSubCategoryUmbuchung();
            this.parent = parent;

        }


        //Buttons Beenden
        private void btn_newBookingCloseEin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_newBookingCloseAus_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_newBookingCloseUmbuchung_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Methoden
        public void ListComboboxKontoEinnahme()
        {
            konto = hbContext.Konto.ToList();
            cb_kontenEinnahme.ItemsSource = konto;
            cb_kontenEinnahme.SelectedIndex = 0;
        }

        public void ListComboboxKontoAusgabe()
        {
            konto = hbContext.Konto.ToList();
            cb_kontenAusgabe.ItemsSource = konto;
            cb_kontenAusgabe.SelectedIndex = 0;
        }

        public void ListComboboxKontoUmbuchenAus()
        {
            konto = hbContext.Konto.ToList();
            cb_kontoAusgehend.ItemsSource = konto;
            cb_kontoAusgehend.SelectedIndex = 0;
        }

        public void ListComboboxKontoUmbuchenEin()
        {
            konto = hbContext.Konto.ToList();
            cb_kontoEingehend.ItemsSource = konto;
            cb_kontoEingehend.SelectedIndex = 0;
        }

        public void ListComboboxSubCategoryEinnahme()
        {
            subCategoriesEinnahmen = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Einnahme").First().SubCategories.ToList();
            cb_rubrikEinnahme.ItemsSource = subCategoriesEinnahmen;
            cb_rubrikEinnahme.SelectedIndex = 0;
        }

        public void ListComboboxSubCategoryAusgabe()
        {
            subCategoriesAusgaben = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Ausgabe").First().SubCategories.ToList();
            cb_rubrikAusgabe.ItemsSource = subCategoriesAusgaben;
            cb_rubrikAusgabe.SelectedIndex = 0;
        }

        public void ListComboboxSubCategoryUmbuchung()
        {
            subCategoriesUmbuchen = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Umbuchung").First().SubCategories.ToList();
            cb_rubrikUmbuchen.ItemsSource = subCategoriesUmbuchen;
            cb_rubrikUmbuchen.SelectedIndex = 0;
        }


        //Button Buchung erstellen Einnahme
        private void bt_buchungErstellenEinnahmen_Click(object sender, RoutedEventArgs e)
        {
            if (checkEinnahmeIsValid())
            {
                var booking = new Booking();
                booking.Amount = Math.Abs(Convert.ToDecimal(buchungEinnahmen.Text));
                booking.Note = notizEinnahmen.Text;
                booking.Date = (DateTime) datumEinnahme.SelectedDate;
                booking.SubCategory = subCategoriesEinnahmen[cb_rubrikEinnahme.SelectedIndex];
                booking.Konto = konto[cb_kontenEinnahme.SelectedIndex];
                hbContext.Booking.Add(booking);
                hbContext.SaveChanges();
                lb_bookingInfoCheck.Content = "Buchung erfolgreich";
                parent.ReadBooking();
            }
        }

        //Abfrage korrekte Eingaben Einnahme
        private bool checkEinnahmeIsValid()
        {
            try
            {
                Convert.ToDecimal(buchungEinnahmen.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitte einen gültigen Betrag eingeben!");
                return false;
            }
            if (cb_rubrikEinnahme.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte eine Kategorie auswählen!");
                return false;
            }
            if (datumEinnahme.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein Datum auswählen!");
                return false;
            }
            if (cb_kontenEinnahme.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte eine Konto auswählen!");
                return false;
            }

            return true;
        }


        //Button Buchung Ausgabe erstellen
        private void bt_newBookingAugaben_Click(object sender, RoutedEventArgs e)
        {
            if (checkAusgabeIsValid())
            {
                var booking = new Booking();
                booking.Amount = -Math.Abs(Convert.ToDecimal(buchungAusgabe.Text));
                booking.Note = noteAusgabe.Text;
                booking.Date = (DateTime) datumAusgabe.SelectedDate;
                booking.SubCategory = subCategoriesAusgaben[cb_rubrikAusgabe.SelectedIndex];
                booking.Konto = konto[cb_kontenAusgabe.SelectedIndex];
                hbContext.Booking.Add(booking);
                hbContext.SaveChanges();
                lb_bookingInfoCheck.Content = "Buchung erfolgreich";
                parent.ReadBooking();
            }
        }

        //Abfrage korrekte Eingabe Ausgaben
        private bool checkAusgabeIsValid()
        {
            try
            {
                Convert.ToDecimal(buchungAusgabe.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitte einen gültigen Betrag eingeben!");
                return false;
            }
            if (cb_rubrikAusgabe.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte eine Kategorie auswählen!");
                return false;
            }
            if (datumAusgabe.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein Datum auswählen!");
                return false;
            }
            if (cb_kontenAusgabe.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte ein Konto auswählen!");
                return false;
            }

            return true;
        }


        //Button Umbuchung erstellen
        private void bt_umbuchung_Click(object sender, RoutedEventArgs e)
        {
            if (checkUmbuchungIsValid())
            {
                var booking = new Booking();
                booking.Date = (DateTime) datumUmbuchung.SelectedDate;
                booking.Amount = -Math.Abs(Convert.ToDecimal(txb_betragUmbuchung.Text));
                booking.SubCategory = subCategoriesUmbuchen[cb_rubrikUmbuchen.SelectedIndex];
                booking.Konto = konto[cb_kontoAusgehend.SelectedIndex];

                hbContext.Booking.Add(booking);
                hbContext.SaveChanges();

                var bookings = new Booking();
                bookings.Date = (DateTime) datumUmbuchung.SelectedDate;
                bookings.Amount = Math.Abs(Convert.ToDecimal(txb_betragUmbuchung.Text));
                bookings.SubCategory = subCategoriesUmbuchen[cb_rubrikUmbuchen.SelectedIndex];
                bookings.Konto = konto[cb_kontoEingehend.SelectedIndex];

                hbContext.Booking.Add(bookings);
                hbContext.SaveChanges();
                lb_bookingInfoCheck.Content = "Buchung erfolgreich";
                parent.ReadBooking();
            }
        }

        //Abfrage korrekte Eingabe Umbuchung
        private bool checkUmbuchungIsValid()
        {
            try
            {
                Convert.ToDecimal(txb_betragUmbuchung.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitte einen gültigen Betrag eingeben!");
                return false;
            }
            if (cb_kontoAusgehend.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte Konto angeben von dem Abgehoben wird!");
                return false;
            }
            if (cb_kontoEingehend.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte Konto angeben auf das eingezahlt wird!");
                return false;
            }
            if (datumUmbuchung.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein Datum auswählen!");
                return false;
            }

            return true;
        }
    }
}
