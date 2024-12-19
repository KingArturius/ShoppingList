using System.Collections.ObjectModel;
using ShoppingListProject.Models;

namespace ShoppingListProject.Views;

public partial class AddShop : ContentView
{
    ObservableCollection<Category> Categories { get; set; }
    ObservableCollection<string> Shops { get; set; }

    public AddShop(ObservableCollection<Category> categories, ObservableCollection<string> shops)
    {
        InitializeComponent();
        Categories = categories;
        Shops = shops;
    }

    private async void OnAddShopClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(shopName.Text))
        {
            await Application.Current.MainPage.DisplayAlert("B³¹d", "Nazwa sklepu nie mo¿e byæ pusta.", "OK");
        }
        else
        {
            Shops.Add(shopName.Text);
            shopName.Text = string.Empty;

            OnGoToMenuClicked(sender, e);
        }
    }

    private void OnGoToMenuClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new MainPage(Categories, Shops));
    }
}
