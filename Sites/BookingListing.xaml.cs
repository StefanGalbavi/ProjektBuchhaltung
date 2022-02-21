using HaushaltsbuchWPF.Class;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Color = MigraDoc.DocumentObjectModel.Color;
using Page = System.Windows.Controls.Page;

namespace HaushaltsbuchWPF.Sites
{
    /// <summary>
    /// Interaktionslogik für BookingListing.xaml
    /// </summary>
    public partial class BookingListing : Page
    {
        public readonly HBContext hBContext = new HBContext();

        public List<Booking> bookings { get; set; } = new List<Booking>();

        //public List<BookingListing>? bookingListings { get; set; }

        public List<Konto> kontos { get; set; }

        public List<Category> categories { get; set; }

        public List<SubCategory> subCategories { get; set; } = new List<SubCategory>();

        public DateTime selectedDate { get; set; }

        public BookingListing()
        {
            InitializeComponent();
            selectedDate = DateTime.Now;
            ReadBooking();
            datePicker.Text = DateTime.Now.Date.ToString();
            ListComboboxBookingKontoChange();
        }

        //Öffnet neues Fenster für neue Buchungen
        private void btn_newBookingEinnahme_Click(object sender, RoutedEventArgs e)
        {
            NewBooking newBooking = new NewBooking(this);
            newBooking.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newBooking.Show();
        }


        //Listet alle Buchungen auf und Rechnet die Summe aus
        public void ReadBooking()
        {
            if (hBContext == null || selectedDate == null)
            {
                return;
            }

            bookings = hBContext.Booking.Where(x => x.Date.Month == selectedDate.Month && x.Date.Year == selectedDate.Year)?.Include(b => b.Konto)?.Include(b => b.SubCategory)?.OrderBy(x => x.Date).ToList();
            decimal sum = 0;
            //calculate sum
            foreach (Booking booking in bookings)
            {
                sum += booking.Amount;
            }

            txb_summeMonat.Text = sum.ToString();


            ItemListBookings.ItemsSource = bookings;


            ReadLastMonthTransfer();

            SumRestMoney();
        }

        private void ReadLastMonthTransfer()
        {
            var firstOfThisMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            var sum = hBContext.Booking.Include(b => b.Konto).Include(b => b.SubCategory).Where(x => x.Date < firstOfThisMonth).Sum(b => b.Amount);
            lbl_uebertragVormonat.Content = sum;
        }

        private void SumRestMoney()
        {
            bookings = hBContext.Booking.Include(b => b.Konto).Include(b => b.SubCategory).OrderBy(x => x.Date).Where(x => x.Date.Month == selectedDate.Month && x.Date.Year == selectedDate.Year).ToList();
            decimal sum1 = 0;
            //calculate sum
            foreach (Booking booking in bookings)
            {
                sum1 += booking.Amount;
            }

            var firstOfThisMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            var sum2 = hBContext.Booking.Include(b => b.Konto).Include(b => b.SubCategory).Where(x => x.Date < firstOfThisMonth).Sum(b => b.Amount);
            var summeRest = sum1 + sum2;
            txb_summeMonat2.Text = summeRest.ToString();
        }



        //ComboBox Select Alle Konten
        public void ListComboboxBookingKontoChange()
        {

            kontos = hBContext.Konto.ToList();
            Konto k = new Konto();
            k.KontoName = "Alle Buchungen";
            kontos.Add(k);

            cb_shwoKonto.SelectedIndex = kontos.Count - 1;

            cb_shwoKonto.ItemsSource = kontos;
        }


        // Save the selected employee's name, because we will remove
        // the employee's name from the list.
        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            ComboBox comboBox = (ComboBox) sender;

            Konto selectedKonto = (Konto) cb_shwoKonto.SelectedItem;

            int count = 0;
            int resultIndex = -1;
            if (selectedKonto.KontoId != 0)
            {
                bookings = hBContext.Booking.Include(b => b.Konto).Include(b => b.SubCategory).OrderBy(x => x.Date).Where(x => x.Konto != null && x.Konto.KontoId == selectedKonto.KontoId).Where(x => x.Date.Month == selectedDate.Month && x.Date.Year == selectedDate.Year).ToList();
                ItemListBookings.ItemsSource = bookings;

                decimal sum = 0;
                foreach (Booking booking in bookings)
                {
                    sum += booking.Amount;
                }

                txb_summeMonat.Text = sum.ToString();
            }
            else
            {
                ReadBooking();
            }
        }


        //DatePicker
        private void DatePicker_Opened(object? sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            DatePicker datepicker = (DatePicker) sender;
            Popup popup = (Popup) datepicker.Template.FindName("PART_Popup", datepicker);
            Calendar cal = (Calendar) popup.Child;
            cal.DisplayModeChanged += Calender_DisplayModeChanged;
            cal.DisplayMode = CalendarMode.Decade;
        }

        private void Calender_DisplayModeChanged(object? sender, CalendarModeChangedEventArgs e)
        {
            if (sender == null)
                return;
            Calendar calendar = (Calendar) sender;

            if (calendar.DisplayMode == CalendarMode.Month)
            {
                calendar.SelectedDate = calendar.DisplayDate;

                datePicker.IsDropDownOpen = false;

                selectedDate = calendar.DisplayDate;

                ReadBooking();
            }
        }

        private void btn_pdfExport_Click(object sender, RoutedEventArgs e)
        {
            CreatePdfBooking();

        }

        private void CreatePdfBooking()
        {
            MigraDoc.DocumentObjectModel.Document doc = new MigraDoc.DocumentObjectModel.Document();
            MigraDoc.DocumentObjectModel.Section sec = doc.AddSection();
            sec.PageSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
            sec.PageSetup.LeftMargin = 50;
            sec.PageSetup.TopMargin = 40;
            sec.PageSetup.RightMargin = 40;
            sec.PageSetup.BottomMargin = 40;

            //Seitenzahle Automatisch gnerieren
            Paragraph paragraph1 = new Paragraph();
            paragraph1.AddText("Page ");
            paragraph1.AddPageField();
            paragraph1.AddText(" of ");
            paragraph1.AddNumPagesField();
            sec.Footers.Primary.Add(paragraph1);

            //sec.AddParagraph("List of Bookings " + selectedDate.ToString("MMMM.yyyy"));
            //sec.AddParagraph();
            Paragraph paragraph = sec.AddParagraph();
            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
            paragraph.Format.Font.Size = 18;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddFormattedText("Buchungen vom " + selectedDate.ToString("MMMM yyyy"));
            sec.AddParagraph();
            sec.AddParagraph();

            //Table
            MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            table.Borders.Width = 0.2;
            table.BottomPadding = 5;
            table.TopPadding = 5;
            table.LeftPadding = 3;

            //Column
            MigraDoc.DocumentObjectModel.Tables.Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            column.Format.Font.Size = 12;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            column.Format.Font.Size = 12;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            column.Format.Font.Size = 12;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Right;
            column.Format.Font.Size = 12;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            column.Format.Font.Size = 12;

            //Header of Table
            MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
            MigraDoc.DocumentObjectModel.Tables.Cell cell = row.Cells[0];
            cell.AddParagraph("Datum");
            cell.Format.Font.Bold = true;

            cell = row.Cells[1];
            cell.AddParagraph("Konto");
            cell.Format.Font.Bold = true;

            cell = row.Cells[2];
            cell.AddParagraph("Kategorie");
            cell.Format.Font.Bold = true;

            cell = row.Cells[3];
            cell.AddParagraph("Betrag");
            cell.Format.Font.Bold = true;

            cell = row.Cells[4];
            cell.AddParagraph("Notiz");
            cell.Format.Font.Bold = true;


            //def Rows of Table
            foreach (Booking elem in bookings)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(elem.Date.ToString("dd.MM.yyyy"));

                cell = row.Cells[1];
                cell.AddParagraph(elem.Konto.KontoName);


                cell = row.Cells[2];
                if (elem.SubCategory != null)
                {
                    cell.AddParagraph(elem.SubCategory.SubCategoryName);
                }

                cell = row.Cells[3];
                if (elem.Amount < 0)
                {
                    cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Color.Parse("Red");
                }
                cell.AddParagraph(elem.Amount.ToString());


                cell = row.Cells[4];
                if (elem.Note != null)
                {
                    cell.AddParagraph(elem.Note);
                }


                //Jede zweite Zeile einfärben
                for (var i = 1; i < table.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        table.Rows[i].Shading.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(216, 216, 216);
                    }
                }


            }

            doc.LastSection.Add(table);


            //rendering doc
            MigraDoc.Rendering.PdfDocumentRenderer docRend = new MigraDoc.Rendering.PdfDocumentRenderer(false);
            docRend.Document = doc;
            docRend.RenderDocument();

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "PDF documents (.pdf)|*.pdf"; // Filter files by extensio

            string fileName = "";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                // Save document 
                fileName = dlg.FileName;
                docRend.PdfDocument.Save(fileName);
            }

            if (File.Exists(fileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
        }

    }



}
