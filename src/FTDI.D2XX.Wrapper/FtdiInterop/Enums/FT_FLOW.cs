namespace FTDI.D2XX.Wrapper.FtdiInterop.Enums
{
    public enum FT_FLOW : short
    {
        FT_FLOW_NONE = 0x0000,
        FT_FLOW_RTS_CTS = 0x0100,
        FT_FLOW_DTR_DSR = 0x0200,
        FT_FLOW_XON_XOFF = 0x0400
    }
}
