using System;

namespace FTDI.D2XX.Wrapper.FtdiInterop.Enums
{
    [Flags]
    public enum FT_FLAGS : uint
    {
        FT_FLAGS_OPENED = 1,
        FT_FLAGS_HISPEED = 2
    }

    //// Device information flags
    //enum {
    //    FT_FLAGS_OPENED = 1,
    //    FT_FLAGS_HISPEED = 2
    //};
}
