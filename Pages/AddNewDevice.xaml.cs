using System;
using System.Collections.Generic;
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
using LiveSynergy.Data;

namespace LiveSynergy.Pages
{
    public partial class AddNewDevice : PhoneApplicationPage
    {
        static readonly string[] category = { "space", "device" };

        static readonly string[] types = { "Light", "Computer", "Television" };

        static readonly string[] ExistingFriendFromServer = { "Jeff", "Mike", "Kaifei", "Fred" };
        static readonly string[] ExistingSpaceFromServer = { "\\msra\\floor\\12\\12434", "\\msra\\floor\\12\\focus_room", "\\msra\\floor\\12\\pantry" };
        static readonly string[] ExistingDeviceFromServer = { "\\msra\\floor\\12\\xbox_360", "\\msra\\floor\\12\\fridge", "\\msra\\floor\\12\\television" };

        List<string> location = new List<string>();

        public AddNewDevice()
        {
            InitializeComponent();
            TypeOnAdd.DataContext = types;
            CategoryOnAdd.DataContext = category;

            location.Add("\\msra\\floor\\12\\focus_room");
            location.Add("\\msra");
            location.Add("\\msra\\floor");
            location.Add("\\msra\\floor\\12");

            existingFriend.DataContext = ExistingFriendFromServer;

            //this.LocationOnAdd.ItemsSource = location;
        }

        void OnCategorySelected(object sender, EventArgs args)
        {
            if (CategoryOnAdd.SelectedItem.ToString() == "device")
            {
                LocationOnAddBlock.Visibility = Visibility.Visible;
                LocationOnAdd.Visibility = Visibility.Visible;
                TypeOnAdd.Visibility = Visibility.Visible;
            }
            else
            {
                LocationOnAddBlock.Visibility = Visibility.Collapsed;
                LocationOnAdd.Visibility = Visibility.Collapsed;
                TypeOnAdd.Visibility = Visibility.Collapsed;
            }
        }




        void OnClickDone(object sender, EventArgs args)
        {
            bool IsDoneAlready = true;
            if (DeviceNameOnAdd.Text == String.Empty)
            {
                MessageBox.Show("Please input the device name!");
                IsDoneAlready = false;
            }
            else if ((CategoryOnAdd.SelectedItem.ToString() == "device") && LocationOnAdd.Text == String.Empty)
            {
                MessageBox.Show("Please input the location of this device!");
                IsDoneAlready = false;
            }
            if (IsDoneAlready)
            {
                MessageBox.Show("You are registering a device:\ndevice name: " + DeviceNameOnAdd.Text + "\n"
                                                           + "Type: " + TypeOnAdd.SelectedItem.ToString() + "\n"
                                                           + "Location: " + LocationOnAdd.Text + "\n"
                                                           + "LivePulseID: " + LivePulseIDOnAdd.Text + "\n"
                                                           + "It will " + (ShowOnMapToggle.IsChecked == true ? "" : "not ") + "be shown on map" + "\n"
                                                           + "Description: " + DescriptionOnAdd.Text);
                // http post to sign up a device

                // using local storage to store something
                Device newDevice = new Device
                {
                    DeviceName = DeviceNameOnAdd.Text,
                    DeviceLocation = LocationOnAdd.Text,
                    DeviceDescription = DescriptionOnAdd.Text,
                    IsNotFavorite = (AddToFavorite.IsChecked == false)
                };
                newDevice.DeviceStateInDB = "on;off;unknown";
                newDevice.DeviceCommandInDB = "turn on;turn off;view energy";
                newDevice.DeviceEventInDB = "IsTurnedOn;IsTurnedOff";

                newDevice.Classification = CategoryOnAdd.SelectedItem.ToString().ToUpper();


                // Add the item to the ViewModel.
                App.ViewModel.AddToDoItem(newDevice);

                // Return to the main page.
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }

        private void OnClickCancel(object sender, EventArgs args)
        {
            this.NavigationService.GoBack();
        }


        private void EnterKeyUp(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                this.Focus();
            }
        }

        private void LocationOnAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LocationOnAdd.Focus();
        }

        private bool existingFriendVisible;
        private void existingFriendButtonClick(object sender, EventArgs args)
        {
            existingFriendVisible = !existingFriendVisible;
            if (existingFriendVisible)
            {
                existingFriend.Visibility = Visibility.Visible;
            }
            else
            {
                existingFriend.Visibility = Visibility.Collapsed;
            }
        }

    }
}