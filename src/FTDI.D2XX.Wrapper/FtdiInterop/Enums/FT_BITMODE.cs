namespace FTDI.D2XX.Wrapper.FtdiInterop.Enums
{
    public enum FT_BITMODE : byte
    {
        /// <summary>
        /// Reset
        /// </summary>
        FT_BITMODE_RESET = 0x00,

        /// <summary>
        /// Asynchronous Bit Bang
        /// </summary>
        FT_BITMODE_ASYNC_BITBANG = 0x01,

        /// <summary>
        /// MPSSE (FT2232, FT2232H, FT4232H and FT232H devices only)
        /// </summary>
        FT_BITMODE_MPSSE = 0x02,

        /// <summary>
        /// Synchronous Bit Bang (FT232R, FT245R, FT2232, FT2232H, FT4232H and FT232H devices only)
        /// </summary>
        FT_BITMODE_SYNC_BITBANG = 0x04,

        /// <summary>
        /// MCU Host Bus Emulation Mode (FT2232, FT2232H, FT4232H and FT232H devices only)
        /// </summary>
        FT_BITMODE_MCU_HOST = 0x08,

        /// <summary>
        /// Fast Opto-Isolated Serial Mode (FT2232, FT2232H, FT4232H and FT232H devices only)
        /// </summary>
        FT_BITMODE_FAST_SERIAL = 0x10,

        /// <summary>
        /// CBUS Bit Bang Mode (FT232R and FT232H devices only)
        /// </summary>
        FT_BITMODE_CBUS_BITBANG = 0x20,

        /// <summary>
        /// Single Channel Synchronous 245 FIFO Mode (FT2232H and FT232H devices only)
        /// </summary>
        FT_BITMODE_SYNC_FIFO = 0x40
    }

    //#define FT_BITMODE_RESET					0x00
    //#define FT_BITMODE_ASYNC_BITBANG			0x01
    //#define FT_BITMODE_MPSSE					0x02
    //#define FT_BITMODE_SYNC_BITBANG				0x04
    //#define FT_BITMODE_MCU_HOST					0x08
    //#define FT_BITMODE_FAST_SERIAL				0x10
    //#define FT_BITMODE_CBUS_BITBANG				0x20
    //#define FT_BITMODE_SYNC_FIFO				0x40
}
