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
using System.Threading.Tasks;

namespace BluetoothBear
{
    /// <summary>
    /// A cross platform abstraction of the Bluetooth Low energy device allowing devices to be created in the pcl library
    /// </summary>
    public class XBleDevice : IBleDevice
    {
        BleDevice dev;

        /// <summary>
        /// Occurs when connection state changed.
        /// </summary>
        public event BleDeviceConnectionStateChangedEventHandler ConnectionStateChanged { add { dev.ConnectionStateChanged += value; } remove { dev.ConnectionStateChanged -= value; } }
        /// <summary>
        /// Occurs when characteristic changed.
        /// </summary>
        public event BleDeviceCharacteristicChangedEventHandler CharacteristicChanged { add { dev.CharacteristicChanged += value; } remove { dev.CharacteristicChanged -= value; } }
        /// <summary>
        /// Occurs when characteristic read.
        /// </summary>
        public event BleDeviceCharacteristicReadEventHandler CharacteristicRead { add { dev.CharacteristicRead += value; } remove { dev.CharacteristicRead -= value; } }
        /// <summary>
        /// Occurs when characteristic write.
        /// </summary>
        public event BleDeviceCharacteristicWriteEventHandler CharacteristicWrite { add { dev.CharacteristicWrite += value; } remove { dev.CharacteristicWrite -= value; } }
        /// <summary>
        /// Occurs when descriptor read.
        /// </summary>
        public event BleDeviceDescriptorReadEventHandler DescriptorRead { add { dev.DescriptorRead += value; } remove { dev.DescriptorRead -= value; } }
        /// <summary>
        /// Occurs when descriptor write.
        /// </summary>
        public event BleDeviceDescriptorWriteEventHandler DescriptorWrite { add { dev.DescriptorWrite += value; } remove { dev.DescriptorWrite -= value; } }

        /// <summary>
        /// Occurs when read remote rssi.
        /// </summary>
        public event BleDeviceReadRemoteRssiEventHandler ReadRemoteRssi { add { dev.ReadRemoteRssi += value; } remove { dev.ReadRemoteRssi -= value; } }

        /// <summary>
        /// Occurs when reliable write completed.
        /// </summary>
        public event BleDeviceReliableWriteCompletedEventHandler ReliableWriteCompleted { add { dev.ReliableWriteCompleted += value; } remove { dev.ReliableWriteCompleted -= value; } }

        /// <summary>
        /// Occurs when services discovered.
        /// </summary>
        public event BleDeviceServicesDiscoveredEventHandler ServicesDiscovered { add { dev.ServicesDiscovered += value; } remove { dev.ServicesDiscovered -= value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.XBleDevice"/> class.
        /// </summary>
        public XBleDevice(BleDevice device)
        {
            dev = device;

        }

        /// <summary>
        /// Connect to this device.
        /// </summary>
        public void Connect()
        {
            dev.Connect();
        }

        /// <summary>
        /// Gets the state of the connection.
        /// </summary>
        /// <value>The state of the connection.</value>
        virtual public BleConnectionState ConnectionState
        {
            get
            {
                return dev.ConnectionState;
            }

        }

        public void DiscoverServices()
        {
            dev.DiscoverServices();
        }
        /// <summary>
        /// Disconnect this device.
        /// </summary>
        public void Disconnect()
        {
            dev.Disconnect();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return dev.Name;
            }
        }

        /// <summary>
        /// Gets the ID. In Android and Windows this is a Mac Address on IOS this is a UUID
        /// </summary>
        /// <value>The UUI.</value>
        public string ID
        {
            get
            {
                return dev.ID;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has performed service discovered.
        /// </summary>
        /// <value>true</value>
        /// <c>false</c>
        public bool IsServiceDiscovered
        {
            get { return dev.IsServiceDiscovered; }
        }

        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <returns>The characteristic.</returns>
        /// <param name="id">Identifier.</param>
        public object GetCharacteristic(Guid id)
        {
            return dev.GetCharacteristic(id);
        }

        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <returns>The characteristic.</returns>
        /// <param name="id">Identifier.</param>
        public object GetCharacteristic(UInt16 id)
        {
            return dev.GetCharacteristic(id);
        }

        /// <summary>
        /// Enables the notifications on a characteristic.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
        public bool EnableNotifications(object characteristic, bool isEnabled)
        {
            return dev.EnableNotifications(characteristic, isEnabled);
        }

        /// <summary>
        /// Reads the characteristic.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        public void ReadCharacteristic(object characteristic)
        {
            dev.ReadCharacteristic(characteristic);
        }


        /// <summary>
        /// Writes the characteristic.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReliable">If set to <c>true</c> is reliable.</param>
        public void WriteCharacteristic(object characteristic, byte[] value, bool isReliable)
        {
            dev.WriteCharacteristic(characteristic, value, isReliable);
        }


        /// <summary>
        /// Raises the connection state changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnConnectionStateChanged(IBleDevice sender, BleConnectionStateChangedEventArgs e)
        {
            dev.OnConnectionStateChanged(sender, e);
        }

        /// <summary>
        /// Raises the characteristic read event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnCharacteristicRead(IBleDevice sender, BleCharacteristicReadEventArgs e)
        {
            dev.OnCharacteristicRead(sender, e);
        }

        /// <summary>
        /// Raises the characteristic write event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnCharacteristicWrite(IBleDevice sender, BleCharacteristicWriteEventArgs e)
        {
            dev.OnCharacteristicWrite(sender, e);
        }

        /// <summary>
        /// Raises the characteristic changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnCharacteristicChanged(IBleDevice sender, BleCharacteristicChangedEventArgs e)
        {
            dev.OnCharacteristicChanged(sender, e);
        }

        /// <summary>
        /// Raises the descriptor read event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnDescriptorRead(IBleDevice sender, BleDescriptorReadEventArgs e)
        {
            dev.OnDescriptorRead(sender, e);
        }

        /// <summary>
        /// Raises the descriptor write event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnDescriptorWrite(IBleDevice sender, BleDescriptorWriteEventArgs e)
        {
            dev.OnDescriptorWrite(sender, e);
        }

        /// <summary>
        /// Raises the read remote rssi event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnReadRemoteRssi(IBleDevice sender, BleReadRemoteRssiEventArgs e)
        {
            dev.OnReadRemoteRssi(sender, e);
        }

        /// <summary>
        /// Raises the reliable write completed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnReliableWriteCompleted(IBleDevice sender, BleReliableWriteCompletedEventArgs e)
        {
            dev.OnReliableWriteCompleted(sender, e);
        }

        /// <summary>
        /// Raises the services discovered event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnServicesDiscovered(IBleDevice sender, BleServicesDiscoveredEventArgs e)
        {
            dev.OnServicesDiscovered(sender, e);
        }


    }
}
