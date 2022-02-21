using HaushaltsbuchWPF.Sites;
using System.Windows;

namespace HaushaltsbuchWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BookingListing? bookingListing;

        private CategoryListing? categoryListing;

        private KontoListing? kontoListing;

        private StandingOrderListing? standingOrderListing;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            MainContent.Content = new BookingListing();
        }

        private void bt_bookListings_Click(object sender, RoutedEventArgs e)
        {
            if (bookingListing == null)
            {
                bookingListing = new BookingListing();
            }
            MainContent.Content = bookingListing;
        }

        private void bt_kontoListings_Click(object sender, RoutedEventArgs e)
        {
            if (kontoListing == null)
            {
                kontoListing = new KontoListing();
            }
            MainContent.Content = kontoListing;

        }

        private void bt_standingOrderListings_Click(object sender, RoutedEventArgs e)
        {
            if (standingOrderListing == null)
            {
                standingOrderListing = new StandingOrderListing();
            }
            MainContent.Content = standingOrderListing;
        }

        private void bt_reportListings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReportListing();
        }

        private void btn_closeProgramm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_categoryListing_Click(object sender, RoutedEventArgs e)
        {
            if (categoryListing == null)
            {
                categoryListing = new CategoryListing();
            }
            MainContent.Content = categoryListing;
        }
    }
}
