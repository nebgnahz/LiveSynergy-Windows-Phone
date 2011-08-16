using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LiveSynergy.Data;

namespace LiveSynergy
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            AllDevice.LoadDevice();
            SortDevices();
            ListPublicDevice.SelectionChanged += OnPublicDeviceSelectedChanged;
        }

        public static System.Collections.Generic.IEnumerable<PublicGrouping<string, Device>> deviceByLocation;
        public static System.Collections.Generic.IEnumerable<PublicGrouping<string, Device>> deviceByName;
        public List<Device> ListDevice;
        private void SortDevices()
        {
            ListDevice = AllDevice.PublicDevice;
            deviceByLocation = from device in ListDevice
                               group device by device.DeviceLocation into c
                               orderby c.Key
                               select new PublicGrouping<string, Device>(c);

            deviceByName = from device in ListDevice
                           group device by device.DeviceNameFirstLetter into c
                           orderby c.Key
                           select new PublicGrouping<string, Device>(c);

            ListPublicDevice.ItemsSource = deviceByName;
        }

        void OnClickSignUp(object sender, EventArgs args)
        {
            this.NavigationService.Navigate(new Uri("/Pages/SignInAndUp.xaml", UriKind.Relative));
        }
        void OnClickSortByName(object sender, EventArgs args)
        {
            ListPublicDevice.GroupHeaderTemplate = this.Resources["GroupHeaderNameFirst"] as DataTemplate;
            ListPublicDevice.ItemsSource = deviceByName;
            ListPublicDevice.GroupItemTemplate = this.Resources["GroupItemNameFirst"] as DataTemplate;
        }
        void OnClickSortByLocation(object sender, EventArgs args)
        {
            ListPublicDevice.GroupHeaderTemplate = this.Resources["GroupHeaderLocation"] as DataTemplate;
            ListPublicDevice.ItemsSource = deviceByLocation;
            ListPublicDevice.GroupItemTemplate = this.Resources["GroupItemLocation"] as DataTemplate;
        }

        void OnPublicDeviceSelectedChanged(object sender, SelectionChangedEventArgs args)
        {
            Device device = ListPublicDevice.SelectedItem as Device;
            if (device != null)
            {
                NavigationService.Navigate(new Uri("/Pages/ViewDevicePage.xaml?Index=" + AllDevice.PublicDevice.IndexOf(device), UriKind.Relative));
            }
        }
        private double PreviousOpacity { get; set;}
        private void Appbar_StateChanged(object sender, ApplicationBarStateChangedEventArgs args)
        {
            var opacity = this.ApplicationBar.Opacity;
            this.ApplicationBar.Opacity = args.IsMenuVisible ? 1 : PreviousOpacity;
            this.PreviousOpacity = opacity;
        }
        void OnAppBarAddClick(object sender, EventArgs args)
        {
            NavigationService.Navigate(new Uri("/Pages/DeviceListPage.xaml", UriKind.Relative));
            /*NavigationService.Navigate(new Uri("/Pages/AddNewDevice.xaml", UriKind.Relative));*/
        }
        void OnAppbarSettingClick(object sender, EventArgs args)
        {
        }
    }
}