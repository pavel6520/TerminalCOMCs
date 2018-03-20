using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;

namespace TerminalCOMCs
{
    class COM
    {
        static string[] PortNames;
        static SerialPort COMport;
        static string COMName;
        static int COMBaud = 115200;
        static Parity COMParity = Parity.None;
        static int COMDataBits = 8;
        static StopBits COMStopBits = StopBits.One;
        static Handshake COMHandshake = Handshake.None;

        public static bool PortNamesUpdate()
        {
            try
            {
                PortNames = SerialPort.GetPortNames();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetPortNames");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        public static int GetPortsCount()
        {
            return PortNames.Length;
        }

        public static string GetPortName(int portNumber)
        {
            return PortNames[portNumber];
        }

        public static bool SetPortName(int portNumber)
        {
            try
            {
                COMName = PortNames[portNumber];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error SetPortName");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        public static int GetPortBaud(int PortBaud)
        {
            switch (PortBaud)
            {
                case 1: return 1200;
                case 2: return 2400;
                case 3: return 4800;
                case 4: return 9600;
                case 5: return 19200;
                case 6: return 38400;
                case 7: return 57600;
                case 8: return 115200;
                case 9: return 230400;
                default: return 0;
            }
        }

        public static bool SetPortBaud(int PortBaud)
        {
            try
            {
                COMBaud = PortBaud;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return false;
            }
            return true;
        }

        public static int SetPortParity(Parity PortParity)
        {
            try
            {
                COMParity = PortParity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return -7;
            }
            return 0;
        }

        public static int SetPortBits(int PortBits)
        {
            try
            {
                COMDataBits = PortBits;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return -6;
            }
            return 0;
        }

        public static int SetPortStopBits(StopBits PortStopBits)
        {
            try
            {
                COMStopBits = PortStopBits;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return -6;
            }
            return 0;
        }

        public static bool InitPort()
        {
            

            try
            {
                COMport = new SerialPort(COMName, COMBaud, COMParity, COMDataBits, COMStopBits)
                {
                    Handshake = COMHandshake,
                    ReadTimeout = 50,
                    WriteTimeout = 50
                };
                COMport.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error InitPort");
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return false;
            }
            return true;
        }
    }

}
