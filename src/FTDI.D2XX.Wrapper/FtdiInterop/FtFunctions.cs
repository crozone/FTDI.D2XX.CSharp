using System.Runtime.InteropServices;

using FTDI.D2XX.Wrapper.FtdiInterop.Enums;
using FTDI.D2XX.Wrapper.FtdiInterop.Models;

// FT_HANDLE is a PVOID, which is a native pointer width value.
using FT_HANDLE = System.IntPtr;

namespace FTDI.D2XX.Wrapper.FtdiInterop
{
    /// <summary>
    /// FTDI ftd2xx.dll interop
    /// </summary>
    /// <seealso href="https://www.ftdichip.com/Support/Documents/ProgramGuides/D2XX_Programmer%27s_Guide(FT_000071).pdf">
    /// D2XX_Programmer%27s_Guide(FT_000071).pdf
    /// </seealso>
    public static class FtFunctions
    {
        private const string Ftd2xxLibName = "ftd2xx.dll";

        /// <summary>
        /// Open the device and return a handle which will be used for subsequent accesses.
        /// </summary>
        /// <param name="deviceIndex">Index of the device to open. Indices are 0 based.</param>
        /// <param name="ftHandle">Pointer to a variable of type FT_HANDLE where the handle will be stored. This handle must be used to access the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_Open([In] int deviceIndex, [Out] out FT_HANDLE ftHandle);

        /// <summary>
        /// Open the specified device and return a handle that will be used for subsequent accesses.
        /// The device can be specified by its serial number, device description or location.
        /// This function can also be used to open multiple devices simultaneously.
        /// Multiple devices can be specified by serial number, device description or location ID(location information derived from the physical location of a device on USB).
        /// Location IDs for specific USB ports can be obtained using the utility USBView and are given in hexadecimal format.
        /// Location IDs for devices connected to a system can be obtained by calling FT_GetDeviceInfoList or FT_ListDevices with the appropriate flags.
        /// </summary>
        /// <param name="pvArg1">Pointer to an argument whose type depends on the value of dwFlags. This is the pointer to string overload for FT_OPEN_BY_SERIAL_NUMBER and FT_OPEN_BY_DESCRIPTION</param>
        /// <param name="dwFlags">Must be FT_OPEN_BY_SERIAL_NUMBER or FT_OPEN_BY_DESCRIPTION</param>
        /// <param name="ftHandle">Pointer to a variable of type FT_HANDLE where the handle will be stored. This handle must be used to access the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName, EntryPoint = "FT_OpenEx")]
        public static extern FT_STATUS FT_OpenEx([In] string pvArg1, [In] FT_OPEN_BY dwFlags, [Out] out FT_HANDLE ftHandle);

        /// <summary>
        /// Open the specified device and return a handle that will be used for subsequent accesses.
        /// The device can be specified by its serial number, device description or location.
        /// This function can also be used to open multiple devices simultaneously.
        /// Multiple devices can be specified by serial number, device description or location ID(location information derived from the physical location of a device on USB).
        /// Location IDs for specific USB ports can be obtained using the utility USBView and are given in hexadecimal format.
        /// Location IDs for devices connected to a system can be obtained by calling FT_GetDeviceInfoList or FT_ListDevices with the appropriate flags.
        /// </summary>
        /// <param name="pvArg1">Pointer to an argument whose type depends on the value of dwFlags. This is the pointer to long overload for FT_OPEN_BY_LOCATION</param>
        /// <param name="dwFlags">Must be FT_OPEN_BY_LOCATION</param>
        /// <param name="ftHandle">Pointer to a variable of type FT_HANDLE where the handle will be stored. This handle must be used to access the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName, EntryPoint = "FT_OpenEx")]
        public static extern FT_STATUS FT_OpenEx([In] in long pvArg1, [In] FT_OPEN_BY dwFlags, [Out] out FT_HANDLE ftHandle);

        /// <summary>
        /// Close an open device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_Close([In] FT_HANDLE ftHandle);

        /// <summary>
        /// Read data from the device.
        /// 
        /// FT_Read always returns the number of bytes read in lpdwBytesReturned.
        /// This function does not return until dwBytesToRead bytes have been read into the buffer.
        /// The number of bytes in the receive queue can be determined by calling FT_GetStatus or FT_GetQueueStatus, and passed to FT_Read as dwBytesToRead so that the function reads the device and returns immediately.
        /// 
        /// When a read timeout value has been specified in a previous call to FT_SetTimeouts, FT_Read returns when the timer expires or dwBytesToRead have been read, whichever occurs first.
        /// If the timeout occurred, FT_Read reads available data into the buffer and returns FT_OK.
        /// An application should use the function return value and lpdwBytesReturned when processing the buffer.
        /// If the return value is FT_OK, and lpdwBytesReturned is equal to dwBytesToRead then FT_Read has completed normally.
        /// If the return value is FT_OK, and lpdwBytesReturned is less then dwBytesToRead then a timeout has occurred and the read has been partially completed.
        /// Note that if a timeout occurred and no data was read, the return value is still FT_OK.
        /// A return value of FT_IO_ERROR suggests an error in the parameters of the function, or a fatal error like a USB disconnect has occurred.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="lpBuffer">Pointer to the buffer that receives the data from the device.</param>
        /// <param name="dwBytesToRead">Number of bytes to be read from the device.</param>
        /// <param name="lpdwBytesReturned">Pointer to a variable of type DWORD which receives the number of bytes read from the device.</param>
        /// <returns>FT_OK if successful, FT_IO_ERROR otherwise.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_Read([In] FT_HANDLE ftHandle, [In] byte[] lpBuffer, [In] int dwBytesToRead, [Out] out int lpdwBytesReturned);

        /// <summary>
        /// Write data to the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="lpBuffer">Pointer to the buffer that contains the data to be written to the device.</param>
        /// <param name="dwBytesToWrite">Number of bytes to write to the device.</param>
        /// <param name="lpdwBytesWritten">Pointer to a variable of type DWORD which receives the number of bytes written to the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_Write([In] FT_HANDLE ftHandle, [In] byte[] lpBuffer, [In] int dwBytesToWrite, [Out] out int lpdwBytesWritten);

        /// <summary>
        /// This function sets the read and write timeouts for the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="dwReadTimeout">Read timeout in milliseconds.</param>
        /// <param name="dwWriteTimeout">Write timeout in milliseconds.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetTimeouts([In] FT_HANDLE ftHandle, [In] int dwReadTimeout, [In] int dwWriteTimeout);

        /// <summary>
        /// This function sets the data characteristics for the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="uWordLength">Number of bits per word - must be FT_BITS_8 or FT_BITS_7.</param>
        /// <param name="uStopBits">Number of stop bits - must be FT_STOP_BITS_1 or FT_STOP_BITS_2.</param>
        /// <param name="uParity">Parity - must be FT_PARITY_NONE, FT_PARITY_ODD, FT_PARITY_EVEN, FT_PARITY_MARK or FT_PARITY SPACE.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetDataCharacteristics([In] FT_HANDLE ftHandle, [In] FT_BITS uWordLength, [In] FT_STOP_BITS uStopBits, [In] FT_PARITY uParity);

        /// <summary>
        /// This function sets the flow control for the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="usFlowControl">Must be one of FT_FLOW_NONE, FT_FLOW_RTS_CTS, FT_FLOW_DTR_DSR or FT_FLOW_XON_XOFF.</param>
        /// <param name="uXon">Character used to signal Xon. Only used if flow control is FT_FLOW_XON_XOFF.</param>
        /// <param name="uXoff">Character used to signal Xoff. Only used if flow control is FT_FLOW_XON_XOFF.</param>
        /// <returns></returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetFlowControl([In] FT_HANDLE ftHandle, [In] FT_FLOW usFlowControl, [In] byte uXon, [In] byte uXoff);

        /// <summary>
        /// This function purges receive and transmit buffers in the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="uEventCh">Combination of FT_PURGE_RX and FT_PURGE_TX.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_Purge([In] FT_HANDLE ftHandle, [In] FT_PURGE uEventCh);

        /// <summary>
        /// This function sends a reset command to the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_ResetDevice([In] FT_HANDLE ftHandle);

        /// <summary>
        /// This function sets the baud rate for the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="dwBaudRate">Baud rate.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetBaudRate([In] FT_HANDLE ftHandle, [In] int dwBaudRate);

        /// <summary>
        /// This function sets the Data Terminal Ready (DTR) control signal.
        /// This function asserts the Data Terminal Ready (DTR) line of the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetDtr([In] FT_HANDLE ftHandle);

        /// <summary>
        /// This function clears the Data Terminal Ready (DTR) control signal.
        /// This function de-asserts the Data Terminal Ready (DTR) line of the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_ClrDtr([In] FT_HANDLE ftHandle);

        /// <summary>
        /// This function sets the Request To Send (RTS) control signal.
        /// This function asserts the Request To Send (RTS) line of the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetRts([In] FT_HANDLE ftHandle);

        /// <summary>
        /// This function clears the Request To Send (RTS) control signal.
        /// This function de-asserts the Request To Send (RTS) line of the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_ClrRts([In] FT_HANDLE ftHandle);

        /// <summary>
        /// Sets the BREAK condition for the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetBreakOn([In] FT_HANDLE ftHandle);

        /// <summary>
        /// Resets the BREAK condition for the device.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetBreakOff([In] FT_HANDLE ftHandle);

        /// <summary>
        /// Gets the device status including number of characters in the receive queue, number of characters in the transmit queue, and the current event status.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="lpdwAmountInRxQueue">Pointer to a variable of type DWORD which receives the number of characters in the receive queue.</param>
        /// <param name="lpdwAmountInTxQueue">Pointer to a variable of type DWORD which receives the number of characters in the transmit queue.</param>
        /// <param name="lpdwEventStatus">Pointer to a variable of type DWORD which receives the current state of the event status.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_GetStatus([In] FT_HANDLE ftHandle, [Out] out int lpdwAmountInRxQueue, [Out] out int lpdwAmountInTxQueue, [Out] out uint lpdwEventStatus);

        /// <summary>
        /// Gets the modem status and line status from the device.
        /// 
        /// The least significant byte of the lpdwModemStatus value holds the modem status.
        /// On Windows and Windows CE, the line status is held in the second least significant byte of the lpdwModemStatus value.
        /// The modem status is bit-mapped as follows: Clear To Send(CTS) = 0x10, Data Set Ready(DSR) = 0x20, Ring Indicator(RI) = 0x40, Data Carrier Detect(DCD) = 0x80.
        /// The line status is bit-mapped as follows: Overrun Error(OE) = 0x02, Parity Error(PE) = 0x04, Framing Error(FE) = 0x08, Break Interrupt(BI) = 0x10.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="lpdwModemStatus">Pointer to a variable of type DWORD which receives the modem status and line status from the device.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_GetModemStatus([In] FT_HANDLE ftHandle, [Out] out FT_MODEM_LINE_STATUS lpdwModemStatus);

        /// <summary>
        /// Gets the number of bytes in the receive queue.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="lpdwAmountInRxQueue">Pointer to a variable of type DWORD which receives the number of bytes in the receive queue.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_GetQueueStatus([In] FT_HANDLE ftHandle, [Out] out int lpdwAmountInRxQueue);

        /// <summary>
        /// Set the latency timer value.
        /// 
        /// In the FT8U232AM and FT8U245AM devices, the receive buffer timeout that is used to flush remaining data from the receive buffer was fixed at 16 ms.
        /// In all other FTDI devices, this timeout is programmable and can be set at 1 ms intervals between 2ms and 255 ms.
        /// This allows the device to be better optimized for protocols requiring faster response times from short data packets.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="ucTimer">Required value, in milliseconds, of latency timer. Valid range is 2 – 255.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetLatencyTimer([In] FT_HANDLE ftHandle, [In] byte ucTimer);

        /// <summary>
        /// Get the current value of the latency timer.
        /// 
        /// In the FT8U232AM and FT8U245AM devices, the receive buffer timeout that is used to flush remaining data from the receive buffer was fixed at 16 ms.
        /// In all other FTDI devices, this timeout is programmable and can be set at 1 ms intervals between 2ms and 255 ms.
        /// This allows the device to be better optimized for protocols requiring faster response times from short data packets.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="pucTimer">Pointer to unsigned char to store latency timer value.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_GetLatencyTimer([In] FT_HANDLE ftHandle, [Out] out byte pucTimer);

        /// <summary>
        /// Gets the instantaneous value of the data bus.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="pucMode">Pointer to unsigned char to store the instantaneous data bus value.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_GetBitMode([In] FT_HANDLE ftHandle, [Out] out FT_BITMODE pucMode);

        /// <summary>
        /// Enables different chip modes.
        /// </summary>
        /// <param name="ftHandle">Handle of the device.</param>
        /// <param name="ucMask">Required value for bit mode mask. This sets up which bits are inputs and outputs.
        /// A bit value of 0 sets the corresponding pin to an input, a bit value of 1 sets the corresponding pin to an output.
        /// In the case of CBUS Bit Bang, the upper nibble of this value controls which pins are inputs and outputs, while the lower nibble controls which of the outputs are high and low.
        /// </param>
        /// <param name="ucMode">Mode value.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_SetBitMode([In] FT_HANDLE ftHandle, [In] byte ucMask, [In] FT_BITMODE ucMode);

        /// <summary>
        /// This function builds a device information list and returns the number of D2XX devices connected to the system.
        /// The list contains information about both unopen and open devices.
        /// 
        /// An application can use this function to get the number of devices attached to the system.
        /// It can then allocate space for the device information list and retrieve the list using FT_GetDeviceInfoList or FT_GetDeviceInfoDetail.
        /// If the devices connected to the system change, the device info list will not be updated until FT_CreateDeviceInfoList is called again.
        /// </summary>
        /// <param name="lpdwNumDevs">Pointer to unsigned long to store the number of devices connected.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_CreateDeviceInfoList([Out] out int lpdwNumDevs);

        /// <summary>
        /// This function returns a device information list and the number of D2XX devices in the list.
        /// 
        /// This function should only be called after calling FT_CreateDeviceInfoList.
        /// If the devices connected to the system change, the device info list will not be updated until FT_CreateDeviceInfoList is called again.
        /// Location ID information is not returned for devices that are open when FT_CreateDeviceInfoList is called.
        /// Information is not available for devices which are open in other processes.
        /// In this case, the Flags parameter of the FT_DEVICE_LIST_INFO_NODE will indicate that the device is open, but other fields will be unpopulated.
        /// 
        /// The array of FT_DEVICE_LIST_INFO_NODES contains all available data on each device.
        /// </summary>
        /// <param name="pDest">Pointer to an array of FT_DEVICE_LIST_INFO_NODE structures.</param>
        /// <param name="lpdwNumDevs">Pointer to the number of elements in the array.</param>
        /// <returns>FT_OK if successful, otherwise the return value is an FT error code.</returns>
        [DllImport(Ftd2xxLibName)]
        public static extern FT_STATUS FT_GetDeviceInfoList([Out] FT_DEVICE_LIST_INFO_NODE[] pDest, [Out] out int lpdwNumDevs);
    }
}
