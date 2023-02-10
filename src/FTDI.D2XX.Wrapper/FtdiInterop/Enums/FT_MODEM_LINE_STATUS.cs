namespace FTDI.D2XX.Wrapper.FtdiInterop.Enums
{
    public enum FT_MODEM_LINE_STATUS : uint
    {
        // Modem status, in least significant byte
        CTS = 0x10,
        DSR = 0x20,
        RI  = 0x40,
        DCD = 0x80,

        // Line status, in second least significant byte
        OE = 0x02 << 8,
        PE = 0x04 << 8,
        FE = 0x08 << 8,
        BI = 0x10 << 8
    }
}
