using HaushaltsbuchWPF.Class;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HaushaltsbuchWPF.Sites
{
    /// <summary>
    /// Interaktionslogik für KontoListing.xaml
    /// </summary>
    public partial class KontoListing : Page
    {
        public readonly HBContext hbContext = new HBContext();

        public List<Konto> myKontos { get; set; }

        public KontoListing()
        {
            InitializeComponent();
            ReadKonto();
        }

        private void bt_newKontoCreate_Click(object sender, RoutedEventArgs e)
        {
            NewKonto newKonto = new NewKonto(this);
            newKonto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newKonto.Show();
        }

        public void ReadKonto()
        {
            myKontos = hbContext.Konto.ToList();
            ItemListKontos.ItemsSource = myKontos;
        }

    }
}
