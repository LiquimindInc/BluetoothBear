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
    /// Bluetooth LE connection states.
    /// </summary>
    public enum BleConnectionState
    {

        /// <summary>
        /// The device is not currently connected. 
        /// </summary>
        Disconnected,
        /// <summary>
        /// The device is not currently connected. 
        /// </summary>
        Disconnecting,
        /// <summary>
        /// The device is connected but no services have been discovered so it cannot be used.
        /// </summary>
        Connected,
        /// <summary>
        /// The device is in the process of creating a connection
        /// </summary>
        Connecting,
        /// <summary>
        /// The device has been connected and all the services have been enumerated. 
        /// </summary>
        ConnectedWithServices
    };


    /// <summary>
    /// Interface that defines the basic operations of Bluetooth LE Device
    /// </summary>
    public interface IBleDevice
    {
        /// <summary>
        /// Occurs when connection state changed.
        /// </summary>
        event BleDeviceConnectionStateChangedEventHandler ConnectionStateChanged;
        /// <summary>
        /// Occurs when characteristic changed.
        /// </summary>
        event BleDeviceCharacteristicChangedEventHandler CharacteristicChanged;
        /// <summary>
        /// Occurs when characteristic read.
        /// </summary>
        event BleDeviceCharacteristicReadEventHandler CharacteristicRead;
        /// <summary>
        /// Occurs when characteristic write.
        /// </summary>
        event BleDeviceCharacteristicWriteEventHandler CharacteristicWrite;
        /// <summary>
        /// Occurs when descriptor read.
        /// </summary>
        event BleDeviceDescriptorReadEventHandler DescriptorRead;
        /// <summary>
        /// Occurs when descriptor write.
        /// </summary>
        event BleDeviceDescriptorWriteEventHandler DescriptorWrite;

        /// <summary>
        /// Occurs when read remote rssi.
        /// </summary>
        event BleDeviceReadRemoteRssiEventHandler ReadRemoteRssi;

        /// <summary>
        /// Occurs when reliable write completed.
        /// </summary>
        event BleDeviceReliableWriteCompletedEventHandler ReliableWriteCompleted;

        /// <summary>
        /// Occurs when services discovered.
        /// </summary>
        event BleDeviceServicesDiscoveredEventHandler ServicesDiscovered;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the ID. In Android and Windows this is a Mac Address on IOS this is a UUID
        /// </summary>
        /// <value>The UUI.</value>
        string ID { get; }

        /// <summary>
        /// Gets the state of the connection.
        /// </summary>
        /// <value>The state of the connection.</value>
        BleConnectionState ConnectionState { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has performed service discovered.
        /// </summary>
        /// <value><c>true</c> if this instance is service discovered; otherwise, <c>false</c>.</value>
        bool IsServiceDiscovered { get; }

        /// <summary>
        /// Connect to this device.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect this device.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <returns>The characteristic.</returns>
        /// <param name="id">Identifier.</param>
        object GetCharacteristic(Guid id);

        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <returns>The characteristic.</returns>
        /// <param name="id">Identifier.</param>
        object GetCharacteristic(UInt16 id);

        /// <summary>
        /// Enables the notifications on a characteristic.
        /// </summary>
        /// <returns><c>true</c>, if notifications was enabled, <c>false</c> otherwise.</returns>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        bool EnableNotifications(object characteristic, bool enabled);


        /// <summary>
        /// Reads the characteristic.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        void ReadCharacteristic(object characteristic);

        /// <summary>
        /// Writes the characteristic.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReliable">If set to <c>true</c> is reliable.</param>
        void WriteCharacteristic(object characteristic, byte[] value, bool isReliable);

        void DiscoverServices();

        /// <summary>
        /// Raises the connection state changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnConnectionStateChanged(IBleDevice sender, BleConnectionStateChangedEventArgs e);

        /// <summary>
        /// Raises the characteristic read event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnCharacteristicRead(IBleDevice sender, BleCharacteristicReadEventArgs e);

        /// <summary>
        /// Raises the characteristic write event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnCharacteristicWrite(IBleDevice sender, BleCharacteristicWriteEventArgs e);

        /// <summary>
        /// Raises the characteristic changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnCharacteristicChanged(IBleDevice sender, BleCharacteristicChangedEventArgs e);

        /// <summary>
        /// Raises the descriptor read event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnDescriptorRead(IBleDevice sender, BleDescriptorReadEventArgs e);

        /// <summary>
        /// Raises the descriptor write event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnDescriptorWrite(IBleDevice sender, BleDescriptorWriteEventArgs e);

        /// <summary>
        /// Raises the read remote rssi event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnReadRemoteRssi(IBleDevice sender, BleReadRemoteRssiEventArgs e);

        /// <summary>
        /// Raises the reliable write completed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnReliableWriteCompleted(IBleDevice sender, BleReliableWriteCompletedEventArgs e);

        /// <summary>
        /// Raises the services discovered event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnServicesDiscovered(IBleDevice sender, BleServicesDiscoveredEventArgs e);


    }
}
