﻿#pragma checksum "C:\Users\v-benzh\Documents\Visual Studio 2010\Projects\LiveSynergy\LiveSynergy\Pages\DeviceListPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0642A56FF914A946CB989FFD7657C483"
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
using Microsoft.Phone.Shell;
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


namespace LiveSynergy {
    
    
    public partial class DeviceListPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.LongListSelector All;
        
        internal Microsoft.Phone.Controls.PivotItem space;
        
        internal System.Windows.Controls.ListBox SpaceList;
        
        internal Microsoft.Phone.Controls.PivotItem @private;
        
        internal System.Windows.Controls.ListBox PrivateList;
        
        internal Microsoft.Phone.Controls.PivotItem @public;
        
        internal System.Windows.Controls.ListBox PublicList;
        
        internal Microsoft.Phone.Controls.PivotItem friend;
        
        internal System.Windows.Controls.ListBox FriendList;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton newTaskAppBarButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/LiveSynergy;component/Pages/DeviceListPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.All = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("All")));
            this.space = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("space")));
            this.SpaceList = ((System.Windows.Controls.ListBox)(this.FindName("SpaceList")));
            this.@private = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("private")));
            this.PrivateList = ((System.Windows.Controls.ListBox)(this.FindName("PrivateList")));
            this.@public = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("public")));
            this.PublicList = ((System.Windows.Controls.ListBox)(this.FindName("PublicList")));
            this.friend = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("friend")));
            this.FriendList = ((System.Windows.Controls.ListBox)(this.FindName("FriendList")));
            this.newTaskAppBarButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("newTaskAppBarButton")));
        }
    }
}

