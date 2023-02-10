using System;

namespace FTDI.D2XX.Wrapper.FtdiInterop.Enums
{
    [Flags]
    public enum FT_OPEN_BY
    {
        FT_OPEN_BY_SERIAL_NUMBER = 1,
        FT_OPEN_BY_DESCRIPTION = 2,
        FT_OPEN_BY_LOCATION = 4
    }
}
