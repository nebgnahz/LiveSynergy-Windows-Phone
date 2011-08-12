using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace LiveSynergy.Pages
{
    public partial class SignInAndUp : PhoneApplicationPage
    {

        static readonly string[] MSRAgroups = { "\\MSRA\\MASS", "\\MSRA\\WebSM", "\\MSRA\\University", "\\MSRA\\Graphics", "\\MSRA\\Innovation", "\\MSRA\\Hardware", "\\MSRA\\Search" };
        
        public SignInAndUp()
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

        private void OnClickDone(object sender, EventArgs args)
        {
            bool IsDoneAlready = true;
            if (SignInOrUpPivot.SelectedItem == SignUpPivot)
            {
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
                else if (Password1OnSignUp.Password == String.Empty)
                {
                    MessageBox.Show("Please input your password!");
                    IsDoneAlready = false;
                }
                else if (Password2OnSignUp.Password == String.Empty)
                {
                    MessageBox.Show("Please input your password again!");
                    IsDoneAlready = false;
                }
                else if (Password1OnSignUp.Password != Password2OnSignUp.Password)
                {
                    MessageBox.Show("Passwords do not match!");
                    IsDoneAlready = false;
                }
                if (IsDoneAlready)
                {
                    ///////////////////////////////////////////////////////////////////////////////////////
                    // http post to sign up
                    ///////////////////////////////////////////////////////////////////////////////////////
                    MessageBox.Show("You are creating an account: \nname: " + NameOnSignUp.Text + "\n"
                                                               + "Group@MSRA is " + MSRAGroupOnSignUp.SelectedItem.ToString() + "\n"
                                                               + (LiveLinkIDOnSignUp.Text == String.Empty ? "" : "LiveLinkID: ") + LiveLinkIDOnSignUp.Text + "\n"
                                                               + "Email: " + EmailOnSignUp.Text + "\n" +
                                                               (IsRememberPWOnSignUp.IsChecked == true ? "":"not ") + "to remember your name and password" + "\n" +
                                                               (IsAutoSignInOnSignUp.IsChecked == true ? "":"not ") + "to login automatically" + "\n");
                    if (IsRememberPWOnSignUp.IsChecked == true)
                    {
                        IsolatedStorageSettings.ApplicationSettings["UserName"] = NameOnSignUp.Text;
                        IsolatedStorageSettings.ApplicationSettings["Password"] = Password1OnSignUp.Password;
                        IsolatedStorageSettings.ApplicationSettings["IsRememberPW"] = IsRememberPWOnSignUp.IsChecked.ToString();
                        IsolatedStorageSettings.ApplicationSettings["IsAutoSignIn"] = IsAutoSignInOnSignUp.IsChecked.ToString();
                    }
                }
            }
            else
            {
                if (NameOnSignIn.Text == String.Empty)
                {
                    MessageBox.Show("Please input your user name!");
                    IsDoneAlready = false;
                }
                else if (PasswordOnSignIn.Password == String.Empty)
                {
                    MessageBox.Show("Please input your password!");
                    IsDoneAlready = false;
                }
                if (IsDoneAlready)
                {
                    ///////////////////////////////////////////////////////////////////////////////////////
                    // http post to sign in
                    ///////////////////////////////////////////////////////////////////////////////////////
                    
                    MessageBox.Show("You are signing in: \nname: " + NameOnSignIn.Text + "\n" + 
                        (IsRememberPW.IsChecked == true ? "":"not ") + "to remember your name and password" + "\n" +
                        (IsAutoSignIn.IsChecked == true ? "":"not ") + "to login automatically" + "\n");

                    
                    if (IsRememberPWOnSignUp.IsChecked == true)
                    {
                        IsolatedStorageSettings.ApplicationSettings["UserName"] = NameOnSignUp.Text;
                        IsolatedStorageSettings.ApplicationSettings["Password"] = Password1OnSignUp.Password;
                        IsolatedStorageSettings.ApplicationSettings["IsRememberPW"] = IsRememberPWOnSignUp.IsChecked.ToString();
                        IsolatedStorageSettings.ApplicationSettings["IsAutoSignIn"] = IsAutoSignInOnSignUp.IsChecked.ToString();
                    }
                    else if (IsRememberPW.IsChecked == true)
                    {
                        IsolatedStorageSettings.ApplicationSettings["UserName"] = NameOnSignIn.Text;
                        IsolatedStorageSettings.ApplicationSettings["Password"] = PasswordOnSignIn.Password;
                        IsolatedStorageSettings.ApplicationSettings["IsRememberPW"] = IsRememberPW.IsChecked.ToString();
                        IsolatedStorageSettings.ApplicationSettings["IsAutoSignIn"] = IsAutoSignIn.IsChecked.ToString();
                    }
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


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object IsRemeberPWOnLoad;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("IsRememberPW", out IsRemeberPWOnLoad))
            {
                if ((string)IsRemeberPWOnLoad == "True")
                {
                    NameOnSignIn.Text = IsolatedStorageSettings.ApplicationSettings["UserName"].ToString();
                    PasswordOnSignIn.Password = IsolatedStorageSettings.ApplicationSettings["Password"].ToString();
                    IsRememberPW.IsChecked = Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings["IsRememberPW"].ToString());
                    IsAutoSignIn.IsChecked = Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings["IsAutoSignIn"].ToString());
                    IsRememberPWOnSignUp.IsChecked = Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings["IsRememberPW"].ToString());
                    IsAutoSignInOnSignUp.IsChecked = Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings["IsAutoSignIn"].ToString());
                }
                else
                {
                    IsRememberPW.IsChecked = false;
                    IsAutoSignIn.IsChecked = false;
                    IsRememberPWOnSignUp.IsChecked = false;
                    IsAutoSignInOnSignUp.IsChecked = false;
                }
            }
        }
        
    }
}