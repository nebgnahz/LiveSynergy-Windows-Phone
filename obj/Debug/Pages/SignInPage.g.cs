﻿#pragma checksum "c:\users\v-benzh\documents\visual studio 2010\Projects\LiveSynergy\LiveSynergy\Pages\SignInPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1D85F8402C6F9A5A898E185A09583DE0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace LiveSynergy.Pages {
    
    
    public partial class SignInPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock SignUpAppTitle;
        
        internal System.Windows.Controls.TextBlock SignUpPageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox NameOnSignIn;
        
        internal System.Windows.Controls.PasswordBox PasswordOnSingIn;
        
        internal Microsoft.Phone.Controls.ToggleSwitch IsRememberPW;
        
        internal Microsoft.Phone.Controls.ToggleSwitch IsAutoSignIn;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/LiveSynergy;component/Pages/SignInPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.SignUpAppTitle = ((System.Windows.Controls.TextBlock)(this.FindName("SignUpAppTitle")));
            this.SignUpPageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("SignUpPageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.NameOnSignIn = ((System.Windows.Controls.TextBox)(this.FindName("NameOnSignIn")));
            this.PasswordOnSingIn = ((System.Windows.Controls.PasswordBox)(this.FindName("PasswordOnSingIn")));
            this.IsRememberPW = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("IsRememberPW")));
            this.IsAutoSignIn = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("IsAutoSignIn")));
        }
    }
}
