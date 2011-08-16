using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using Microsoft.Phone.Data.Linq.Mapping;

namespace LiveSynergy.Data
{

    public class DeviceDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public DeviceDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the to-do items.
        public Table<Device> Items;
    }


    [Index(Columns = "Classification", IsUnique = false, Name = "classification")]
    [Table]
    public class Device : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _db_ID;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int DB_ID
        {
            get
            {
                return _db_ID;
            }
            set
            {
                if (_db_ID != value)
                {
                    NotifyPropertyChanging("DB_ID");
                    _db_ID = value;
                    NotifyPropertyChanged("DB_ID");
                }
            }
        }


        private string _deviceName;     
        private string _deviceLiveID;
        private string _deviceDescription;
        private string _deviceLocation;
        // there exists the conversion from string to ObservableCollection
        private string _deviceState;
        private string _deviceCommand;
        private string _deviceEvent;
        private string _deviceChildren;
        
        public BitmapImage DevicePic { get; set; }

        
        public string DeviceNameFirstLetter
        {
            get
            {
                return _deviceName.Substring(0, 1);
            }
        }
        
        [Column]
        public string DeviceName
        {
            set
            {
                if (_deviceName != value)
                {
                    NotifyPropertyChanging("DeviceName");
                    _deviceName = value;
                    NotifyPropertyChanged("DeviceName");
                }
            }
            get
            {
                return _deviceName;
            }
        }
        [Column]
        public string DeviceLiveID
        {
            set
            {
                if (_deviceLiveID != value)
                {
                    NotifyPropertyChanging("DeviceLiveID");
                    _deviceLiveID = value;
                    NotifyPropertyChanged("DeviceLiveID");
                }
            }
            get
            {
                return _deviceLiveID;
            }

        }
        [Column]
        public string DeviceDescription
        {
            set
            {
                if (_deviceDescription != value)
                {
                    NotifyPropertyChanging("DeviceDescription");
                    _deviceDescription = value;
                    NotifyPropertyChanged("DeviceDescription");
                }
            }
            get
            {
                return _deviceDescription;
            }
        }
        [Column]
        public string DeviceLocation
        {
            set
            {
                if (_deviceLocation != value)
                {
                    NotifyPropertyChanging("DeviceLocation");
                    _deviceLocation = value;
                    NotifyPropertyChanged("DeviceLocation");
                }
            }
            get
            {
                return _deviceLocation;
            }
        }

        [Column]
        public string DeviceStateInDB
        {
            set
            {
                if (_deviceState != value)
                {
                    NotifyPropertyChanging("DeviceState");
                    _deviceState = value;
                    NotifyPropertyChanged("DeviceState");
                }
            }
            get
            {
                return _deviceState;
            }
        }
        public ObservableCollection<string> DeviceState
        {
            get
            {
                ObservableCollection<string> _deviceStateTmp = new ObservableCollection<string>();
                if (String.IsNullOrEmpty(_deviceState))
                {
                    return _deviceStateTmp;
                }
                string[] split = _deviceState.Split(';');
                foreach (string s in split)
                {
                    _deviceStateTmp.Add(s);
                }
                return _deviceStateTmp;
            }
        }


        [Column]
        public string DeviceCommandInDB
        {
            set
            {
                if (_deviceCommand != value)
                {
                    NotifyPropertyChanging("DeviceCommand");
                    _deviceCommand = value;
                    NotifyPropertyChanged("DeviceCommand");
                }
            }
            get
            {
                return _deviceCommand;
            }
        }
        public ObservableCollection<string> DeviceCommand
        {
            get
            {
                ObservableCollection<string> _deviceCommandTmp = new ObservableCollection<string>();
                if (String.IsNullOrEmpty(_deviceCommand))
                {
                    return _deviceCommandTmp;
                }
                string[] split = _deviceCommand.Split(';');
                foreach (string s in split)
                {
                    _deviceCommandTmp.Add(s);
                }
                return _deviceCommandTmp;
            }
        }


        [Column]
        public string DeviceEventInDB
        {
            set
            {
                if (_deviceEvent != value)
                {
                    NotifyPropertyChanging("DeviceEvent");
                    _deviceEvent = value;
                    NotifyPropertyChanged("DeviceEvent");
                }
            }
            get
            {
                return _deviceEvent;
            }
        }
        public ObservableCollection<string> DeviceEvent
        {
            get
            {
                ObservableCollection<string> _deviceEventTmp = new ObservableCollection<string>();
                if (String.IsNullOrEmpty(_deviceEvent))
                {
                    return _deviceEventTmp;
                }
                string[] split = _deviceEvent.Split(';');
                foreach (string s in split)
                {
                    _deviceEventTmp.Add(s);
                }
                return _deviceEventTmp;
            }
        }


        [Column]
        public string DeviceChildrenInDB
        {
            set
            {
                if (_deviceChildren != value)
                {
                    NotifyPropertyChanging("DeviceChildren");
                    _deviceChildren = value;
                    NotifyPropertyChanged("DeviceChildren");
                }
            }
            get
            {
                return _deviceChildren;
            }
        }
        public ObservableCollection<string> DeviceChildren
        {
            get
            {
                ObservableCollection<string> _deviceChildrenTmp = new ObservableCollection<string>();
                if (String.IsNullOrEmpty(_deviceChildren))
                {
                    return _deviceChildrenTmp;
                }
                string[] split = _deviceChildren.Split(';');
                foreach (string s in split)
                {
                    _deviceChildrenTmp.Add(s);
                }
                return _deviceChildrenTmp;
            }
        }

        public List<int> EnergyConsumption { get; set; }       // a list of points of

        [Column(CanBeNull=false)]
        public string Classification {set; get; }

        
        // Define completion value: private field, public property, and database column.
        private bool _isNotFavorite;
        [Column]
        public bool IsNotFavorite
        {
            get 
            {
                return _isNotFavorite; 
            }
            set
            {
                if (_isNotFavorite != value)
                {
                    NotifyPropertyChanging("IsNotFavorite");
                    _isNotFavorite = value;
                    NotifyPropertyChanged("IsNotFavorite");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region INotifyPropertyChanging Members
        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion
    }
}
