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
using Android.App;
using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Java.Util;

namespace BluetoothBear
{
    public class BleDeviceAndroid : BleDevice
    {

        BluetoothDevice dev;
        BluetoothGatt gatt;
        BleCallback bleCallback = null;
        //List<BluetoothGattService> services = null;

        public override void DiscoverServices()
        {
            gatt.DiscoverServices();

        }

        public List<BluetoothGattService> Services
        {
            get { return gatt.Services.ToList(); }
        }

        private string mTag = MethodBase.GetCurrentMethod().DeclaringType.ToString();


        public BleDeviceAndroid(BluetoothDevice d)
        {
            dev = d;
            bleCallback = new BleCallback(this);
        }

        public override string Name
        {
            get
            {
                return dev.Name;
            }
            protected set { throw new NotImplementedException(); }
        }

        public override string ID
        {
            get
            {
                return dev.Address;
            }
            protected set { throw new NotImplementedException(); }
        }



        private bool isServiceDiscovered = false;

        public override bool IsServiceDiscovered
        {
            get
            {
                return isServiceDiscovered;
            }
            protected set
            {
                isServiceDiscovered = value;
            }
        }

        public override BleConnectionState ConnectionState
        {
            get;
            protected set;

        }


        public override void Connect()
        {
            if (ConnectionState == BleConnectionState.Disconnected)
            {
                // ConnectionState = BleConnectionState.Connecting;
                if (gatt != null)
                {
                    gatt.Disconnect();
                    gatt.Close();
                }
                gatt = dev.ConnectGatt(Application.Context, true, bleCallback);


            }
        }





        public override void Disconnect()
        {
            gatt.Disconnect();
        }

        public override object GetCharacteristic(Guid id)
        {
            foreach (BluetoothGattService service in Services)
            {
                UUID uuid = UUID.FromString(id.ToString());
                BluetoothGattCharacteristic characteristic = service.GetCharacteristic(uuid);

                if (characteristic != null)
                {
                    return characteristic;
                }
            }
            return null;
        }

        public override object GetCharacteristic(ushort id)
        {

            foreach (BluetoothGattService service in Services)
            {
                //   BluetoothGattCharacteristic characteristic = service.Characteristics()
                // if (characteristic != null)
                //  {
                //      return characteristic;
                //  }
            }
            return null;
            //   throw new NotImplementedException();
        }




        private UUID CLIENT_CHARACTERISTIC_CONFIG_UUID = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
        public override bool EnableNotifications(object characteristic, bool enabled)
        {
            bool result = gatt.SetCharacteristicNotification((BluetoothGattCharacteristic)characteristic, enabled);
            if (result == true)
            {
                BluetoothGattDescriptor descriptor = ((BluetoothGattCharacteristic)characteristic).GetDescriptor(CLIENT_CHARACTERISTIC_CONFIG_UUID);
                if (enabled)
                {
                    result = descriptor.SetValue(BluetoothGattDescriptor.EnableNotificationValue.ToArray());
                }
                else
                {
                    result = descriptor.SetValue(BluetoothGattDescriptor.DisableNotificationValue.ToArray());
                }
                if (result == true)
                {
                    gatt.WriteDescriptor(descriptor);
                }
            }
            return result;


        }

        public override void ReadCharacteristic(object characteristic)
        {
            gatt.ReadCharacteristic((BluetoothGattCharacteristic)characteristic);
        }

        public override void WriteCharacteristic(object characteristic, byte[] value, bool isReliable)
        {
            gatt.WriteCharacteristic((BluetoothGattCharacteristic)characteristic);
        }

        public void SetConnectionState(BleConnectionState state)
        {
            this.ConnectionState = state;
        }



        private class BleCallback : BluetoothGattCallback
        {
            BleDevice dev;
            public BleCallback(BleDevice d)
            {
                dev = d;
            }

            public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
            {
                base.OnCharacteristicRead(gatt, characteristic, status);
                dev.OnCharacteristicRead(dev, new BleCharacteristicReadEventArgs(characteristic, status.ToBleGattOpererationState()));
            }

            public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
            {
                base.OnConnectionStateChange(gatt, status, newState);

                dev.OnConnectionStateChanged(dev, new BleConnectionStateChangedEventArgs(dev.ConnectionState, GetConnectionState(newState)));

            }

            private BleConnectionState GetConnectionState(ProfileState state)
            {
                switch (state)
                {
                    case ProfileState.Connected:
                        return BleConnectionState.Connected;
                    case ProfileState.Connecting:
                        return BleConnectionState.Connecting;
                    case ProfileState.Disconnected:
                        return BleConnectionState.Disconnected;
                    case ProfileState.Disconnecting:
                        return BleConnectionState.Disconnecting;
                    default:
                        return BleConnectionState.Disconnected;

                }
            }

            public override void OnDescriptorRead(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
            {
                base.OnDescriptorRead(gatt, descriptor, status);
            }

            public override void OnReadRemoteRssi(BluetoothGatt gatt, int rssi, GattStatus status)
            {
                base.OnReadRemoteRssi(gatt, rssi, status);
            }

            public override void OnDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
            {
                base.OnDescriptorWrite(gatt, descriptor, status);
            }

            public override void OnReliableWriteCompleted(BluetoothGatt gatt, GattStatus status)
            {
                base.OnReliableWriteCompleted(gatt, status);
            }

            public override void OnServicesDiscovered(BluetoothGatt gatt, GattStatus status)
            {
                base.OnServicesDiscovered(gatt, status);

                dev.OnConnectionStateChanged(dev, new BleConnectionStateChangedEventArgs(dev.ConnectionState, BleConnectionState.ConnectedWithServices));
                if (status == GattStatus.Success)
                {

                    dev.OnServicesDiscovered(dev, new BleServicesDiscoveredEventArgs(BleGattOperationState.Success));
                }
                else
                {

                    dev.OnServicesDiscovered(dev, new BleServicesDiscoveredEventArgs(BleGattOperationState.Failure));
                }
            }

            public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
            {
                base.OnCharacteristicChanged(gatt, characteristic);
                BleCharacteristicChangedEventArgs ev = new BleCharacteristicChangedEventArgs(characteristic, characteristic.GetValue(), BleGattOperationState.Success);
                dev.OnCharacteristicChanged(dev, ev);
            }

            public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
            {
                base.OnCharacteristicWrite(gatt, characteristic, status);
                dev.OnCharacteristicWrite(dev, new BleCharacteristicWriteEventArgs(characteristic, status.ToBleGattOpererationState()));
            }


        }




    }

    public static class ConversionHelpers
    {
        public static BleGattOperationState ToBleGattOpererationState(this GattStatus status)
        {
            switch (status)
            {
                case GattStatus.Failure:
                    return BleGattOperationState.Failure;
                case GattStatus.InsufficientAuthentication:
                    return BleGattOperationState.NotPermitted;
                case GattStatus.InsufficientEncryption:
                    return BleGattOperationState.NotPermitted;
                case GattStatus.InvalidAttributeLength:
                    return BleGattOperationState.NotPermitted;
                case GattStatus.InvalidOffset:
                    return BleGattOperationState.NotPermitted;
                case GattStatus.ReadNotPermitted:
                    return BleGattOperationState.NotPermitted;
                case GattStatus.RequestNotSupported:
                    return BleGattOperationState.RequestNotSupported;
                case GattStatus.Success:
                    return BleGattOperationState.Success;
                case GattStatus.WriteNotPermitted:
                    return BleGattOperationState.NotPermitted;
                default:
                    return BleGattOperationState.Failure;
            }

        }
    }
}
