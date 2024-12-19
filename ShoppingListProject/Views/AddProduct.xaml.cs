using ShoppingListProject.Models;
using System.Collections.ObjectModel;

namespace ShoppingListProject.Views
{
    public partial class AddProduct : ContentView
    {
        ObservableCollection<Category> Categories { get; set; }
        ObservableCollection<string> Shops {  get; set; }

        public AddProduct(ObservableCollection<Category> categories, ObservableCollection<string> shops)
        {
            InitializeComponent();
            Categories = categories;
            Shops = shops;
            productCategory.ItemsSource = Categories.Select(c => c.Name).ToList();
            shopPicker.ItemsSource = Shops;
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            if(productName.Text==null || !(float.TryParse(productQuantity.Text, out var result) && result >= 0) || productUnit.SelectedItem==null || productCategory.SelectedItem == null || shopPicker.SelectedItem==null)
            {
                await Application.Current.MainPage.DisplayAlert("B³¹d", "Nazwa ¿adna wartoœæ nie mo¿e byæ pusta.", "OK");
            }
            else
            {
                string name = productName.Text;
                float quantity = result;
                string unit = productUnit.SelectedItem as string;
                string category = productCategory.SelectedItem as string;
                bool isOptional = productIsOptional.IsChecked;
                string shop = shopPicker.SelectedItem as string;
                Product newProduct = new Product(name, quantity, unit, isOptional, shop);

                var categoryWithNewProduct = Categories.FirstOrDefault(c => c.Name == category);

                categoryWithNewProduct.Products.Add(newProduct);

                OnGoToMenuClicked(sender, e);
            }
        }
        private void OnGoToMenuClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage(Categories, Shops));
        }
    }
}
