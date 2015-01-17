using System;
using CoreBluetooth;
using Foundation;

namespace BluetoothBear
{


    public class BleDeviceiOS : BleDevice
    {
        public CBPeripheral Peripheral { get; private set; }

        private BleManageriOS Owner;

        public override void DiscoverServices()
        {
            if (Peripheral == null)
                return;

            Peripheral.DiscoverServices();
        }

        public BleDeviceiOS(BleManageriOS owner, CBPeripheral peripheral, NSDictionary advertisement, int rssi)
        {
            Owner = owner;


            Peripheral = peripheral;

            Name = Peripheral.Name;
            ID = Peripheral.Identifier.ToString();

            //Todo: check this won't cause memory leak when object get delete.
            Peripheral.DiscoveredService += HandleDiscoveredService;
            Peripheral.RssiUpdated += HandleRssiUpdated;
            Peripheral.UpdatedCharacterteristicValue += HandleUpdatedCharacterteristicValue;
            Peripheral.WroteCharacteristicValue += HandleWroteCharacteristicValue;
            Peripheral.UpdatedValue += HandleUpdatedValue;
            Peripheral.WroteDescriptorValue += HandleWroteDescriptorValue;


        }

        void HandleWroteDescriptorValue(object sender, CBDescriptorEventArgs e)
        {
            CBPeripheral peripheral = sender as CBPeripheral;
            if (peripheral == null || peripheral != Peripheral)
                return;

            if (e.Error == null)

                OnDescriptorWrite(this, new BleDescriptorWriteEventArgs(e.Descriptor, BleGattOperationState.Success));
            else
                OnDescriptorWrite(this, new BleDescriptorWriteEventArgs(e.Descriptor, BleGattOperationState.Failure));
        }

        void HandleUpdatedValue(object sender, CBDescriptorEventArgs e)
        {
            CBPeripheral peripheral = sender as CBPeripheral;
            if (peripheral == null || peripheral != Peripheral)
                return;

            if (e.Error == null)
                OnDescriptorRead(this, new BleDescriptorReadEventArgs(e.Descriptor, BleGattOperationState.Success));
            else
                OnDescriptorRead(this, new BleDescriptorReadEventArgs(e.Descriptor, BleGattOperationState.Failure));
        }

        void HandleWroteCharacteristicValue(object sender, CBCharacteristicEventArgs e)
        {
            CBPeripheral peripheral = sender as CBPeripheral;
            if (peripheral == null || peripheral != Peripheral)
                return;

            if (e.Error == null)
                OnCharacteristicWrite(this, new BleCharacteristicWriteEventArgs(e.Characteristic, BleGattOperationState.Success));
            else
                OnCharacteristicWrite(this, new BleCharacteristicWriteEventArgs(e.Characteristic, BleGattOperationState.Failure));
        }

        void HandleUpdatedCharacterteristicValue(object sender, CBCharacteristicEventArgs e)
        {
            CBPeripheral peripheral = sender as CBPeripheral;
            if (peripheral == null || peripheral != Peripheral)
                return;

            if (e.Error == null)
                OnCharacteristicChanged(this, new BleCharacteristicChangedEventArgs(e.Characteristic, e.Characteristic.Value.ToArray(),  BleGattOperationState.Success));
            else
                OnCharacteristicChanged(this, new BleCharacteristicChangedEventArgs(e.Characteristic, e.Characteristic.Value.ToArray() ,BleGattOperationState.Failure));
        }

        private void HandleRssiUpdated(object sender, NSErrorEventArgs e)
        {
            CBPeripheral peripheral = sender as CBPeripheral;
            if (peripheral == null || peripheral != Peripheral)
                return;

            if (e.Error == null)
                OnReadRemoteRssi(this, new BleReadRemoteRssiEventArgs(Peripheral.RSSI.Int32Value, BleGattOperationState.Success));
            else
                OnReadRemoteRssi(this, new BleReadRemoteRssiEventArgs(Peripheral.RSSI.Int32Value, BleGattOperationState.Failure));
        }

        private void HandleDiscoveredService(object sender, NSErrorEventArgs e)
        {
            CBPeripheral peripheral = sender as CBPeripheral;
            if (peripheral == null || peripheral != Peripheral)
                return;

            int servicesCount = peripheral.Services.Length;
            peripheral.DiscoveredCharacteristic += (o, es) =>
            {
                servicesCount--;
                if (servicesCount == 0)
                {
                    IsServiceDiscovered = true;
                    if (e.Error == null)
                    {
                        OnServicesDiscovered(this, new BleServicesDiscoveredEventArgs(BleGattOperationState.Success));
                        ConnectionState = BleConnectionState.ConnectedWithServices;
                    }
                    else
                    {
                        OnServicesDiscovered(this, new BleServicesDiscoveredEventArgs(BleGattOperationState.Failure));
                    }
                }
            };
            foreach (CBService service in peripheral.Services)
            {
                peripheral.DiscoverCharacteristics(service);
            }

        }



        public override string Name
        {
            get;
            protected set;
        }

        public override string ID
        {
            get;
            protected set;
        }

        private BleConnectionState connectionState;
        public override BleConnectionState ConnectionState
        {
            get { return connectionState; }
            protected set
            {
                if (value != connectionState)
                {
                    BleConnectionState oldState = connectionState;
                    connectionState = value;
                    OnConnectionStateChanged(this, new BleConnectionStateChangedEventArgs(oldState, value));
                }
            }
        }


        public override bool IsServiceDiscovered
        {
            get;
            protected set;
        }

        public override void Connect()
        {
            if (ConnectionState == BleConnectionState.Disconnected)
            {
                ConnectionState = BleConnectionState.Connecting;
                Owner.CentralManager.ConnectPeripheral(Peripheral);
            }
        }

        public override void Disconnect()
        {
            if (ConnectionState != BleConnectionState.Disconnected)
            {
                Owner.CentralManager.CancelPeripheralConnection(Peripheral);
                ConnectionState = BleConnectionState.Disconnected;

            }
        }

        public override object GetCharacteristic(Guid id)
        {
            byte[] guidData = id.ToByteArray();
            byte[] cbData;
            if (Peripheral.Services != null)
            {
                foreach (CBService service in Peripheral.Services)
                {
                    if (service.Characteristics != null)
                    {
                        foreach (CBCharacteristic characteristic in service.Characteristics)


                            if (characteristic.UUID.Data.Length == 2)
                            {
                                cbData = characteristic.UUID.Data.ToArray();
                                if (cbData[0] == guidData[1] && cbData[1] == guidData[0])
                                {
                                    return characteristic;
                                }
                            }
                            else if (characteristic.UUID.ToString().Equals(id.ToString()))
                            {
                                return characteristic;
                            }
                    }
                }
            }

            return null;
        }

        public override object GetCharacteristic(UInt16 id)
        {
            if (Peripheral.Services != null)
            {
                foreach (CBService service in Peripheral.Services)
                {
                    if (service.Characteristics != null)
                    {
                        foreach (CBCharacteristic characteristic in service.Characteristics)
                        {
                            if (characteristic.UUID.ToString().Equals(id.ToString("X4")))
                                return characteristic;
                        }
                    }
                }
            }

            return null;
        }

        public override bool EnableNotifications(object characteristic, bool enabled)
        {
            bool result = false;

            if (Peripheral.Services != null)
            {
                foreach (CBService service in Peripheral.Services)
                {
                    if (service.Characteristics != null)
                    {
                        foreach (CBCharacteristic c in service.Characteristics)
                        {
                            if (c == characteristic)
                            {
                                Peripheral.SetNotifyValue(enabled, c);
                                result = true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public override void ReadCharacteristic(object characteristic)
        {
            CBCharacteristic c = characteristic as CBCharacteristic;
            if (c == null)
                return;

            Peripheral.ReadValue(c);
        }

        public override void WriteCharacteristic(object characteristic, byte[] value, bool isReliable)
        {
            CBCharacteristic c = characteristic as CBCharacteristic;
            if (c == null)
                return;

            if (isReliable)
            {
                Peripheral.WriteValue(NSData.FromArray(value), c, CBCharacteristicWriteType.WithResponse);
            }
            else
            {
                Peripheral.WriteValue(NSData.FromArray(value), c, CBCharacteristicWriteType.WithoutResponse);
            }
        }

        public void OnConnected()
        {
            ConnectionState = BleConnectionState.Connected;
        }

        public void OnDisconnected()
        {
            ConnectionState = BleConnectionState.Disconnected;
        }

    }
}

