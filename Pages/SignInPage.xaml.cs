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
    public partial class SignInPage : PhoneApplicationPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void OnClickSignIn(object sender, EventArgs args)
        {
            bool IsDoneAlready = true;
            if (NameOnSignIn.Text == String.Empty)
            {
                MessageBox.Show("Please input your user name!");
                IsDoneAlready = false;
            }
            else if (PasswordOnSingIn.Password == String.Empty)
            {
                MessageBox.Show("Please input your password!");
                IsDoneAlready = false;
            }
            if (IsDoneAlready)
            {
                // http post to sign up
                MessageBox.Show("You are signing in: \nname: " + NameOnSignIn.Text + "\n");
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
    }
}