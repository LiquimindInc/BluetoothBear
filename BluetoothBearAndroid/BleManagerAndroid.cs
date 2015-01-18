//     The MIT License (MIT)
//     
//     Copyright (c) 2015 Liquimind Inc
//     
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//     
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Reflection;
using Android.Bluetooth;
using Android.Util;


[assembly: Xamarin.Forms.Dependency(typeof(BluetoothBear.BleManagerAndroid))]
namespace BluetoothBear
{
    public class BleManagerAndroid : Service, IBleManager, BluetoothAdapter.ILeScanCallback
    {

        public event BleDeviceFoundEventHandler BleDeviceFound;



        private string mTag = MethodBase.GetCurrentMethod().DeclaringType.ToString();

        private BluetoothManager bluetoothManager;
        private BluetoothAdapter bluetoothAdapter;

        private List<BleDevice> devicesound = new List<BleDevice>();
        private BleBinder bleBinder = new BleBinder();
        public override IBinder OnBind(Intent intent)
        {
            return bleBinder;
        }


        private static DateTime currentDateTime; 

        public static void Initialize()
        {
            currentDateTime = DateTime.Now;
        }

        public BleManagerAndroid()
        {
            if (bluetoothManager == null)
            {
                bluetoothManager = (BluetoothManager)Application.Context.GetSystemService(BluetoothService);
                if (bluetoothManager == null)
                {
                    Log.Error(mTag, "Unable to initialize BluetoothManager.");
                    throw new Exception("Unable to get bluetooth adapter");
                }
                bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            }
        }

        public bool IsBleSupported()
        {
            return PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureBluetoothLe);
        }

        public bool IsBleEnabled()
        {
            return bluetoothAdapter.IsEnabled;
        }

        public void TrySetBleEnabled(bool enabled)
        {
            if (bluetoothAdapter.IsEnabled != enabled)
            {
                if (enabled == false)
                {
                    bluetoothAdapter.Disable();
                }
                else
                {
                    bluetoothAdapter.Enable();
                }

            }
        }

        public void StartBleScan()
        {
            bluetoothAdapter.StartLeScan(this);
            devicesound.Clear();
        }

        public void StopBleScan()
        {
            bluetoothAdapter.StopLeScan(this);
            devicesound.Clear();
        }

        public void OnLeScan(BluetoothDevice device, int rssi, byte[] scanRecord)
        {
            if (BleDeviceFound != null)
            {

                OnDeviceFound(new BleDeviceAndroid(device));
            }

        }

        public void OnDeviceFound(BleDevice dev)
        {
            if (BleDeviceFound != null && !devicesound.Contains(dev))
            {
                devicesound.Add(dev);
                BleDeviceFound(this, dev);
            }



        }
    }

    public class BleBinder : Android.OS.Binder
    {

        public BleManagerAndroid BleManager { get { return new BleManagerAndroid(); } }
    }
}