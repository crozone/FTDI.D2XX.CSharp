namespace FTDI.D2XX.Wrapper.FtdiInterop.Enums
{
    /// <summary>
    /// Device status
    /// </summary>
    /// <seealso cref="ftd2xx.h">
    /// See: ftd2xx.h, the ftd2xx library header.
    /// </seealso>
    public enum FT_STATUS : ulong // FT_STATUS is type ULONG, aka "unsigned long int", which is "at least" 32 bits in C, but can be 64 bits on 64 bit machines. Use ulong to capture all possible values.
    {
        FT_OK = 0,
        FT_INVALID_HANDLE,
        FT_DEVICE_NOT_FOUND,
        FT_DEVICE_NOT_OPENED,
        FT_IO_ERROR,
        FT_INSUFFICIENT_RESOURCES,
        FT_INVALID_PARAMETER,
        FT_INVALID_BAUD_RATE,

        FT_DEVICE_NOT_OPENED_FOR_ERASE,
        FT_DEVICE_NOT_OPENED_FOR_WRITE,
        FT_FAILED_TO_WRITE_DEVICE,
        FT_EEPROM_READ_FAILED,
        FT_EEPROM_WRITE_FAILED,
        FT_EEPROM_ERASE_FAILED,
        FT_EEPROM_NOT_PRESENT,
        FT_EEPROM_NOT_PROGRAMMED,
        FT_INVALID_ARGS,
        FT_NOT_SUPPORTED,
        FT_OTHER_ERROR,
        FT_DEVICE_LIST_NOT_READY,
    };

    //typedef PVOID   FT_HANDLE;
    //typedef ULONG   FT_STATUS;

    ////
    //// Device status
    ////
    //enum {
    //    FT_OK,
    //    FT_INVALID_HANDLE,
    //    FT_DEVICE_NOT_FOUND,
    //    FT_DEVICE_NOT_OPENED,
    //    FT_IO_ERROR,
    //    FT_INSUFFICIENT_RESOURCES,
    //    FT_INVALID_PARAMETER,
    //    FT_INVALID_BAUD_RATE,

    //    FT_DEVICE_NOT_OPENED_FOR_ERASE,
    //    FT_DEVICE_NOT_OPENED_FOR_WRITE,
    //    FT_FAILED_TO_WRITE_DEVICE,
    //    FT_EEPROM_READ_FAILED,
    //    FT_EEPROM_WRITE_FAILED,
    //    FT_EEPROM_ERASE_FAILED,
    //    FT_EEPROM_NOT_PRESENT,
    //    FT_EEPROM_NOT_PROGRAMMED,
    //    FT_INVALID_ARGS,
    //    FT_NOT_SUPPORTED,
    //    FT_OTHER_ERROR,
    //    FT_DEVICE_LIST_NOT_READY,
    //};
}
