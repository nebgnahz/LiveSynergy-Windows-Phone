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

namespace LiveSynergy.Pages
{
    public partial class AddNewDevice : PhoneApplicationPage
    {


        static readonly string[] types = { "Light", "Computer", "Television" };
        List<string> location = new List<string>();

        public AddNewDevice()
        {
            InitializeComponent();
            DataContext = types;

            location.Add("\\msra\\floor\\12\\focus_room");
            location.Add("\\msra");
            location.Add("\\msra\\floor");
            location.Add("\\msra\\floor\\12");

            //this.LocationOnAdd.ItemsSource = location;
        }
        void OnClickDone(object sender, EventArgs args)
        {
            bool IsDoneAlready = true;
            if (DeviceNameOnAdd.Text == String.Empty)
            {
                MessageBox.Show("Please input the device name!");
                IsDoneAlready = false;
            }
            else if (LocationOnAdd.Text == String.Empty)
            {
                MessageBox.Show("Please input the location!");
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

    }
}