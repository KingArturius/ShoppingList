using ShoppingListProject.Models;
using System.Collections.ObjectModel;

namespace ShoppingListProject.Views;

public partial class CategoryList : ContentView
{
    ObservableCollection<Category> Categories { get; set; }
    ObservableCollection<string> Shops { get; set; }

    public CategoryList(ObservableCollection<Category> categories, ObservableCollection<string> shops)
    {
        InitializeComponent();
        Categories = categories;
        ResetListExpanding();
        Shops = shops;

        BindingContext = this;
        categoriesList.ItemsSource = Categories;
    }

    private async void OnCategoryDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Category category)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Potwierdzenie",
                $"Czy na pewno chcesz usun¹æ kategoriê \"{category.Name}\" i wszystkie jej produkty?",
                "Tak",
                "Nie");

            if (confirm)
            {
                Categories.Remove(category);
            }
        }
    }
    private void ResetListExpanding()
    {
        foreach (var category in Categories)
        {
            category.IsExpanded = false;
        }
    }


    private void OnCategoryExpandClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Category category)
        {
            category.IsExpanded = !category.IsExpanded;
        }
    }

    private void OnGoToMenuClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new MainPage(Categories, Shops));
    }
}
