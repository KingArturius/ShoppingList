using System.ComponentModel;

namespace ShoppingListProject.Models
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        private float quantity;
        private string unit;
        private string shop;
        private bool isOptional;
        private bool isBought;

        public bool IsPropertyChangedRegistered { get; set; } = false;

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public float Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public string Unit
        {
            get => unit;
            set
            {
                if (unit != value)
                {
                    unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        public string Shop
        {
            get => shop;
            set
            {
                if (shop != value)
                {
                    shop = value;
                    OnPropertyChanged(nameof(Shop));
                }
            }
        }

        public bool IsOptional
        {
            get => isOptional;
            set
            {
                if (isOptional != value)
                {
                    isOptional = value;
                    OnPropertyChanged(nameof(IsOptional));
                }
            }
        }

        public bool IsBought
        {
            get => isBought;
            set
            {
                if (isBought != value)
                {
                    isBought = value;
                    OnPropertyChanged(nameof(IsBought));
                }
            }
        }

        public Product(string name, float quantity, string unit, bool isOptional, string shop)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            IsOptional = isOptional;
            Shop = shop;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
