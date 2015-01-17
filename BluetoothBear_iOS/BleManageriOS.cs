using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CoreBluetooth;
using Foundation;





[assembly: Xamarin.Forms.Dependency(typeof(BluetoothBear.BleManageriOS))]
namespace BluetoothBear
{
    public class BleManageriOS : CBCentralManagerDelegate, IBleManager
    {

        private static DateTime currentDateTime; 

        public static void Initialize()
        {
            currentDateTime = DateTime.Now;
        }

        public event BleDeviceFoundEventHandler BleDeviceFound;

        private void OnBleDeviceFound(BleDevice device)
        {
            if (BleDeviceFound != null)
                BleDeviceFound(this, (BleDevice)device);
        }

        public void TrySetBleEnabled(bool isEnabled)
        {
            throw new NotSupportedException("iOS does not support manually setting the bluetooth adpater.");
        }

        public CBCentralManager CentralManager { get; private set; }

        private List<BleDevice> DiscoveredDevices;

        public BleManageriOS()
        {
            CentralManager = new CBCentralManager();
            CentralManager.Delegate = this;
            DiscoveredDevices = new List<BleDevice>();
        }

        public bool IsBleSupported()
        {
            if (CentralManager.State == CBCentralManagerState.Unsupported)
                return false;

            return true;
        }

        public bool IsBleEnabled()
        {
            if (CentralManager.State == CBCentralManagerState.PoweredOn)
                return true;

            return false;
        }

        /// <summary>
        /// If ble state is the deseired state nothing happens. iOS does not support this feature. 
        /// </summary>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        public void SetBleEnabled(bool enabled)
        {
            if (IsBleEnabled() && enabled || !IsBleEnabled() && !enabled)
            {
                return;
            }
            else
            {
                throw (new NotImplementedException("iOS Does not support enabling and disabling ble from the app"));
            }
        }

        public void StartBleScan()
        {

            DiscoveredDevices.Clear();
            CentralManager.ScanForPeripherals(new CBUUID[] { });
          
        }

        public void StopBleScan()
        {
            CentralManager.StopScan();
        }

        public int getDeviceCount()
        {
            return DiscoveredDevices.Count;
        }

        public BleDevice getDevice(int index)
        {
            if (index < 0 || index >= getDeviceCount())
                return null;

            return DiscoveredDevices[index];
        }

        private bool DeviceExistsInDiscoveredList(CBPeripheral peripheral)
        {
            foreach (BleDevice device in DiscoveredDevices)
            {
                if (peripheral.Identifier.ToString() == device.ID)
                    return true;
            }
            return false;
        }


        public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            foreach (BleDeviceiOS device in DiscoveredDevices)
            {
                if (device.ID == peripheral.Identifier.ToString())
                {

                    device.OnConnected();
                    return;
                }
            }
        }

        public override void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
        {
            Debug.WriteLine("DisconnectedPeripheral: " + peripheral.Name);
            if (error != null)
                Debug.WriteLine("Error:: " + error.ToString());

            foreach (BleDeviceiOS device in DiscoveredDevices)
            {
                if (device.ID == peripheral.Identifier.ToString())
                {
                    device.OnDisconnected();
                    return;
                }
            }
        }

        public override void FailedToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
        {
            Debug.WriteLine("FailedToConnectPeripheral: " + peripheral.Name);
            if (error != null)
                Debug.WriteLine("Error:: " + error.ToString());
        }

        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
        {
            Debug.WriteLine("DiscoveredPeripheral: " + peripheral.Name);

            if (DeviceExistsInDiscoveredList(peripheral))
                return;

            BleDeviceiOS device = new BleDeviceiOS(this, peripheral, advertisementData, RSSI.Int32Value);

            DiscoveredDevices.Add(device);

            OnBleDeviceFound(device);
        }

        public override void RetrievedConnectedPeripherals(CBCentralManager central, CBPeripheral[] peripherals)
        {
            foreach (CBPeripheral peripheral in peripherals)
                Debug.WriteLine("RetrievedConnectedPeripherals: " + peripheral.Name);
        }

        public override void RetrievedPeripherals(CBCentralManager central, CBPeripheral[] peripherals)
        {
            foreach (CBPeripheral peripheral in peripherals)
                Debug.WriteLine("RetrievedPeripherals: " + peripheral.Name);
        }

        public override void UpdatedState(CBCentralManager central)
        {
            if (CentralManager.State == CBCentralManagerState.PoweredOff)
                Debug.WriteLine("CoreBluetooth BLE hardware is powered off");
            else if (CentralManager.State == CBCentralManagerState.PoweredOn)
                Debug.WriteLine("CoreBluetooth BLE hardware is powered on and ready");
            else if (CentralManager.State == CBCentralManagerState.Unauthorized)
                Debug.WriteLine("CoreBluetooth BLE state is unauthorized");
            else if (CentralManager.State == CBCentralManagerState.Unknown)
                Debug.WriteLine("CoreBluetooth BLE state is unknown");
            else if (CentralManager.State == CBCentralManagerState.Unsupported)
                Debug.WriteLine("CoreBluetooth BLE hardware is unsupported on this platform");
        }
    }
}
