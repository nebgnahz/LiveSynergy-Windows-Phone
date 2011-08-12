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
    public partial class SignUpPage : PhoneApplicationPage
    {
    
        static readonly string[] MSRAgroups = {"\\MSRA\\MASS", "\\MSRA\\WebSM", "\\MSRA\\University", "\\MSRA\\Graphics", "\\MSRA\\Innovation", "\\MSRA\\Hardware", "\\MSRA\\Search"};
        
        public SignUpPage()
        {
            InitializeComponent();
            DataContext = MSRAgroups;
        }



        private void EmailLostFocus(object sender, EventArgs args)
        {
            if (EmailOnSignUp.Text != String.Empty && EmailOnSignUp.Text.IndexOf('@') == -1)
            {
                MessageBox.Show("Do not forget to include the '@'");
            }
        }

        private void OnClickSignUP(object sender, EventArgs args)
        {
            bool IsDoneAlready = true;
            if (NameOnSignUp.Text == String.Empty)
            {
                MessageBox.Show("Please input your user name!");
                IsDoneAlready = false;
            }
            else if (EmailOnSignUp.Text == String.Empty)
            {
                MessageBox.Show("Please input your e-mail address!");
                IsDoneAlready = false;
            }
            else if (EmailOnSignUp.Text.IndexOf('@') == -1)
            {
                MessageBox.Show("You still forget to include the '@'");
                IsDoneAlready = false;
            }
            else if (Password1OnSingUp.Password == String.Empty)
            {
                MessageBox.Show("Please input your password!");
                IsDoneAlready = false;
            }
            else if (Password2OnSingUp.Password == String.Empty)
            {
                MessageBox.Show("Please input your password again!");
                IsDoneAlready = false;
            }
            else if (Password1OnSingUp.Password != Password2OnSingUp.Password)
            {
                MessageBox.Show("Passwords do not match!");
                IsDoneAlready = false;
            }
            if (IsDoneAlready)
            {
                // http post to sign up
                MessageBox.Show("You are creating an account: \nname: " + NameOnSignUp.Text + "\n"
                                                           + "Group@MSRA is " + MSRAGroupOnSignUp.SelectedItem.ToString() + "\n"
                                                           + (LiveLinkIDOnSignUp.Text == String.Empty ? "" : "LiveLinkID: ") + LiveLinkIDOnSignUp.Text + "\n"
                                                           + "Email: " + EmailOnSignUp.Text + "\n");
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