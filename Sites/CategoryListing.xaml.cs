using HaushaltsbuchWPF.Class;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HaushaltsbuchWPF.Sites
{
    /// <summary>
    /// Interaktionslogik für CategoryListing.xaml
    /// </summary>
    public partial class CategoryListing : Page
    {
        public readonly HBContext hbContext = new HBContext();

        public List<Category> categories { get; set; }

        public List<SubCategory> subCategories { get; set; }

        public List<SubCategory> subCategoriesAusgaben { get; set; }

        public List<SubCategory> subCategoriesUmbuchung { get; set; }

        public CategoryListing()
        {
            InitializeComponent();
            ReadDataCategoryEinahmen();

        }

        private void bt_newCategory_Click(object sender, RoutedEventArgs e)
        {
            NewCategory newCategory = new NewCategory(this);
            newCategory.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newCategory.Show();

        }


        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            subCategories = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Einnahme").First().SubCategories.ToList();
            ItemListCategoryEinnahmen.ItemsSource = subCategories;
        }

        private void lbl_Ausgaben_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            subCategories = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Ausgabe").First().SubCategories.ToList();
            ItemListCategoryAusgaben.ItemsSource = subCategories;
        }

        private void lbl_umbuchen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            subCategories = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Umbuchung").First().SubCategories.ToList();
            ItemListCategoryUmbuchung.ItemsSource = subCategories;
        }


        public void ReadDataCategoryEinahmen()
        {
            subCategories = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Einnahme").First().SubCategories.ToList();
            ItemListCategoryEinnahmen.ItemsSource = subCategories;
        }

        public void ReadDataCategoryAusgaben()
        {
            subCategoriesAusgaben = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Ausgabe").First().SubCategories.ToList();
            ItemListCategoryAusgaben.ItemsSource = subCategories;
        }

        public void ReadDataCategoryUmbuchung()
        {
            subCategoriesUmbuchung = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Umbuchung").First().SubCategories.ToList();
            ItemListCategoryUmbuchung.ItemsSource = subCategories;
        }

    }
}
