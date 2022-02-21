using HaushaltsbuchWPF.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HaushaltsbuchWPF.Sites
{
    /// <summary>
    /// Interaktionslogik für NewOrder.xaml
    /// </summary>
    public partial class NewOrder : Window
    {
        public readonly HBContext hBContext = new HBContext();

        public List<Konto> kontos { get; set; }

        public List<Category> categories = new List<Category>();

        public List<SubCategory> subCategories { get; set; }

        public List<StandingOrder> standingOrders { get; set; }

        public NewOrder()
        {
            InitializeComponent();
            ListComboboxNewOrderEinnahme();
            ListComboboxNewOrderEinnahmeCategory();
            ListComboboxNewOrderAusgaben();
            ListComboboxNewOrderAusgabenCategory();
        }


        //Einnahmen Buchen und Combobox Liste Category
        public void ListComboboxNewOrderEinnahme()
        {
            kontos = hBContext.Konto.ToList();
            cb_kontenEinnahme.ItemsSource = kontos;
        }

        public void ListComboboxNewOrderEinnahmeCategory()
        {
            subCategories = hBContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Einnahme").First().SubCategories.ToList();
            cb_rubrikEinnahme.ItemsSource = subCategories;
        }


        //Ausgabe Buchen und Combobox Liste Category
        public void ListComboboxNewOrderAusgaben()
        {
            kontos = hBContext.Konto.ToList();
            cb_kontenAusgaben.ItemsSource = kontos;
        }

        public void ListComboboxNewOrderAusgabenCategory()
        {
            subCategories = hBContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Ausgabe").First().SubCategories.ToList();
            cb_rubrikAusgabe.ItemsSource = subCategories;
        }


        //Dauerauftrag Buchen Einnahme / Ausgaben
        private void btn_dauerauftragEinnahmeBuchen_Click(object sender, RoutedEventArgs e)
        {
            DauerauftragEinname();
        }

        private void btn_dauerauftragAusgabeBuchen_Click(object sender, RoutedEventArgs e)
        {
            DauerauftragAusgabe();
        }


        //Prüfen ob alle Felder ausgefpllt sind
        private bool checkDauerauftragAusgabeIsValid()
        {
            try
            {
                Convert.ToDecimal(txb_newOrderAusgabeAmount.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitte einen gültigen Betrag eingeben!");
                return false;
            }

            if (String.IsNullOrWhiteSpace(txb_bezeichnungBuchungAusgabeDauer.Text))
            {
                MessageBox.Show("Bitte einen gültigen Bezeichnung eingeben");
                return false;
            }
            if (cb_kontenAusgaben == null)
            {
                MessageBox.Show("Bitte ein Konto auswählen!");
                return false;
            }
            if (cb_rubrikAusgabe.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte eine Kategorie auswählen!");
                return false;
            }
            if (txb_notizNewOrderAusgabe == null)
            {
                MessageBox.Show("Bitte geben sie eine kurze Notiz dazu!");
                return false;
            }


            if (dbr_buchungDatumStartNewOrderAusgabe.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein Start Datum auswählen!");
                return false;
            }
            if (dbr_buchungWiederholungNewOrderAusgabe.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte wiederholungs auswählen");
                return false;
            }
            if (dbr_buchungDatumBeendenNewOrderAusgabe.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein End Datum auswählen!");
                return false;
            }

            return true;
        }

        private bool checkDauerauftragEinnahmeIsValid()
        {
            try
            {
                Convert.ToDecimal(txb_dauerauftragEinnahmeBetrag.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitte einen gültigen Betrag eingeben!");
                return false;
            }

            if (String.IsNullOrWhiteSpace(txb_bezeichnungBuchungEinnahmeDauer.Text))
            {
                MessageBox.Show("Bitte einen gültigen Bezeichnung eingeben");
                return false;
            }
            if (cb_kontenEinnahme == null)
            {
                MessageBox.Show("Bitte ein Konto auswählen!");
                return false;
            }
            if (cb_rubrikEinnahme.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte eine Kategorie auswählen!");
                return false;
            }
            if (txb_dauerauftragEinnahmeNotiz == null)
            {
                MessageBox.Show("Bitte geben sie eine kurze Notiz dazu!");
                return false;
            }


            if (dp_nextBooking.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein Start Datum auswählen!");
                return false;
            }
            if (cb_buchungWiederholungEinnahme.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte wiederholungs auswählen");
                return false;
            }
            if (dp_buchungsDauerEinnahme.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein End Datum auswählen!");
                return false;
            }

            return true;
        }


        //Buttons zum Schliessen des Fenster
        private void btn_closeNewOrder_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_closeNewOrderEin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_closeNewOrderAus_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //Methoden
        private void DauerauftragAusgabe()
        {
            if (checkDauerauftragAusgabeIsValid())
            {
                var booking = new StandingOrder();
                booking.OderName = txb_bezeichnungBuchungAusgabeDauer.Text;
                booking.Konto = kontos[cb_kontenAusgaben.SelectedIndex];
                booking.SubCategory = subCategories[cb_rubrikAusgabe.SelectedIndex];
                booking.Note = txb_notizNewOrderAusgabe.Text;
                booking.Amount = -Math.Abs(Convert.ToDecimal(txb_newOrderAusgabeAmount.Text));
                booking.StandingOrderDateStart = (DateTime) dbr_buchungDatumStartNewOrderAusgabe.SelectedDate;
                booking.Repetition = dbr_buchungWiederholungNewOrderAusgabe.SelectedIndex.ToString();
                booking.StandingOrderDateEnd = (DateTime) dbr_buchungDatumBeendenNewOrderAusgabe.SelectedDate;

                hBContext.StandingOrder.Add(booking);
                hBContext.SaveChanges();
            }
            lb_bookingInfoCheckAusgaben.Content = "Buchung erfolgreich";
        }

        private void DauerauftragEinname()
        {
            if (checkDauerauftragEinnahmeIsValid())
            {
                var booking = new StandingOrder();
                booking.OderName = txb_bezeichnungBuchungEinnahmeDauer.Text;
                booking.Konto = kontos[cb_kontenEinnahme.SelectedIndex];
                booking.SubCategory = subCategories[cb_rubrikEinnahme.SelectedIndex];
                booking.Note = txb_dauerauftragEinnahmeNotiz.Text;
                booking.Amount = Math.Abs(Convert.ToDecimal(txb_dauerauftragEinnahmeBetrag.Text));
                booking.StandingOrderDateStart = (DateTime) dp_nextBooking.SelectedDate;
                booking.Repetition = cb_buchungWiederholungEinnahme.SelectedIndex.ToString();
                booking.StandingOrderDateEnd = (DateTime) dp_buchungsDauerEinnahme.SelectedDate;

                hBContext.StandingOrder.Add(booking);
                hBContext.SaveChanges();
            }
            lb_bookingInfoCheckEinnahmen.Content = "Eintrag erfolgreich";
        }
    }
}
