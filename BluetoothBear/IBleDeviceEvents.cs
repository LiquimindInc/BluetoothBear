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
    /// Bluetooth LE device connection state changed handler. 
    /// </summary>
    public delegate void BleDeviceConnectionStateChangedEventHandler(IBleDevice sender, BleConnectionStateChangedEventArgs e);

    /// <summary>
    /// Bluetooth LE device characteristic chagned event handler. 
    /// </summary>
    public delegate void BleDeviceCharacteristicChangedEventHandler(IBleDevice sender, BleCharacteristicChangedEventArgs e);

    /// <summary>
    ///  Bluetooth LE device characteristic read event handler.
    /// </summary>
    public delegate void BleDeviceCharacteristicReadEventHandler(IBleDevice sender, BleCharacteristicReadEventArgs e);

    /// <summary>
    ///  Bluetooth LE device characteristic write event handler.
    /// </summary>
    public delegate void BleDeviceCharacteristicWriteEventHandler(IBleDevice sender, BleCharacteristicWriteEventArgs e);

    /// <summary>
    /// Bluetooth LE device descriptor read event handler.
    /// </summary>
    public delegate void BleDeviceDescriptorReadEventHandler(IBleDevice sender, BleDescriptorReadEventArgs e);

    /// <summary>
    /// Bluetooth LE device descriptor write event handler.
    /// </summary>
    public delegate void BleDeviceDescriptorWriteEventHandler(IBleDevice sender, BleDescriptorWriteEventArgs e);

    /// <summary>
    /// Bluetooth LE device read remote rssi event handler.
    /// </summary>
    public delegate void BleDeviceReadRemoteRssiEventHandler(IBleDevice sender, BleReadRemoteRssiEventArgs e);

    /// <summary>
    /// Bluetooth LE device write completed event handler. 
    /// </summary>
    public delegate void BleDeviceReliableWriteCompletedEventHandler(IBleDevice sender, BleReliableWriteCompletedEventArgs e);

    /// <summary>
    /// Bluetooth LE device services discovered event handler.
    /// </summary>
    public delegate void BleDeviceServicesDiscoveredEventHandler(IBleDevice sender, BleServicesDiscoveredEventArgs e);



    /// <summary>
    /// Ble connection state changed event arguments.
    /// </summary>
    public class BleConnectionStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the old state.
        /// </summary>
        /// <value>The old state.</value>
        public BleConnectionState OldState { get; private set; }
        /// <summary>
        /// Gets the new state.
        /// </summary>
        /// <value>The new state.</value>
        public BleConnectionState NewState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleConnectionStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldState">Old state.</param>
        /// <param name="newState">New state.</param>
        public BleConnectionStateChangedEventArgs(BleConnectionState oldState, BleConnectionState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

    }


    /// <summary>
    /// Ble characteristic read event arguments.
    /// </summary>
    public class BleCharacteristicReadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <value>The characteristic.</value>
        public object Characteristic { get; private set; }
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public BleGattOperationState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleCharacteristicReadEventArgs"/> class.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="state">State.</param>
        public BleCharacteristicReadEventArgs(object characteristic, BleGattOperationState state)
        {
            Characteristic = characteristic;
            State = state;
        }
    }

    /// <summary>
    /// Ble characteristic write event arguments.
    /// </summary>
    public class BleCharacteristicWriteEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <value>The characteristic.</value>
        public object Characteristic { get; private set; }
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public BleGattOperationState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleCharacteristicWriteEventArgs"/> class.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="state">State.</param>
        public BleCharacteristicWriteEventArgs(object characteristic, BleGattOperationState state)
        {
            Characteristic = characteristic;
            State = state;
        }
    }

    /// <summary>
    /// Ble characteristic changed arguments.
    /// </summary>
    public class BleCharacteristicChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        /// <value>The characteristic.</value>
        public object Characteristic { get; private set; }
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public BleGattOperationState State { get; private set; }

        public byte[] CharacteristicValue { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleCharacteristicChangedArgs"/> class.
        /// </summary>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="state">State.</param>
        public BleCharacteristicChangedEventArgs(object characteristic, byte[] charValue, BleGattOperationState state)
        {
            Characteristic = characteristic;
            State = state;
            CharacteristicValue = charValue;
        }
    }

    /// <summary>
    /// Ble descriptor read event arguments.
    /// </summary>
    public class BleDescriptorReadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the descriptor.
        /// </summary>
        /// <value>The descriptor.</value>
        public object Descriptor { get; private set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public BleGattOperationState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleDescriptorReadEventArgs"/> class.
        /// </summary>
        /// <param name="descriptor">Descriptor.</param>
        /// <param name="state">State.</param>
        public BleDescriptorReadEventArgs(object descriptor, BleGattOperationState state)
        {
            Descriptor = descriptor;
            State = state;
        }
    }

    /// <summary>
    /// Ble descriptor write event arguments.
    /// </summary>
    public class BleDescriptorWriteEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the descriptor.
        /// </summary>
        /// <value>The descriptor.</value>
        public object Descriptor { get; private set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public BleGattOperationState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleDescriptorWriteEventArgs"/> class.
        /// </summary>
        /// <param name="descriptor">Descriptor.</param>
        /// <param name="state">State.</param>
        public BleDescriptorWriteEventArgs(object descriptor, BleGattOperationState state)
        {
            Descriptor = descriptor;
            State = state;
        }
    }

    /// <summary>
    /// Ble read remote rssi event arguments.
    /// </summary>
    public class BleReadRemoteRssiEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the RSSI.
        /// </summary>
        /// <value>The RSS.</value>
        public int RSSI { get; private set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public BleGattOperationState State { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleReadRemoteRssiEventArgs"/> class.
        /// </summary>
        /// <param name="rssi">Rssi.</param>
        /// <param name="state">State.</param>
        public BleReadRemoteRssiEventArgs(int rssi, BleGattOperationState state)
        {
            RSSI = rssi;
            State = state;
        }
    }

    /// <summary>
    /// Ble reliable write completed event arguments.
    /// </summary>
    public class BleReliableWriteCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public BleGattOperationState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleReliableWriteCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="state">State.</param>
        public BleReliableWriteCompletedEventArgs(BleGattOperationState state)
        {
            State = state;
        }
    }

    /// <summary>
    /// Ble services discovered event arguments.
    /// </summary>
    public class BleServicesDiscoveredEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public BleGattOperationState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBleApi.BleServicesDiscoveredEventArgs"/> class.
        /// </summary>
        /// <param name="state">State.</param>
        public BleServicesDiscoveredEventArgs(BleGattOperationState state)
        {
            State = state;
        }
    }


}
