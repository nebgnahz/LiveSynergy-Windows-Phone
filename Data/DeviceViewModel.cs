using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace LiveSynergy.Data
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private DeviceDataContext deviceDB;

        // Class constructor, create the data context object.
        public DeviceViewModel(string deviceDBConnectionString)
        {
            deviceDB = new DeviceDataContext(deviceDBConnectionString);
        }

        // All to-do items.
        private ObservableCollection<Device> _alltheDevice;
        public ObservableCollection<Device> AllTheDevice
        {
            get { return _alltheDevice; }
            set
            {
                _alltheDevice = value;
                NotifyPropertyChanged("AllTheDevice");
            }
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {
            // Specify the query for all to-do items in the database.
            var deviceInDB = from Device device in deviceDB.Items
                                select device;

            // Query the database and load all to-do items.
            AllTheDevice = new ObservableCollection<Device>(deviceInDB);
        }

        // Add a to-do item to the database and collections.
        public void AddToDoItem(Device newDevice)
        {
            // Add a to-do item to the data context.
            deviceDB.Items.InsertOnSubmit(newDevice);

            // Save changes to the database.
            deviceDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            AllTheDevice.Add(newDevice);
        }

        // Remove a to-do task item from the database and collections.
        public void DeleteToDoItem(Device DeviceForDelete)
        {
            // Remove the to-do item from the "all" observable collection.
            AllTheDevice.Remove(DeviceForDelete);

            // Remove the to-do item from the data context.
            deviceDB.Items.DeleteOnSubmit(DeviceForDelete);

            // Save changes to the database.
            deviceDB.SubmitChanges();
        }



        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            deviceDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
