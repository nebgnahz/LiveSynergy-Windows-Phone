﻿#pragma checksum "C:\Users\v-benzh\Documents\Visual Studio 2010\Projects\LiveSynergy\LiveSynergy\Pages\ViewDevicePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5658E940F89BF144AF16822A647C164D"
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
    
    
    public partial class ViewDevicePage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.Pivot devicePivot;
        
        internal System.Windows.Controls.TextBlock nameText;
        
        internal System.Windows.Controls.TextBlock LivePulseID;
        
        internal System.Windows.Controls.TextBlock LivePulseIDText;
        
        internal System.Windows.Controls.TextBlock location;
        
        internal System.Windows.Controls.TextBlock locationText;
        
        internal System.Windows.Controls.TextBlock descriptionText;
        
        internal System.Windows.Controls.ListBox stateList;
        
        internal System.Windows.Controls.ListBox commandList;
        
        internal System.Windows.Controls.ListBox eventList;
        
        internal System.Windows.Controls.ListBox childrenList;
        
        internal System.Windows.Controls.Canvas EnergyCanvas;
        
        internal System.Windows.Controls.Canvas xlabel;
        
        internal System.Windows.Controls.Canvas ylabel;
        
        internal System.Windows.Controls.RadioButton lasthour;
        
        internal System.Windows.Controls.RadioButton lastday;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/LiveSynergy;component/Pages/ViewDevicePage.xaml", System.UriKind.Relative));
            this.devicePivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("devicePivot")));
            this.nameText = ((System.Windows.Controls.TextBlock)(this.FindName("nameText")));
            this.LivePulseID = ((System.Windows.Controls.TextBlock)(this.FindName("LivePulseID")));
            this.LivePulseIDText = ((System.Windows.Controls.TextBlock)(this.FindName("LivePulseIDText")));
            this.location = ((System.Windows.Controls.TextBlock)(this.FindName("location")));
            this.locationText = ((System.Windows.Controls.TextBlock)(this.FindName("locationText")));
            this.descriptionText = ((System.Windows.Controls.TextBlock)(this.FindName("descriptionText")));
            this.stateList = ((System.Windows.Controls.ListBox)(this.FindName("stateList")));
            this.commandList = ((System.Windows.Controls.ListBox)(this.FindName("commandList")));
            this.eventList = ((System.Windows.Controls.ListBox)(this.FindName("eventList")));
            this.childrenList = ((System.Windows.Controls.ListBox)(this.FindName("childrenList")));
            this.EnergyCanvas = ((System.Windows.Controls.Canvas)(this.FindName("EnergyCanvas")));
            this.xlabel = ((System.Windows.Controls.Canvas)(this.FindName("xlabel")));
            this.ylabel = ((System.Windows.Controls.Canvas)(this.FindName("ylabel")));
            this.lasthour = ((System.Windows.Controls.RadioButton)(this.FindName("lasthour")));
            this.lastday = ((System.Windows.Controls.RadioButton)(this.FindName("lastday")));
        }
    }
}

