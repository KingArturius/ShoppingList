using ShoppingListProject.Models;
using ShoppingListProject.Views;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace ShoppingListProject
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Category> Categories { get; set; }
        ObservableCollection<string> Shops { get; set; }

        private const string ShoppingListFileName = "shoppingList.json";

        public MainPage()
        {
            InitializeComponent();
            LoadDataAsync();

            if (Shops == null || Shops.Count == 0)
            {
                Shops = new ObservableCollection<string>
                {
                    "Dowolny",
                    "Biedronka",
                    "Lidl"
                };
            }

            if (Categories == null || Categories.Count == 0)
            {
                Categories = new ObservableCollection<Category>
                {
                    new Category("Nabiał"),
                    new Category("Owoce"),
                    new Category("Warzywa"),
                    new Category("AGD"),
                    new Category("RTV"),
                };
            }
        }

        public MainPage(ObservableCollection<Category> categories, ObservableCollection<string> shops)
        {
            InitializeComponent();
            Categories = categories;
            Shops = shops;
        }

        private void onProductsListClicked(object sender, EventArgs e)
        {
            Content = new ProductsList(Categories, Shops);
        }

        private void OnCategoryListClicked(object sender, EventArgs e)
        {
            Content = new CategoryList(Categories, Shops);
        }

        private void OnShoppingListClicked(object sender, EventArgs e)
        {
            Content = new ShoppingList(Categories, Shops);
        }

        private void OnAddProductClicked(object sender, EventArgs e)
        {
            Content = new AddProduct(Categories, Shops);
        }

        private void OnAddCategoryClicked(object sender, EventArgs e)
        {
            Content = new AddCategory(Categories, Shops);
        }

        private void OnAddShopClicked(object sender, EventArgs e)
        {
            Content = new AddShop(Categories, Shops);
        }

        private async void OnSaveListClicked(object sender, EventArgs e)
        {
            await SaveDataAsync();
        }

        private async Task SaveDataAsync()
        {
            try
            {
                var shoppingListPath = Path.Combine(FileSystem.AppDataDirectory, ShoppingListFileName);

                var shoppingListData = new ShoppingListData
                {
                    Categories = Categories,
                    Shops = Shops
                };

                await SaveToJson(shoppingListPath, shoppingListData);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Wystąpił problem podczas zapisywania danych: {ex.Message}", "OK");
            }
        }

        public class ShoppingListData
        {
            public ObservableCollection<Category> Categories { get; set; }
            public ObservableCollection<string> Shops { get; set; }
        }

        private async Task SaveToJson<T>(string filePath, T data)
        {
            var json = JsonSerializer.Serialize(data);
            await File.WriteAllTextAsync(filePath, json);
            await DisplayAlert("Sukces", $"Dane zostały zapisane w pliku:\n {filePath}", "OK");
        }

        private async void LoadDataAsync()
        {
            try
            {
                var shoppingListPath = Path.Combine(FileSystem.AppDataDirectory, ShoppingListFileName);

                if (File.Exists(shoppingListPath))
                {
                    var shoppingListJson = await File.ReadAllTextAsync(shoppingListPath);
                    var shoppingListData = JsonSerializer.Deserialize<ShoppingListData>(shoppingListJson);

                    if (shoppingListData != null)
                    {
                        Categories = shoppingListData.Categories ?? new ObservableCollection<Category>();
                        Shops = shoppingListData.Shops ?? new ObservableCollection<string>();
                    }
                    else
                    {
                        Categories = new ObservableCollection<Category>();
                        Shops = new ObservableCollection<string>();
                    }
                }
                else
                {
                    Categories = new ObservableCollection<Category>();
                    Shops = new ObservableCollection<string>();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Wystąpił problem podczas wczytywania danych: {ex.Message}", "OK");
                Categories = new ObservableCollection<Category>();
                Shops = new ObservableCollection<string>();
            }
        }

        private async void OnExportListClicked(object sender, EventArgs e)
        {
            try
            {
                string exportPath = string.Empty;

                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    exportPath = Path.Combine(FileSystem.AppDataDirectory, ShoppingListFileName);
                }
                else if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    exportPath = Path.Combine(downloadsFolder, "Downloads", ShoppingListFileName);
                }
                else
                {
                    exportPath = Path.Combine(FileSystem.AppDataDirectory, ShoppingListFileName);
                }

                var shoppingListData = new ShoppingListData
                {
                    Categories = Categories,
                    Shops = Shops
                };

                var shoppingListJson = JsonSerializer.Serialize(shoppingListData);

                await File.WriteAllTextAsync(exportPath, shoppingListJson);

                await DisplayAlert("Sukces", $"Plik został zapisany w folderze: {exportPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Wystąpił problem podczas eksportowania danych: {ex.Message}", "OK");
            }
        }

        private async void OnImportListClicked(object sender, EventArgs e)
        {
            try
            {
                var fileResult = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Wybierz plik do zaimportowania",
                });

                if (fileResult != null && Path.GetExtension(fileResult.FileName).ToLower() == ".json")
                {
                    var json = await File.ReadAllTextAsync(fileResult.FullPath);

                    var shoppingListData = JsonSerializer.Deserialize<ShoppingListData>(json);

                    if (shoppingListData != null)
                    {
                        Categories = shoppingListData.Categories ?? new ObservableCollection<Category>();
                        Shops = shoppingListData.Shops ?? new ObservableCollection<string>();
                        await DisplayAlert("Sukces", $"Plik {fileResult.FileName} został zaimportowany.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Błąd", "Błąd deserializacji pliku.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Błąd", "Proszę wybrać plik JSON.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Wystąpił problem podczas importowania danych: {ex.Message}", "OK");
            }
        }
    }
}
