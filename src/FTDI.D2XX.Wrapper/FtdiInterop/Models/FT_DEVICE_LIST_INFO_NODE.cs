using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using FTDI.D2XX.Wrapper.FtdiInterop.Enums;

namespace FTDI.D2XX.Wrapper.FtdiInterop.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FT_DEVICE_LIST_INFO_NODE
    {
        /// <summary>
        /// The flag value is a 4-byte bit map containing miscellaneous data as defined Appendix A – Type Definitions.
        /// Bit 0 (least significant bit) of this number indicates if the port is open (1) or closed (0).
        /// Bit 1 indicates if the device is enumerated as a high-speed USB device (2) or a full-speed USB device (0).
        /// The remaining bits (2 - 31) are reserved.
        /// </summary>
        public FT_FLAGS Flags;
        public int Type;
        public int ID;
        public int LocId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)] public string SerialNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string Description;
        public IntPtr ftHandle;
    }

    //FT_DEVICE_LIST_INFO_NODE(see FT_GetDeviceInfoList and FT_GetDeviceInfoDetail)
    //typedef struct _ft_device_list_info_node {
    //  DWORD Flags;
    //  DWORD Type;
    //  DWORD ID;
    //  DWORD LocId;
    //  char SerialNumber[16];
    //  char Description[64];
    //  FT_HANDLE ftHandle;
    //} FT_DEVICE_LIST_INFO_NODE;
}
