using ShoppingListProject.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShoppingListProject.Views
{
    public partial class ShoppingList : ContentView
    {
        ObservableCollection<Category> Categories { get; set; }
        ObservableCollection<Product> SortedProducts = new ObservableCollection<Product>();
        ObservableCollection<string> Shops { get; set; }

        public ShoppingList(ObservableCollection<Category> categories, ObservableCollection<string> shops)
        {
            InitializeComponent();
            Categories = categories;
            Shops = shops;

            shopPicker.SelectedItem = "Dowolny";
            shopPicker.ItemsSource = Shops;
            BindingContext = this;

            foreach (var category in Categories)
            {
                category.Products.CollectionChanged += OnProductsChanged;

                foreach (var product in category.Products)
                {
                    product.PropertyChanged += OnProductPropertyChanged;
                }
            }

            SortProducts();
            productsList.ItemsSource = SortedProducts;
        }

        private void SortProducts()
        {
            SortedProducts.Clear();
            foreach (var category in Categories)
            {
                foreach (var product in category.Products)
                {
                    if (!product.IsBought)
                    {
                        SortedProducts.Add(product);
                    }
                }
            }
        }

        private void OnShopPicked(object sender, EventArgs e)
        {
            var selectedShop = shopPicker.SelectedItem as string;

            if (selectedShop == "Dowolny")
            {
                SortProducts();
            }
            else
            {
                SortedProducts.Clear();
                foreach (var category in Categories)
                {
                    foreach (var product in category.Products)
                    {
                        if (!product.IsBought && (product.Shop == selectedShop || product.Shop == "Dowolny"))
                        {
                            SortedProducts.Add(product);
                        }
                    }
                }
            }

            productsList.ItemsSource = SortedProducts;
        }

        private void OnProductsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Product product in e.OldItems)
                {
                    product.PropertyChanged -= OnProductPropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (Product product in e.NewItems)
                {
                    product.PropertyChanged += OnProductPropertyChanged;
                }
            }

            SortProducts();
        }

        private void OnProductPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Product.IsBought) || e.PropertyName == nameof(Product.IsOptional))
            {
                SortProducts();
            }
        }

        private void OnGoToMenuClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage(Categories, Shops));
        }
    }
}
