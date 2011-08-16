using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.Phone.Controls;

using LiveSynergy.Data;

namespace LiveSynergy
{
    public partial class DeviceListPage : PhoneApplicationPage
    {
        private static System.Collections.Generic.IEnumerable<PublicGrouping<string, Device>> devices;
        private bool ClickNoNavigate;

        public DeviceListPage()
        {
            InitializeComponent();

            ClickNoNavigate = false;
            LoadForAllList();
            All.SelectionChanged += OnPublicDeviceSelectedChanged;
        }

        private void LoadForAllList()
        {
            // all list
            devices = from device in App.ViewModel.AllTheDevice
                      group device by device.DeviceNameFirstLetter into c
                      orderby c.Key
                      select new PublicGrouping<string, Device>(c);

            All.GroupHeaderTemplate = this.Resources["GroupHeaderNameFirst"] as DataTemplate;
            All.ItemsSource = devices;
            All.GroupItemTemplate = this.Resources["GroupItemNameFirst"] as DataTemplate;

            // public device list
            var publicDevice = from device in App.ViewModel.AllTheDevice
                               where device.Classification == "PUBLIC"
                               select device;
            PublicList.ItemsSource = publicDevice;

            var SpaceList = from device in App.ViewModel.AllTheDevice
                            where device.Classification == "SPACE"
                            select device;
            PublicList.ItemsSource = publicDevice;

            var FriendList = from device in App.ViewModel.AllTheDevice
                             where device.Classification == "FRIEND"
                             select device;
            PublicList.ItemsSource = publicDevice;

            var privateDevice = from device in App.ViewModel.AllTheDevice
                                where device.Classification == "PRIVATE"
                                select device;
            PrivateList.ItemsSource = privateDevice;
        }

        private void newTaskAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddNewDevice.xaml", UriKind.Relative));
        }


        void OnPublicDeviceSelectedChanged(object sender, SelectionChangedEventArgs args)
        {
            if (ClickNoNavigate)
            {
                ClickNoNavigate = false;
                return;
            }
            Device device = All.SelectedItem as Device;
            if (device != null)
            {
                NavigationService.Navigate(new Uri("/Pages/ViewDevicePage.xaml?Index=" + App.ViewModel.AllTheDevice.IndexOf(device), UriKind.Relative));
            }
        }

        private void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                Device deviceForDelete = button.DataContext as Device;

                App.ViewModel.DeleteToDoItem(deviceForDelete);
            }

            LoadForAllList();
            this.Focus();
            ClickNoNavigate = true;
        }
        private void addToFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                Device deviceAddToFavorite = button.DataContext as Device;
                deviceAddToFavorite.IsNotFavorite = false;
                button.Opacity = 0.5;
                button.IsEnabled = false;
            }

            this.Focus();
            ClickNoNavigate = true;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Save changes to the database.
            App.ViewModel.SaveChangesToDB();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            LoadForAllList();
        }
    }

    public class BoolToOpacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
             return (bool)value ? 1 : 0.5;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(1) : false;
        }
    }
}