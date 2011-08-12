using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LiveSynergy.Data
{
    public class Device : System.ComponentModel.INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _deviceName;     
        private string _deviceLiveID;
        private string _deviceDescription;
        private string _deviceLocation;
        // there might be multiple states/commands/events/children, so use the observablecollection
        private ObservableCollection<string> _deviceState = new ObservableCollection<string>();
        private ObservableCollection<string> _deviceCommand = new ObservableCollection<string>();
        private ObservableCollection<string> _deviceEvent = new ObservableCollection<string>();
        private ObservableCollection<string> _deviceChildren = new ObservableCollection<string>();
        
        public BitmapImage DevicePic { get; set; }

        public string DeviceNameFirstLetter
        {
            get
            {
                return _deviceName.Substring(0, 1);
            }
        }
        public string DeviceName
        {
            set
            {
                if (_deviceName != value)
                {
                    _deviceName = value;
                    OnPropertyChanged("DeviceName");
                }
            }
            get
            {
                return _deviceName;
            }
        }
        public string DeviceLiveID
        {
            set
            {
                if (_deviceLiveID != value)
                {
                    _deviceLiveID = value;
                    OnPropertyChanged("DeviceLiveID");
                }
            }
            get
            {
                return _deviceLiveID;
            }

        }
        public string DeviceDescription
        {
            set
            {
                if (_deviceDescription != value)
                {
                    _deviceDescription = value;
                    OnPropertyChanged("DeviceDescription");
                }
            }
            get
            {
                return _deviceDescription;
            }
        }
        public string DeviceLocation
        {
            set
            {
                if (_deviceLocation != value)
                {
                    _deviceLocation = value;
                    OnPropertyChanged("DeviceLocation");
                }
            }
            get
            {
                return _deviceLocation;
            }
        }
        public ObservableCollection<string> DeviceState
        {
            set
            {
                if (_deviceState != value)
                {
                    _deviceState = value;
                    OnPropertyChanged("DeviceState");
                }
            }
            get
            {
                return _deviceState;
            }
        }
        public ObservableCollection<string> DeviceCommand
        {
            set
            {
                if (_deviceCommand != value)
                {
                    _deviceCommand = value;
                    OnPropertyChanged("DeviceCommand");
                }
            }
            get
            {
                return _deviceCommand;
            }
        }
        public ObservableCollection<string> DeviceEvent
        {
            set
            {
                if (_deviceEvent != value)
                {
                    _deviceEvent = value;
                    OnPropertyChanged("DeviceEvent");
                }
            }
            get
            {
                return _deviceEvent;
            }
        }
        public ObservableCollection<string> DeviceChildren
        {
            set
            {
                if (_deviceChildren != value)
                {
                    _deviceChildren = value;
                    OnPropertyChanged("DeviceChildren");
                }
            }
            get
            {
                return _deviceChildren;
            }
        }
        public List<int> EnergyConsumption{ get; set; }       // a list of points of
        
        public int EnergyReportInterval { get; set; }

        protected virtual void OnPropertyChanged(string propChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propChanged));
        }

    }

}
