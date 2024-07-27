using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBillCalculator
{
    public class MenuItem : INotifyPropertyChanged
    {
        private int _quantity;

        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public double Total => Price * Quantity;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MenuItem(string name, string category, double price)
        {
            Name = name;
            Category = category;
            Price = price;
            Quantity = 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
