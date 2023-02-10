# FTDI.D2XX.Wrapper

C# interop wrapper for the FTDI D2XX Direct Driver (ftd2xx.dll)

## Usage

The `FtdiD2xxDevice` class provides an object-oriented managed wrapper for the native FTDI Direct Driver interface.
The class abstracts the native device handle and implements `IDisposable` and destructor in order to make using the API safe.

```csharp
using FTDI.D2XX.Wrapper;

// Optionally get a list of all devices
// FT_DEVICE_LIST_INFO_NODE[] devices = FtdiD2xxDevice.GetDevices();

// Create the device
using (FtdiD2xxDevice device = new FtdiD2xxDevice())
{
    // Open the device
    device.OpenByIndex(0);
    //device.OpenBySerial(serial);
    //device.OpenByDescription(description);
    //device.OpenByLocation(location);

    // Use the device
    // ...

    // Close the device.
    // This is not strictly required, since disposing the device will also close it.
    device.Close();
}
```

The `FtFunctions` static class implements the actual `DllImports` that interop with the native ftd2xx library.

## Project Status

Most functions listed in the [FD2XX Programmer's Guide](https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&cad=rja&uact=8&ved=2ahUKEwiP1ZyC54n9AhXSR2wGHa3-DgcQFnoECBUQAQ&url=https%3A%2F%2Fwww.ftdichip.com%2FSupport%2FDocuments%2FProgramGuides%2FD2XX_Programmer%2527s_Guide(FT_000071).pdf) have been implemented.

However, some are lower priority and still TODOs.

## GPIO and Relay boards

Many USB to GPIO/Relay boards (eg the [SainSmart USB to Relay boards](https://www.umart.com.au/product/sainsmart-usb-eight-channel-relay-board-for-automation-12-v-60454)) are implemented using an FTDI FT245R class chip. The relays are wired to each of the 8 output pins on the IC via a high current driver.

By default, the FTDI FT245R devices behave like a standard USB to serial converter, and the output pins are set to the standard serial pins TX/RX/DTR/RTS, etc...

Treating it like a standard Virtual COM Port will therefore not work.

Instead, the device needs to be put into a bitbang bit mode, which causes all of the 8 output pins to latch to the value of the last byte received.
This mode can only be set by calling into the ftd2xx.dll, which in turn talks to the FTDI Direct Mode driver to send a SetBitMode request to the device.

On Windows, the Direct Driver is installed in parallel with the Virtual COM Port driver (VCP) by the CDM driver package.
Windows 10 usually finds this driver on Windows Update without any additional setup.

On Linux/MacOS, only one driver can be installed at a time. The VCP driver will need to be uninstalled and then the Direct Driver installed in its place.

### Example

```csharp
// Create the device
using (FtdiD2xxDevice device = new FtdiD2xxDevice())
{
    // Open the device by zero based index
    device.OpenByIndex(0);
    
    // It's also possible to open the device by serial string, description string, or location long

    //device.OpenBySerial(serial);
    //device.OpenByDescription(description);
    //device.OpenByLocation(location);

    // Reset the device so that it's in a known state
    device.ResetDevice();

    // Purge the RX and TX buffers
    device.Purge(FT_PURGE.FT_PURGE_RX | FT_PURGE.FT_PURGE_TX);

    // Set the baud rate
    device.SetBaudRate(9600);

    // Put the device into async bitbang mode.
    // This mode uses the chip output pins as a parallel interface.
    // Commonly, USB to GPIO/Relay breakout boards are implemented using a FT245R (or similar) chip that is wired in this way.
    device.SetBitMode(0xFF, FT_BITMODE.FT_BITMODE_ASYNC_BITBANG);

    // Write to device
    byte[] sendBuffer = new byte[1];
    for (int loops = 0; loops < 8; loops++)
    {
        byte currentFlags = 0x00;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                currentFlags ^= (byte)(1 << j);
                sendBuffer[0] = currentFlags;
                int bytesWritten = device.Write(sendBuffer, sendBuffer.Length);

                Thread.Sleep(50);
            }
        }
    }

    // Close the device.
    // This is not strictly required, since disposing the device will also close it.
    device.Close();
}
```

## Native Library

This project interops with the native FTDI D2XX shared library. Typically, this is ftd2xx.dll, however there are many platform specific versions.

See [FTDI D2XX Direct Driver Downlaods](https://ftdichip.com/drivers/d2xx-drivers/).

This project interacts with the native library through `DllImport`. Currently the library name is hardcoded to "ftd2xx.dll".

**The library does not include a version of ftd2xx.dll itself**, nor does it attempt to bind to a different library name depending on the OS or process architecture.

Including the correct "ftd2xx.dll" in the build output is left as an excercise for the library consumer. It should be copied into the build directory during the application build.

### TODO

It would be nice to improve the DLL loading experience. Source generators could accomplish this.

A dedicated static class could be generated for every OS/architecture combination, and mapped to a different library name. A proxy class would then decide which concrete implementation to call at runtime, based on the runtime OS platform and process architecture.

This is a low-priority TODO due to complexity.
