using System;
using System.Threading;

using FTDI.D2XX.Wrapper.FtdiInterop.Enums;
using FTDI.D2XX.Wrapper;
using FTDI.D2XX.Wrapper.FtdiInterop.Models;

namespace FTDI.D2XX.TestConsole
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Finding FTDI D2XX devices ...");
            FT_DEVICE_LIST_INFO_NODE[] devices = FtdiD2xxDevice.GetDevices();

            for(int i = 0; i < devices.Length; i++)
            {
                Console.WriteLine($"[{i}] Serial: {devices[i].SerialNumber}, Description: {devices[i].Description}, Location: {devices[i].LocId}");
            }

            if (devices.Length > 0)
            {
                Console.WriteLine("Starting bitbang test");
                DoBitbangTest(0);
                Console.WriteLine("Done.");
            }
            else
            {
                Console.WriteLine("No devices connected.");
            }
        }

        private static void DoBitbangTest(int deviceIndex)
        {
            int baud = 9600;

            // Set Asynchronous Bit Bang Mode.
            // Note: Using FT_BITMODE.FT_BITMODE_SYNC_BITBANG (0x04) will cause the chip to stop writing out data when the read buffer fills up.
            // Since we don't care about reading any data in this example, use async mode which can write forever and simply throws the read data away once the buffer has filled.
            FT_BITMODE bitmode = FT_BITMODE.FT_BITMODE_ASYNC_BITBANG;

            using (FtdiD2xxDevice device = new FtdiD2xxDevice())
            {
                byte[] sendBuffer = new byte[1];
                Console.WriteLine($"Opening device index {deviceIndex}");
                device.OpenByIndex(deviceIndex);
                //device.OpenBySerial(serial);
                //device.OpenByDescription(description);
                //device.OpenByLocation(location);
                Console.WriteLine("Reset");
                device.ResetDevice();

                Console.WriteLine("Purge RX/TX buffers");
                device.Purge(FT_PURGE.FT_PURGE_RX | FT_PURGE.FT_PURGE_TX);

                Console.WriteLine($"Setting baud {baud}");
                device.SetBaudRate(baud);

                
                Console.WriteLine($"Setting bitmode {bitmode}");
                device.SetBitMode(0xFF, bitmode);


                Console.WriteLine($"Doing demo ...");

                for (int loops = 0; loops < 8; loops++)
                {
                    byte currentFlags = 0x00;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            currentFlags ^= (byte)(1 << j);
                            sendBuffer.AsSpan().Fill(currentFlags);

                            int bytesWritten = device.Write(sendBuffer, sendBuffer.Length);

                            Thread.Sleep(50);
                        }
                    }
                }

                Console.WriteLine($"Demo complete.");


                Console.WriteLine("Closing device");
                device.Close();
            }
        }
    }
}