﻿using System;
using System.IO.Ports;
using System.Threading;

namespace TerminalCOMCs
{
    class COM
    {
        private static string[] PortNames;
        private static SerialPort COMport;
        private static string COMName;
        private static int COMBaud = 9600;
        private static Parity COMParity = Parity.None;
        private static int COMDataBits = 8;
        private static StopBits COMStopBits = StopBits.One;
        private static Handshake COMHandshake = Handshake.None;
        public static int COMReadTimeout = 10;
        public static int COMWriteTimeout = 50;
        
        public bool PortsNamesUpdate()
        {
            //Writes COM port names in a PortNames
            try
            {
                PortNames = SerialPort.GetPortNames();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error GetPortNames");
                return false;
            }
            return true;
        }
        
        public int GetPortsCount()
        {
            //Return the count of COM ports
            return PortNames.Length;
        }

        public string GetPortsName(int NameNumber)
        {
            //Return the COM port name under the number NameNumber
            return PortNames[NameNumber];
        }
        
        public bool SetPortName(int NameNumber)
        {
            //Write in COMName a port name from the PortNames array
            try
            {
                COMName = PortNames[NameNumber];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error SetPortName");
                return false;
            }
            return true;
        }

        public bool InitCOMport()
        {
            //Initialize COM port
            try
            {
                COMport = new SerialPort(COMName);
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

        public string GetCOMportName()
        {
            //Get the name of COMport.
            return COMport.PortName;
        }

        public int GetBaud(int Baud)
        {
            //Return the possible values of speed in Baud
            switch (Baud)
            {//Standart speeds (used in HC-06 Bluetoots module)
                case 1: return 1200;
                case 2: return 2400;
                case 3: return 4800;
                case 4: return 9600; //Popular speed
                case 5: return 19200;
                case 6: return 38400;
                case 7: return 57600;
                case 8: return 115200;
                case 9: return 230400;
                default: return 0;
            }
        }
        
        public bool SetPortBaud(int Baud)
        {
            //Write in COMBaud a speed in Baud
            try
            {
                if (Baud > 0)
                {
                    COMBaud = Baud;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error SetPortBaud");
                return false;
            }
            return true;
        }

        public int GetCOMportBaud()
        {
            return COMport.BaudRate;
        }

        public string GetParityName(int NumParity)
        {
            //Return the name values of Parity Bit
            switch (NumParity)
            {
                case 1: return "Parity.None";
                case 2: return "Parity.Odd";
                case 3: return "Parity.Even";
                case 4: return "Parity.Mark";
                case 5: return "Parity.Space";
                default: return "";
            }
        }

        public bool SetPortParity(int NumParity)
        {
            //Write in COMParity a value of Parity
            try
            {
                switch (NumParity)
                {
                    case 1: COMParity = Parity.None; break;
                    case 2: COMParity = Parity.Odd; break;
                    case 3: COMParity = Parity.Even; break;
                    case 4: COMParity = Parity.Mark; break;
                    case 5: COMParity = Parity.Space; break;
                    default: return false;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("SetPortParity");
                return false;
            }
            return true;
        }

        public int GetCOMportParity()
        {
            switch (COMport.Parity)
            {
                case Parity.None: return 1;
                case Parity.Odd: return 2;
                case Parity.Even: return 3;
                case Parity.Mark: return 4;
                case Parity.Space: return 5;
                default: return 0;
            }
        }

        public bool SetPortDataBits(int DataBits)
        {
            //Write in COMDataBits a value of 5..8
            try
            {
                if(DataBits >= 5 && DataBits <= 8)
                {
                    COMDataBits = DataBits;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error SetPortDataBits");
                return false;
            }
            return true;
        }

        public int GetCOMportDataBits()
        {
            return COMport.DataBits;
        }

        public string GetStopBitsName(int StopBit)
        {
            //Return the name values of Stop Bits
            switch (StopBit)
            {
                case 1: return "StopBits.None (ERROR, not used)";
                case 2: return "StopBits.One";
                case 3: return "StopBits.OnePointFive";
                case 4: return "StopBits.Two";
                default: return "";
            }
        }

        public bool SetPortStopBits(int StopBit)
        {
            //Write in COMStopBits a value of StopBits
            try
            {
                switch (StopBit)
                {
                    //case 1: COMStopBits = StopBits.None; break;
                    case 2: COMStopBits = StopBits.One; break;
                    case 3: COMStopBits = StopBits.OnePointFive; break;
                    case 4: COMStopBits = StopBits.Two; break;
                    default: return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error SetPortStopBits");
                return false;
            }
            return true;
        }

        public int GetCOMportStopBits()
        {
            switch (COMport.StopBits)
            {
                case StopBits.None: return 1;
                case StopBits.One: return 2;
                case StopBits.OnePointFive: return 3;
                case StopBits.Two: return 4;
                default: return 0;
            }
        }

        public string GetHandshakeName(int NumHandshake)
        {
            //Return the possible values of Handshake (value in 1..4)
            switch (NumHandshake)
            {
                case 1: return "Handshake.None"; //Default
                case 2: return "Handshake.XOnXOff";
                case 3: return "Handshake.RequestToSend";
                case 4: return "Handshake.RequestToSendXOnXOff";
                default: return "";
            }
        }

        public bool SetPortHandshake(int NumHandshake)
        {
            //Return the possible values of Handshake (value in 1..4)
            switch (NumHandshake)
            {
                case 1: COMHandshake = Handshake.None; break;
                case 2: COMHandshake = Handshake.XOnXOff; break;
                case 3: COMHandshake = Handshake.RequestToSend; break;
                case 4: COMHandshake = Handshake.RequestToSendXOnXOff; break;
                default: return false;
            }
            return true;
        }

        public int GetCOMportHandshake()
        {
            switch (COMport.Handshake)
            {
                case Handshake.None: return 1;
                case Handshake.XOnXOff: return 2;
                case Handshake.RequestToSend: return 3;
                case Handshake.RequestToSendXOnXOff: return 4;
                default: return 0;
            }
        }

        public bool SetConfCOMport()
        {
            //Initialize COM port
            try
            {
                COMport.BaudRate = COMBaud;
                COMport.Parity = COMParity;
                COMport.DataBits = COMDataBits;
                COMport.StopBits = COMStopBits;
                COMport.Handshake = COMHandshake;
                COMport.ReadTimeout = COMReadTimeout;
                COMport.WriteTimeout = COMWriteTimeout;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error InitPort");
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool OpenCOMport()
        {
            //Opens the connection
            try
            {
                COMport.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error OpenPort");
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool IsOpenCOMport()
        {
            try
            {
                return COMport.IsOpen;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error IsOpenCOMport");
                return false;
            }
        }

        public int ReadCOMport()
        {
            try
            {
                if (COMport.IsOpen)
                {
                    return COMport.ReadChar();
                }
                else
                {
                    return -2;
                }
            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                Console.WriteLine("Error ReadCOMport");
                Console.WriteLine(e);
                Thread.Sleep(2000);
                return -2;
            }
            return -1;
        }

        public bool WriteCOMport(string WriteStr, bool NewLine)
        {
            try
            {
                COMport.Write(WriteStr);
                if (NewLine)
                {
                    COMport.WriteLine("");
                }
                return true;
            }
            catch (ArgumentNullException)
            {
                return true;
            }
            catch (TimeoutException)
            {
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error WriteCOMport string");
                Console.WriteLine(e);
                return false;
            }
        }

        public bool WriteCOMport(int WriteStr, bool NewLine)
        {
            try
            {
                COMport.Write(Convert.ToChar(WriteStr) + "");
                if (NewLine)
                {
                    COMport.WriteLine("");
                }
                return true;
            }
            catch (ArgumentNullException)
            {
                return true;
            }
            catch (TimeoutException)
            {
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error WriteCOMport char");
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
