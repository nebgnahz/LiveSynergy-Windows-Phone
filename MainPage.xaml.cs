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
using ControlTiltEffect;

namespace LiveSynergy
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            FavoriteList.SelectionChanged += OnFavoriteListSelectionChanged;
        }
        private void LoadAll()
        {
            var FavoriteObject = from device in App.ViewModel.AllTheDevice
                                 where device.IsNotFavorite==false
                                 orderby device.DeviceNameFirstLetter
                                 select device;
            FavoriteList.ItemsSource = FavoriteObject;
            if (FavoriteObject.Count() == 0)
            {
                HelpText.Visibility = Visibility.Visible;
            }
            else
            {
                HelpText.Visibility = Visibility.Collapsed;
            }
        }
        void OnFavoriteListSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            Device device = FavoriteList.SelectedItem as Device;
            if (device != null)
            {
                NavigationService.Navigate(new Uri("/Pages/ViewDevicePage.xaml?Index=" + App.ViewModel.AllTheDevice.IndexOf(device), UriKind.Relative));
            }
        }
/*
        public static System.Collections.Generic.IEnumerable<PublicGrouping<string, Device>> deviceByLocation;
        public static System.Collections.Generic.IEnumerable<PublicGrouping<string, Device>> deviceByName;
        public List<Device> ListDevice;*/
        /*private void SortDevices()
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
        */
        
        private void OnCommandListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( CommandList.SelectedIndex==-1 )
            {
                return;
            }
            switch (CommandList.SelectedIndex)
            {
                
                case 0:
                    {
                        NavigationService.Navigate(new Uri("/Pages/AddNewDevice.xaml", UriKind.Relative));
                        break;
                    }
                case 1:
                    {
                        NavigationService.Navigate(new Uri("/Pages/DeviceListPage.xaml", UriKind.Relative));
                        break;
                    }
                case 2:
                    {
                        NavigationService.Navigate(new Uri("/Pages/SignInAndUp.xaml", UriKind.Relative));
                        break;
                    }
                default: break;
            }
            CommandList.SelectedIndex = -1;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            LoadAll();
        }
    }
}