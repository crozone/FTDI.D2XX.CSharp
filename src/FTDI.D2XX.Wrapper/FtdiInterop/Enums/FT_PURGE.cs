using System;

namespace FTDI.D2XX.Wrapper.FtdiInterop.Enums
{
    [Flags]
    public enum FT_PURGE : byte
    {
        FT_PURGE_RX = 1,
        FT_PURGE_TX = 2
    }
}
