using System;

using FTDI.D2XX.Wrapper.FtdiInterop;
using FTDI.D2XX.Wrapper.FtdiInterop.Enums;
using FTDI.D2XX.Wrapper.FtdiInterop.Models;

namespace FTDI.D2XX.Wrapper
{
    public class FtdiD2xxDevice : IDisposable
    {
        private IntPtr ftHandle;
        private bool isDisposed;

        public FtdiD2xxDevice()
        {
            ftHandle = IntPtr.Zero;
            isDisposed = false;
        }

        //
        // Public properties
        //

        public bool IsOpen => ftHandle != IntPtr.Zero;

        //
        // Public methods
        //

        /// <summary>
        /// Open the device by its index
        /// </summary>
        /// <param name="deviceNumber">The zero based index of the device</param>
        public void OpenByIndex(int deviceNumber)
        {
            ThrowIfDisposed();
            ThrowIfOpen();

            var result = FtFunctions.FT_Open(deviceNumber, out IntPtr ftHandle);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_Open));

            this.ftHandle = ftHandle;
        }

        /// <summary>
        /// Open the device by its serial number
        /// </summary>
        /// <param name="serial">The serial number of the device</param>
        public void OpenBySerial(string serial)
        {
            ThrowIfDisposed();
            ThrowIfOpen();

            var result = FtFunctions.FT_OpenEx(serial, FT_OPEN_BY.FT_OPEN_BY_SERIAL_NUMBER, out IntPtr ftHandle);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_OpenEx));

            this.ftHandle = ftHandle;
        }

        /// <summary>
        /// Open the device by its description
        /// </summary>
        /// <param name="description">The description of the device</param>
        public void OpenByDescription(string description)
        {
            ThrowIfDisposed();
            ThrowIfOpen();

            var result = FtFunctions.FT_OpenEx(description, FT_OPEN_BY.FT_OPEN_BY_DESCRIPTION, out IntPtr ftHandle);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_OpenEx));

            this.ftHandle = ftHandle;
        }

        /// <summary>
        /// Open the device by its location
        /// </summary>
        /// <param name="location">The location identifier of the device</param>
        public void OpenByLocation(long location)
        {
            ThrowIfDisposed();
            ThrowIfOpen();

            var result = FtFunctions.FT_OpenEx(in location, FT_OPEN_BY.FT_OPEN_BY_LOCATION, out IntPtr ftHandle);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_OpenEx));

            this.ftHandle = ftHandle;
        }

        /// <summary>
        /// Close the device
        /// </summary>
        public void Close()
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_Close(ftHandle);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_Close));

            ftHandle = IntPtr.Zero;
        }

        /// <summary>
        /// Read data from the device.
        /// 
        /// The number of bytes in the receive queue can be determined by calling GetStatus or GetQueueStatus, and passed to Read as bytesToRead so that the function reads the device and returns immediately.
        /// 
        /// When a read timeout value has been specified in a previous call to SetTimeouts, Read returns when the timer expires or bytesToRead have been read, whichever occurs first.
        /// If the timeout occurred, Read reads available data into the buffer and the return value will be less than the returned read bytes count.
        /// </summary>
        /// <param name="buffer">Buffer that will receive bytes read from the device</param>
        /// <param name="bytesToRead">The maximum number of bytes to read</param>
        /// <returns>The number of bytes read from the device.</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Read(byte[] buffer, int bytesToRead)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            if (bytesToRead > buffer.Length) throw new ArgumentException($"{nameof(bytesToRead)} must be less than or equal to the length of {nameof(buffer)}", nameof(bytesToRead));

            var result = FtFunctions.FT_Read(ftHandle, buffer, bytesToRead, out int bytesRead);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_Read));

            return bytesRead;
        }

        /// <summary>
        /// Read data from the device.
        /// 
        /// The number of bytes in the receive queue can be determined by calling GetStatus or GetQueueStatus, and passed to Read as bytesToRead so that the function reads the device and returns immediately.
        /// 
        /// When a read timeout value has been specified in a previous call to SetTimeouts, Read returns when the timer expires or bytesToRead have been read, whichever occurs first.
        /// If the timeout occurred, Read reads available data into the buffer and the return value will be less than the returned read bytes count.
        /// </summary>
        /// <param name="readBuffer">Buffer that will receive bytes read from the device</param>
        /// <returns>The number of bytes read from the device.</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Read(Span<byte> readBuffer)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            FT_STATUS result;
            int bytesRead;
            unsafe
            {
                fixed (byte* ptr = readBuffer)
                {
                    result = FtFunctions.FT_Read(ftHandle, (IntPtr)ptr, readBuffer.Length, out bytesRead);
                }
            }
            ThrowIfNotOK(result, nameof(FtFunctions.FT_Read));
            return bytesRead;
        }

        /// <summary>
        /// Read a single byte from the device
        /// </summary>
        /// <returns>The byte read from the device, or null if the read timed out</returns>
        public byte? ReadByte()
        {
            Span<byte> readBuffer = stackalloc byte[1];
            int bytesRead = Read(readBuffer);
            return bytesRead > 0 ? readBuffer[0] : null;
        }

        /// <summary>
        /// Write data to the device.
        /// </summary>
        /// <param name="buffer">Buffer that contains the data to be written to the device.</param>
        /// <param name="bytesToWrite">Maximum number of bytes to write to the device.</param>
        /// <returns>The number of bytes written to the device.</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Write(byte[] buffer, int bytesToWrite)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            if (bytesToWrite > buffer.Length) throw new ArgumentException($"{nameof(bytesToWrite)} must be less than or equal to the length of {nameof(buffer)}", nameof(bytesToWrite));

            var result = FtFunctions.FT_Write(ftHandle, buffer, bytesToWrite, out int bytesWritten);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_Write));

            return bytesWritten;
        }

        /// <summary>
        /// Write data to the device.
        /// </summary>
        /// <param name="writeBuffer">Buffer that contains the data to be written to the device.</param>
        /// <returns>The number of bytes written to the device.</returns>
        public int Write(Span<byte> writeBuffer)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            FT_STATUS result;
            int bytesWritten;
            unsafe
            {
                fixed (byte* ptr = writeBuffer)
                {
                    result = FtFunctions.FT_Write(ftHandle, (IntPtr)ptr, writeBuffer.Length, out bytesWritten);
                }
            }
            ThrowIfNotOK(result, nameof(FtFunctions.FT_Write));
            return bytesWritten;
        }

        /// <summary>
        /// Write a single byte to the device
        /// </summary>
        /// <param name="writeByte">Single byte to be written to the device.</param>
        /// <returns>The number of bytes written to the device.</returns>
        public int WriteByte(byte writeByte)
        {
            Span<byte> writeBuffer = stackalloc byte[] { writeByte };
            return Write(writeBuffer);
        }

        /// <summary>
        /// Sets the read and write timeouts for the device.
        /// </summary>
        /// <param name="readTimeout">Read timeout in milliseconds.</param>
        /// <param name="writeTimeout">Write timeout in milliseconds.</param>
        public void SetTimeouts(int readTimeout, int writeTimeout)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_SetTimeouts(ftHandle, readTimeout, writeTimeout);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_SetTimeouts));
        }

        /// <summary>
        /// Sets the baud rate for the device
        /// </summary>
        /// <param name="baudRate">The baud rate</param>
        public void SetBaudRate(int baudRate)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_SetBaudRate(ftHandle, baudRate);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_SetBaudRate));
        }

        /// <summary>
        /// Send a reset command to the device
        /// </summary>
        public void ResetDevice()
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_ResetDevice(ftHandle);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_ResetDevice));
        }

        /// <summary>
        /// Purges transmit and receive buffers for the device, depending on the flags given
        /// </summary>
        /// <param name="purgeFlags">Combination of FT_PURGE_RX and FT_PURGE_TX.</param>
        public void Purge(FT_PURGE purgeFlags)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_Purge(ftHandle, purgeFlags);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_Purge));

        }

        /// <summary>
        /// Enables different chip modes.
        /// </summary>
        /// <param name="outputMask">Bit mode output mask. This sets up which bits are inputs and outputs.
        /// A bit value of 0 sets the corresponding pin to an input, a bit value of 1 sets the corresponding pin to an output.
        /// In the case of CBUS Bit Bang, the upper nibble of this value controls which pins are inputs and outputs, while the lower nibble controls which of the outputs are high and low.
        /// </param>
        /// <param name="bitmode">Mode value.</param>
        public void SetBitMode(byte outputMask, FT_BITMODE bitmode)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_SetBitMode(ftHandle, outputMask, bitmode);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_SetBitMode));
        }

        /// <summary>
        /// Gets the instantaneous value of the data bus
        /// </summary>
        /// <returns>The instantaneous data bus value.</returns>
        public FT_BITMODE GetBitMode()
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_GetBitMode(ftHandle, out FT_BITMODE bitmode);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_GetBitMode));

            return bitmode;
        }

        /// <summary>
        /// Sets the data characteristics for the device.
        /// </summary>
        /// <param name="wordLength">Number of bits per word - must be FT_BITS_8 or FT_BITS_7.</param>
        /// <param name="stopBits">Number of stop bits - must be FT_STOP_BITS_1 or FT_STOP_BITS_2.</param>
        /// <param name="parity">Parity - must be FT_PARITY_NONE, FT_PARITY_ODD, FT_PARITY_EVEN, FT_PARITY_MARK or FT_PARITY SPACE.</param>
        public void SetDataCharacteristics(FT_BITS wordLength, FT_STOP_BITS stopBits, FT_PARITY parity)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_SetDataCharacteristics(ftHandle, wordLength, stopBits, parity);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_SetDataCharacteristics));
        }

        /// <summary>
        /// Sets the flow control for the device.
        /// </summary>
        /// <param name="flowControl">Must be one of FT_FLOW_NONE, FT_FLOW_RTS_CTS, FT_FLOW_DTR_DSR or FT_FLOW_XON_XOFF.</param>
        /// <param name="xOnCharacter">Character used to signal Xon. Only used if flow control is FT_FLOW_XON_XOFF.</param>
        /// <param name="xOffCharacter">Character used to signal Xoff. Only used if flow control is FT_FLOW_XON_XOFF.</param>
        public void SetFlowControl(FT_FLOW flowControl, byte xOnCharacter = 0x11, byte xOffCharacter = 0x013)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_SetFlowControl(ftHandle, flowControl, xOnCharacter, xOffCharacter);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_SetFlowControl));
        }

        /// <summary>
        /// Sets the Data Terminal Ready (DTR) control signal.
        /// </summary>
        /// <param name="enabled">If true, the DTR line is asserted, otherwise it is de-asserted.</param>
        public void SetDtr(bool enabled)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            if (enabled)
            {
                var result = FtFunctions.FT_SetDtr(ftHandle);
                ThrowIfNotOK(result, nameof(FtFunctions.FT_SetDtr));
            }
            else
            {
                var result = FtFunctions.FT_ClrDtr(ftHandle);
                ThrowIfNotOK(result, nameof(FtFunctions.FT_ClrDtr));
            }
        }

        /// <summary>
        /// Sets the Request To Send (RTS) control signal.
        /// </summary>
        /// <param name="enabled">If true, the RTS line is asserted, otherwise it is de-asserted.</param>
        public void SetRts(bool enabled)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            if (enabled)
            {
                var result = FtFunctions.FT_SetRts(ftHandle);
                ThrowIfNotOK(result, nameof(FtFunctions.FT_SetRts));
            }
            else
            {
                var result = FtFunctions.FT_ClrRts(ftHandle);
                ThrowIfNotOK(result, nameof(FtFunctions.FT_ClrRts));
            }
        }

        /// <summary>
        /// Sets the BREAK condition for the device.
        /// </summary>
        /// <param name="enabled">If true, the Break condition is set, otherwise it is reset.</param>
        public void SetBreak(bool enabled)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            if (enabled)
            {
                var result = FtFunctions.FT_SetBreakOn(ftHandle);
                ThrowIfNotOK(result, nameof(FtFunctions.FT_SetBreakOn));
            }
            else
            {
                var result = FtFunctions.FT_SetBreakOff(ftHandle);
                ThrowIfNotOK(result, nameof(FtFunctions.FT_SetBreakOff));
            }
        }

        /// <summary>
        /// Gets the device status including number of bytes in the receive queue, number of bytes in the transmit queue, and the current event status.
        /// </summary>
        /// <param name="amountInRxQueue">The number of bytes in the receive queue.</param>
        /// <param name="amountInTxQueue">The number of bytes in the transmit queue.</param>
        /// <param name="eventStatus">The current state of the event status.</param>
        public void GetStatus(out int amountInRxQueue, out int amountInTxQueue, out uint eventStatus)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_GetStatus(ftHandle, out amountInRxQueue, out amountInTxQueue, out eventStatus);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_GetStatus));
        }

        /// <summary>
        /// Gets the number of bytes in the receive queue.
        /// </summary>
        /// <param name="amountInRxQueue">The number of bytes in the receive queue.</param>
        public void GetQueueStatus(out int amountInRxQueue)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_GetQueueStatus(ftHandle, out amountInRxQueue);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_GetQueueStatus));
        }

        /// <summary>
        /// Gets the modem status and line status from the device.
        /// 
        /// Line status is only available on Windows and Windows CE.
        /// </summary>
        /// <param name="modemStatus">The modem status and line status from the device.</param>
        public void GetModemStatus(out FT_MODEM_LINE_STATUS modemStatus)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_GetModemStatus(ftHandle, out modemStatus);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_GetModemStatus));
        }

        /// <summary>
        /// Get the current value of the latency timer.
        /// 
        /// In the FT8U232AM and FT8U245AM devices, the receive buffer timeout that is used to flush remaining data from the receive buffer was fixed at 16 ms.
        /// In all other FTDI devices, this timeout is programmable and can be set at 1 ms intervals between 2ms and 255 ms.
        /// This allows the device to be better optimized for protocols requiring faster response times from short data packets.
        /// </summary>
        /// <returns>The latency timer value in milliseconds</returns>
        public byte GetLatencyTimer()
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_GetLatencyTimer(ftHandle, out byte timerValue);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_GetLatencyTimer));

            return timerValue;
        }

        /// <summary>
        /// Set the latency timer value.
        /// 
        /// In the FT8U232AM and FT8U245AM devices, the receive buffer timeout that is used to flush remaining data from the receive buffer was fixed at 16 ms.
        /// In all other FTDI devices, this timeout is programmable and can be set at 1 ms intervals between 2ms and 255 ms.
        /// This allows the device to be better optimized for protocols requiring faster response times from short data packets.
        /// </summary>
        /// <param name="timerValue">The latency timer value in milliseconds. Valid range is 2 – 255.</param>
        public void SetLatencyTimer(byte timerValue)
        {
            ThrowIfDisposed();
            ThrowIfNotOpen();

            var result = FtFunctions.FT_SetLatencyTimer(ftHandle, timerValue);
            ThrowIfNotOK(result, nameof(FtFunctions.FT_SetLatencyTimer));
        }

        //
        // Public static methods
        //

        /// <summary>
        /// Gets device information for all connected D2XX devices.
        /// 
        /// Location ID information is not returned for devices that are currently open.
        /// 
        /// Information is not available for devices which are open in other processes,
        /// in this case, the Flags parameter of the FT_DEVICE_LIST_INFO_NODE will indicate that the device is open, but other fields will be unpopulated.
        /// </summary>
        /// <returns>Array of FT_DEVICE_LIST_INFO_NODES which contains all available data on each device.</returns>
        public static FT_DEVICE_LIST_INFO_NODE[] GetDevices()
        {
            var createDeviceInfoListResult = FtFunctions.FT_CreateDeviceInfoList(out int lpdwNumDevs);
            ThrowIfNotOK(createDeviceInfoListResult, nameof(FtFunctions.FT_CreateDeviceInfoList));

            FT_DEVICE_LIST_INFO_NODE[] devices = new FT_DEVICE_LIST_INFO_NODE[lpdwNumDevs];
            var getDeviceInfoListResult = FtFunctions.FT_GetDeviceInfoList(devices, out lpdwNumDevs);
            ThrowIfNotOK(getDeviceInfoListResult, nameof(FtFunctions.FT_GetDeviceInfoList));

            return devices;
        }


        //
        // Private helpers
        //

        private static void ThrowIfNotOK(FT_STATUS ftStatus, string functionName)
        {
            if (ftStatus != FT_STATUS.FT_OK)
            {
                Exception ex = new Exception($"{functionName} failed: {ftStatus}");
                ex.Data["Function"] = functionName;
                ex.Data["FT_STATUS"] = ftStatus;
                throw ex;
            }
        }

        private void ThrowIfOpen()
        {
            if (IsOpen) throw new InvalidOperationException("Device is already open");
        }

        private void ThrowIfNotOpen()
        {
            if (!IsOpen) throw new InvalidOperationException("Device is not open");
        }

        private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(isDisposed, this);

        //
        // Dispose methods
        //

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    // Currently, we have none.
                }

                // Free unmanaged resources (unmanaged objects)
                if (IsOpen)
                {
                    FtFunctions.FT_Close(ftHandle);
                    ftHandle = IntPtr.Zero;
                }

                // Set large fields to null
                // Currently, we have none.

                // Set object to disposed.
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~FtdiD2xxDevice()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
    }
}
