using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantBillCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItem> Items { get; set; } = new ObservableCollection<MenuItem>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadMenuItems();
            UpdateBill();

            // Requirement 3: User can update quantity for a particular item in the Data Grid => Bill is updated correspondingly
            Items.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (MenuItem newItem in e.NewItems)
                    {
                        newItem.PropertyChanged += (sender, args) =>
                        {
                            if (args.PropertyName == nameof(MenuItem.Quantity))
                            {
                                UpdateBill();
                            }
                        };
                    }
                }
            };
        }

        private void LoadMenuItems()
        {
            var beverages = new[]
            {
                new MenuItem("Soda", "Beverage", 1.95),
                new MenuItem("Tea", "Beverage", 1.5),
                new MenuItem("Coffee", "Beverage", 1.25),
                new MenuItem("Mineral Water", "Beverage", 2.95),
                new MenuItem("Juice", "Beverage", 2.5),
                new MenuItem("Milk", "Beverage", 1.5)
            };

            var appetizers = new[]
            {
                new MenuItem("Buffalo Wings", "Appetizer", 5.95),
                new MenuItem("Buffalo Fingers", "Appetizer", 6.95),
                new MenuItem("Potato Skins", "Appetizer", 8.95),
                new MenuItem("Nachos", "Appetizer", 8.95),
                new MenuItem("Mushroom Caps", "Appetizer", 10.95),
                new MenuItem("Shrimp Cocktail", "Appetizer", 12.95),
                new MenuItem("Chips and Salsa", "Appetizer", 6.95)
            };

            var mainCourses = new[]
            {
                new MenuItem("Seafood Alfredo", "Main Course", 15.95),
                new MenuItem("Chicken Alfredo", "Main Course", 13.95),
                new MenuItem("Chicken Picatta", "Main Course", 13.95),
                new MenuItem("Turkey Club", "Main Course", 11.95),
                new MenuItem("Lobster Pie", "Main Course", 19.95),
                new MenuItem("Prime Rib", "Main Course", 20.95),
                new MenuItem("Shrimp Scampi", "Main Course", 18.95),
                new MenuItem("Turkey Dinner", "Main Course", 13.95),
                new MenuItem("Stuffed Chicken", "Main Course", 14.95)
            };

            var desserts = new[]
            {
                new MenuItem("Apple Pie", "Dessert", 5.95),
                new MenuItem("Sundae", "Dessert", 3.95),
                new MenuItem("Carrot Cake", "Dessert", 5.95),
                new MenuItem("Mud Pie", "Dessert", 4.95),
                new MenuItem("Apple Crisp", "Dessert", 5.95)
            };

            cmbBeverage.ItemsSource = beverages;
            cmbAppetizer.ItemsSource = appetizers;
            cmbMainCourse.ItemsSource = mainCourses;
            cmbDessert.ItemsSource = desserts;
        }

        // Requirement 1: User can choose an item from any comboBox, the selected item is added to the DataGrid, and the bill is updated.
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedItem = e.AddedItems[0] as MenuItem;
                if (selectedItem != null)
                {
                    var existingItem = Items.FirstOrDefault(item => item.Name == selectedItem.Name);
                    if (existingItem == null)
                    {
                        // Requirement a: If the selected item(s) has not existed in the DataGrid,
                        // then your app should add a new row in the DataGrid
                        var newItem = new MenuItem(selectedItem.Name, selectedItem.Category, selectedItem.Price)
                        {
                            Quantity = 1
                        };
                        Items.Add(newItem);
                    }
                    else
                    {
                        // Requirement b: Otherwise, your app changes the quantity of the corresponding items
                        // rather than adding a new row
                        existingItem.Quantity++;
                    }
                    UpdateBill();
                }
            }
        }


        private void UpdateBill()
        {
            double subTotal = Items.Sum(item => item.Total);
            double taxRate = 0.13;
            double tax = subTotal * taxRate;
            double total = subTotal + tax;

            txtSubTotal.Text = subTotal.ToString("C");
            txtTax.Text = tax.ToString("C");
            txtTotal.Text = total.ToString("C");
        }

        // Requirement 2: User can remove the selected item from the Data Grid => Bill is updated as well
        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is MenuItem selectedItem)
            {
                Items.Remove(selectedItem);
                UpdateBill();
            }
        }

        // Requirement 4: After user clicks the Clear Bill button, the app clears DataGrid and restore the SubTotal, Tax and Total fields to $0.00
        private void ClearBill_Click(object sender, RoutedEventArgs e)
        {
            Items.Clear();
            txtSubTotal.Text = "$0.00";
            txtTax.Text = "$0.00";
            txtTotal.Text = "$0.00";
        }
    }
}
