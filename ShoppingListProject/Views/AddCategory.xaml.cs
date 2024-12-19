using ShoppingListProject.Models;
using System.Collections.ObjectModel;

namespace ShoppingListProject.Views;
public partial class AddCategory : ContentView
{
    ObservableCollection<Category> Categories { get; set; }
    ObservableCollection<string> Shops { get; set; }
    public AddCategory(ObservableCollection<Category> categories, ObservableCollection<string> shops)
	{
		InitializeComponent();
        Categories = categories;
        Shops = shops;
    }

    private async void OnAddCategoryClicked(object sender, EventArgs e)
    {
        if(CategoryName.Text==null)
        {
            await Application.Current.MainPage.DisplayAlert("B³¹d", "Nie mo¿na wykonaæ tej akcji.", "OK");
        }
        else
        {
            Categories.Add(new Category(CategoryName.Text));
            OnGoToMenuClicked(sender, e);
        }
    }

    private void OnGoToMenuClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new MainPage(Categories, Shops));
    }
}