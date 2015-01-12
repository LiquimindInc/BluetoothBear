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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothBear
{
    /// <summary>
    /// Bluetooth LE Device base implementation
    /// </summary>
    public abstract class BleDevice : IBleDevice, IComparable, INotifyPropertyChanged
    {

        #region Equality
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="XBleApi.BleDevice"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="XBleApi.BleDevice"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="XBleApi.BleDevice"/>;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            IBleDevice otherDevice = obj as IBleDevice;
            if (otherDevice != null)
            {
                return otherDevice.ID == this.ID;
            }
            else return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="XBleApi.BleDevice"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="XBleApi.BleDevice"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="XBleApi.BleDevice"/>.</returns>
        public override string ToString()
        {
            return string.Format("[BleDevice: Name={0}, ID={1}, ConnectionState={2}, IsServiceDiscovered={3}]", Name, ID, ConnectionState, IsServiceDiscovered);

        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="obj">Object.</param>
        public int CompareTo(object obj)
        {
            IBleDevice otherDevice = obj as IBleDevice;
            if (otherDevice != null)
            {
                return otherDevice.ID.CompareTo(this.ID);
            }
            throw new NotSupportedException("Unable to compare: " + this.GetType() + " with " + obj.GetType());
        }
        #endregion


        /// <summary>
        /// Occurs when connection state changed.
        /// </summary>
        public event BleDeviceConnectionStateChangedEventHandler ConnectionStateChanged;

        /// <summary>
        /// Occurs when characteristic changed.
        /// </summary>
        public event BleDeviceCharacteristicChangedEventHandler CharacteristicChanged;

        /// <summary>
        /// Occurs when characteristic read.
        /// </summary>
        public event BleDeviceCharacteristicReadEventHandler CharacteristicRead;

        /// <summary>
        /// Occurs when characteristic write.
        /// </summary>
        public event BleDeviceCharacteristicWriteEventHandler CharacteristicWrite;

        /// <summary>
        /// Occurs when descriptor read.
        /// </summary>
        public event BleDeviceDescriptorReadEventHandler DescriptorRead;

        /// <summary>
        /// Occurs when descriptor write.
        /// </summary>
        public event BleDeviceDescriptorWriteEventHandler DescriptorWrite;

        /// <summary>
        /// Occurs when read remote rssi.
        /// </summary>
        public event BleDeviceReadRemoteRssiEventHandler ReadRemoteRssi;

        /// <summary>
        /// Occurs when reliable write completed.
        /// </summary>
        public event BleDeviceReliableWriteCompletedEventHandler ReliableWriteCompleted;

        /// <summary>
        /// Occurs when services discovered.
        /// </summary>
        public event BleDeviceServicesDiscoveredEventHandler ServicesDiscovered;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public abstract string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the ID. In Android and Windows this is a Mac Address on IOS this is a UUID
        /// </summary>
        /// <value>The UUID or mac address of the device.</value>
        public abstract string ID
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the state of the connection.
        /// </summary>
        /// <value>The state of the connection.</value>
        public abstract BleConnectionState ConnectionState
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has performed service discovered.
        /// </summary>
        /// <value>true</value>
        /// <c>false</c>
        public abstract bool IsServiceDiscovered
        {
            get;
            protected set;
        }

        /// <summary>
        /// Connect to this device.
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// Disconnect this device.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <returns>The characteristic.</returns>
        /// <param name="id">Identifier.</param>
        public abstract object GetCharacteristic(Guid id);

        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <returns>The characteristic.</returns>
        /// <param name="id">Identifier.</param>
        public abstract object GetCharacteristic(ushort id);

        /// <summary>
        /// 
        /// </summary>
        public abstract void DiscoverServices();

        /// <summary>
        /// Enables the notifications on a characteristic.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        public abstract bool EnableNotifications(object characteristic, bool enabled);

        /// <summary>
        /// Reads the characteristic.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        public abstract void ReadCharacteristic(object characteristic);

        /// <summary>
        /// Writes the characteristic.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReliable">If set to <c>true</c> is reliable.</param>
        public abstract void WriteCharacteristic(object characteristic, byte[] value, bool isReliable);

        /// <summary>
        /// Raises the connection state changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnConnectionStateChanged(IBleDevice sender, BleConnectionStateChangedEventArgs e)
        {
            ConnectionState = e.NewState;
            if (ConnectionStateChanged != null)
            {
                ConnectionStateChanged(sender, e);
            }

        }

        /// <summary>
        /// Raises the characteristic read event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnCharacteristicRead(IBleDevice sender, BleCharacteristicReadEventArgs e)
        {
            if (CharacteristicRead != null)
            {
                CharacteristicRead(sender, e);
            }
        }

        /// <summary>
        /// Raises the characteristic write event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnCharacteristicWrite(IBleDevice sender, BleCharacteristicWriteEventArgs e)
        {
            if (CharacteristicWrite != null)
            {
                CharacteristicWrite(sender, e);
            }
        }

        /// <summary>
        /// Raises the characteristic changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnCharacteristicChanged(IBleDevice sender, BleCharacteristicChangedEventArgs e)
        {
            if (CharacteristicChanged != null)
            {
                CharacteristicChanged(sender, e);
            }
        }

        /// <summary>
        /// Raises the descriptor read event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnDescriptorRead(IBleDevice sender, BleDescriptorReadEventArgs e)
        {
            if (DescriptorRead != null)
            {
                DescriptorRead(sender, e);
            }
        }

        /// <summary>
        /// Raises the descriptor write event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnDescriptorWrite(IBleDevice sender, BleDescriptorWriteEventArgs e)
        {
            if (DescriptorWrite != null)
            {
                DescriptorWrite(sender, e);
            }
        }

        /// <summary>
        /// Raises the read remote rssi event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnReadRemoteRssi(IBleDevice sender, BleReadRemoteRssiEventArgs e)
        {
            if (ReadRemoteRssi != null)
            {
                ReadRemoteRssi(sender, e);
            }
        }

        /// <summary>
        /// Raises the reliable write completed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnReliableWriteCompleted(IBleDevice sender, BleReliableWriteCompletedEventArgs e)
        {
            if (ReliableWriteCompleted != null)
            {
                ReliableWriteCompleted(sender, e);
            }
        }

        /// <summary>
        /// Raises the services discovered event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnServicesDiscovered(IBleDevice sender, BleServicesDiscoveredEventArgs e)
        {
            if (ServicesDiscovered != null)
            {
                ServicesDiscovered(sender, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        public void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        
    }
}
