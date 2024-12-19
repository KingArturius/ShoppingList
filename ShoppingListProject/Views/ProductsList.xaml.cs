using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ShoppingListProject.Models;

namespace ShoppingListProject.Views;

public partial class ProductsList : ContentView
{
    public ObservableCollection<Product> AllProducts { get; private set; }
    private ObservableCollection<Category> Categories { get; set; }
    private ObservableCollection<string> Shops { get; set; }

    public ProductsList(ObservableCollection<Category> categories, ObservableCollection<string> shops)
    {
        InitializeComponent();
        Categories = categories;
        Shops = shops;

        AllProducts = new ObservableCollection<Product>();

        foreach (var category in Categories)
        {
            category.PropertyChanged += Category_PropertyChanged;
            foreach (var product in category.Products)
            {
                product.PropertyChanged += Product_PropertyChanged;
            }
        }

        UpdateProductsList();
        BindingContext = this;
    }

    private void Category_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Category.Products))
        {
            UpdateProductsList();
        }
    }

    private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Product.IsBought) || e.PropertyName == nameof(Product.Quantity))
        {
            UpdateProductsList();
        }
    }

    private void UpdateProductsList()
    {
        var sortedProducts = Categories
            .SelectMany(category => category.Products)
            .OrderBy(product => product.IsBought)
            .ThenBy(product => product.Name)
            .ToList();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            AllProducts.Clear();
            foreach (var product in sortedProducts)
            {
                AllProducts.Add(product);
            }
        });
    }

    private async void OnProductDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Product product)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Potwierdzenie",
                $"Czy na pewno chcesz usun¹æ produkt \"{product.Name}\"?",
                "Tak",
                "Nie");

            if (confirm)
            {
                foreach (var category in Categories)
                {
                    if (category.Products.Contains(product))
                    {
                        category.Products.Remove(product);
                        break;
                    }
                }

                UpdateProductsList();
            }
        }
    }

    private void OnDecreaseQuantityClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Product product)
        {
            if (product.Quantity > 0)
            {
                product.Quantity -= 1;
            }
        }
    }

    private void OnIncreaseQuantityClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Product product)
        {
            product.Quantity += 1;
        }
    }

    private void OnGoToMenuClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new MainPage(Categories, Shops));
    }
}
