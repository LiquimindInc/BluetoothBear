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
    /// Bluetooth LE Gatt Operation Results
    /// </summary>
    public enum BleGattOperationState
    {
        /// <summary>
        /// Gatt operation completed succesfully.
        /// </summary>
        Success,
        /// <summary>
        /// Gatt operation failed.
        /// </summary>
        Failure,
        /// <summary>
        /// Gatt operation not permitted.
        /// </summary>
        NotPermitted,

        /// <summary>
        /// Gatt operation not supported.
        /// </summary>
        RequestNotSupported,
    };


    /// <summary>
    /// Bluetooth LE device found event handler.
    /// </summary>
    public delegate void BleDeviceFoundEventHandler(IBleManager sender, BleDevice deviceFound);


    /// <summary>
    /// Interface for interacting with a bluetooth module acting in central mode. 
    /// </summary>
    public interface IBleManager
    {
        /// <summary>
        /// Occurs when ble device found.
        /// </summary>
        event BleDeviceFoundEventHandler BleDeviceFound;

        /// <summary>
        /// Determines whether this instance supports bluetooth low energy.
        /// </summary>
        /// <returns><c>true</c> if bluetooth low energy is supported; otherwise, <c>false</c>.</returns>
        bool IsBleSupported();

        /// <summary>
        /// Determines whether bluetooth low energy has been enabled.
        /// </summary>
        /// <returns><c>true</c> if bluetooth low energy is enabled; otherwise, <c>false</c>.</returns>
        bool IsBleEnabled();

        /// <summary>
        /// Enables the ble device. Or throws a NotImplemented exception for unsupported platforms.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        void TrySetBleEnabled(bool enabled);

        /// <summary>
        /// Starts the ble scan.
        /// </summary>
        void StartBleScan();

        /// <summary>
        /// Stops the ble scan.
        /// </summary>
        void StopBleScan();
    }
}
