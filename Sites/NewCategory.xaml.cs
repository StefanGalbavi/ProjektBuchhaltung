using HaushaltsbuchWPF.Class;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HaushaltsbuchWPF.Sites
{
    /// <summary>
    /// Interaktionslogik für NewCategory.xaml
    /// </summary>
    public partial class NewCategory : Window
    {
        HBContext hbContext = new HBContext();

        public List<Category> categories = new List<Category>();

        public List<SubCategory> subCategories = new List<SubCategory>();

        private CategoryListing parent;

        public NewCategory(CategoryListing parent)
        {
            InitializeComponent();
            this.parent = parent;
            ListComboboxCategorie();



        }

        public void ListComboboxCategorie()
        {
            using (HBContext hb = new HBContext())
            {
                categories = hbContext.Category.ToList();
                cb_category.ItemsSource = categories;
            }
        }

        private void bt_newSubCategory_Click(object sender, RoutedEventArgs e)
        {
            SubCategory subCategory = new SubCategory()
            {
                SubCategoryName = SubCategoryName.Text,
                Category = categories[cb_category.SelectedIndex]
            };

            hbContext.SubCategory.Add(subCategory);
            hbContext.SaveChanges();
            parent.ReadDataCategoryEinahmen();
            parent.ReadDataCategoryAusgaben();
            parent.ReadDataCategoryUmbuchung();
            base.Close();
        }

        private void bt_closeNewSubCategory_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
