using System.Collections.Generic;
using System;

namespace LiveSynergy.Data
{
    public class AllDevice
    {

        public static List<Device> PublicDevice;
        static readonly Random _rand = new Random(100);

        public static void LoadDevice()
        {
            PublicDevice = new List<Device>
            {
                new Device(){ DeviceName="Television", DeviceLocation="\\msra\\floor\\12\\focus_room"},
                new Device(){ DeviceName="Light", DeviceLocation="\\msra\\floor\\12\\focus_room"},
                new Device(){ DeviceName="Light2", DeviceLocation="\\msra\\floor\\12\\focus_room"},
                new Device(){ DeviceName="Light3", DeviceLocation="\\msra\\floor\\12\\focus_room"},
                new Device(){ DeviceName="Computer", DeviceLocation="\\msra\\floor\\12\\fred_cubical"},
                new Device(){ DeviceName="Xbox 360", DeviceLocation="\\msra\\floor\\12\\lobby"},
                new Device(){ DeviceName="Oscilloscope", DeviceLocation="\\msra\\floor\\12\\fred_cubical"}
            };

            foreach (Device device in PublicDevice)
            {
                device.DeviceStateInDB = "on;off";

                device.DeviceCommandInDB = "turn on;turn off; view energy";

                device.DeviceCommandInDB = "IsTurnedOn;IsTurnedOff";

                device.EnergyConsumption = new List<int>();                
                for (int i = 0; i < 24 * 60; i++ )
                {
                    device.EnergyConsumption.Add(_rand.Next(0, 500));
                }
            }
        }
    }

}
