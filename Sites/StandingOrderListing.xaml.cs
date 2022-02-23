using HaushaltsbuchWPF.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HaushaltsbuchWPF.Sites
{
    /// <summary>
    /// Interaktionslogik für StandingOrderListing.xaml
    /// </summary>
    public partial class StandingOrderListing : Page
    {
        public readonly HBContext hBContext = new HBContext();

        public List<StandingOrder> standingOrders { get; set; }

        public List<Booking> bookings { get; set; }

        public List<Konto> kontos { get; set; }

        public List<SubCategory> subCategories { get; set; }

        private DateTime selectedDate;

        public StandingOrderListing()
        {
            InitializeComponent();
            selectedDate = DateTime.Now;
            ReadStandingOrderList();
            //datePicker.Text = DateTime.Now.Date.ToString();
        }

        private void btn_newOder_Click(object sender, RoutedEventArgs e)
        {
            NewOrder newOrder = new NewOrder();
            newOrder.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newOrder.Show();
        }

        public void ReadStandingOrderList()
        {

            standingOrders = hBContext.StandingOrder.Include(b => b.Konto).Include(b => b.SubCategory).ToList();

            List<ListViewItem> list = new List<ListViewItem>();

            for (int i = 0; i < standingOrders.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Content = standingOrders[i];
                if (standingOrders[i].Amount < 0)
                {
                    item.Foreground = Brushes.Red;

                }
                list.Add(item);
            }
            ItemListStandongOrders.ItemsSource = list;
        }



    }
}
