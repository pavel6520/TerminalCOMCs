using System;
using System.Threading;

namespace TerminalCOMCs
{
    class TextColor
    {
        public static void ErrorColor(string outString, bool NewLine)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(outString);
            if (NewLine)
            {
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        public static void WhiteColor(string outString, bool NewLine)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(outString);
            if (NewLine)
            {
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        public static void GreenColor(string outString, bool NewLine)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(outString);
            if (NewLine)
            {
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        public static void GreenColor(char outString, bool NewLine)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(outString);
            if (NewLine)
            {
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        public static void DarkCyanColor(string outString, bool NewLine)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(outString);
            if (NewLine)
            {
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        public static void GrayColor(string outString, bool NewLine)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(outString);
            if (NewLine)
            {
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }

    class Program
    {
        private static Thread ReadThread;
        private static bool ReadType;
        private static bool WriteType;
        private static int PortCount;
        private static int PortNum = -1;
        private static int PortBaudRate = -1;
        private static int PortParity = -1;
        private static int PortDataBits = 8;
        private static int PortStopBits = 2;
        private static int PortHandshake = 1;

        public static void ReadCOMport()
        {
            while (true)
            {
                try
                {
                    if (!ReadType)
                    {
                        int ReadBuf = COM.ReadCOMport();
                        if (ReadBuf >= 0)
                        {
                            TextColor.GreenColor(Convert.ToChar(ReadBuf), false);
                        }
                    }
                    else
                    {
                        int ReadBuf = COM.ReadCOMport();
                        if (ReadBuf >= 0)
                        {
                            TextColor.GreenColor(ReadBuf + "", true);
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    TextColor.ErrorColor("\n" + e, true);
                    Thread.Sleep(1000);
                }
            }
        }

        public static bool GetStateConfigurate(int Stage)
        {
            Console.Clear();
            if (Stage == 0)
            {
                while (!COM.InitCOMport())
                {
                    TextColor.ErrorColor("Ошибка инициализации, повторная попытка", true);
                    Thread.Sleep(1000);
                }
                TextColor.GreenColor("Порт инициализирован", true);
            }
            if (!COM.SetConfCOMport())
            {
                TextColor.ErrorColor("Ошибка установки настроек", true);
                Console.ReadKey();
            }
            else
            {
                TextColor.GreenColor("Настройки установлены", true);
            }
            for (int i = 0; i <= Stage; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            TextColor.DarkCyanColor("Выбран порт ", false);
                            TextColor.WhiteColor(COM.GetCOMportName(), true);
                            break;
                        }
                    case 1:
                        {
                            TextColor.DarkCyanColor("Выбрана скорость ", false);
                            TextColor.WhiteColor("" + COM.GetCOMportBaud(), false);
                            TextColor.DarkCyanColor(" Бод", true);
                            break;
                        }
                    case 2:
                        {
                            TextColor.DarkCyanColor("Выбран контроль четности битов ", false);
                            TextColor.WhiteColor(COM.GetParityName(COM.GetCOMportParity()), true);
                            break;
                        }
                    case 3:
                        {
                            TextColor.DarkCyanColor("Кол-во битов данных в байте ", false);
                            TextColor.WhiteColor("" + COM.GetCOMportDataBits(), true);
                            break;
                        }
                    case 4:
                        {
                            TextColor.DarkCyanColor("Кол-во стопБитов ", false);
                            TextColor.WhiteColor(COM.GetStopBitsName(COM.GetCOMportStopBits()), true);
                            break;
                        }
                    case 5:
                        {
                            TextColor.DarkCyanColor("Выбран протокол управления ", false);
                            TextColor.WhiteColor(COM.GetHandshakeName(COM.GetCOMportHandshake()), true);
                            break;
                        }
                    case 6:
                        {
                            if (WriteType)
                            {
                                TextColor.DarkCyanColor("Тип отправляемых данных: ", false);
                                TextColor.WhiteColor("Число (от 0 до 127)", true);
                            }
                            else
                            {
                                TextColor.DarkCyanColor("Тип отправляемых данных: ", false);
                                TextColor.WhiteColor("Строка", true);
                            }
                            if (ReadType)
                            {
                                TextColor.DarkCyanColor("Тип принимаемых данных: ", false);
                                TextColor.WhiteColor("Число (от 0 до 127)", true);
                            }
                            else
                            {
                                TextColor.DarkCyanColor("Тип принимаемых данных: ", false);
                                TextColor.WhiteColor("Строка", true);
                            }
                            break;
                        }
                        
                }
            }
            if (COM.IsOpenCOMport())
            {
                TextColor.GreenColor("Соединение открыто", true);
                return true;
            }
            else
            {
                TextColor.ErrorColor("Соединение закрыто", true);
                return false;
            }
        }

        static void Main(string[] args)
        {
            {
                //1(0) GetNames
                do
                {
                    TextColor.GreenColor("Получение информации об активных COM портах...", true);
                    if (!COM.PortsNamesUpdate())
                    {
                        TextColor.ErrorColor("Ошибка получения имен портов.", true);
                        Console.WriteLine("Нажмите любую клавишу для повторной попытки...\n");
                        Console.ReadKey();
                    }
                    if ((PortCount = COM.GetPortsCount()) == 0)
                    {
                        TextColor.ErrorColor("Активные порты не найдены. Проверьте наличие активных портов в диспетчере устройств.", true);
                        Console.WriteLine("Нажмите любую клавишу для повторной проверки...\n");
                        Console.ReadKey();
                    }
                } while (PortCount == 0);
                for (int i = 0; i < PortCount; i++)
                {
                    Console.WriteLine(i + ".  =>  " + COM.GetPortsName(i));
                }
                TextColor.GrayColor("Введите номер нужного вам порта (от 0 до " + (PortCount - 1) + "): ", false);
                do
                {
                    try
                    {
                        PortNum = Convert.ToInt32(Console.ReadLine());
                        if (PortNum < 0 || PortNum > PortCount - 1)
                        {
                            TextColor.ErrorColor("Неверное число, повторите ввод (число от 0 до " + (PortCount - 1) + "): ", false);
                            continue;
                        }
                    }
                    catch (ArgumentException)
                    {
                        TextColor.ErrorColor("Неверный формат, введите число (от 0 до " + (PortCount - 1) + "): ", false);
                    }
                    catch (FormatException)
                    {
                        TextColor.ErrorColor("Неверный формат, введите число (от 0 до " + (PortCount - 1) + "): ", false);
                    }
                } while (PortNum < 0 || PortNum > PortCount - 1);
                COM.SetPortName(PortNum);
                GetStateConfigurate(0);


                //2(1) SetBaudRate
                TextColor.GreenColor("\nУстановите скорость приема/передачи COM порта в Бодах.", true);
                for (int i = 1; i <= 9; i++)
                {
                    Console.WriteLine(i + ".  =>  " + COM.GetBaud(i));
                }
                TextColor.GrayColor("Введите номер нужной вам скорости или введите вручную (10 и больше): ", false);
                do
                {
                    try
                    {
                        PortBaudRate = Convert.ToInt32(Console.ReadLine());
                        if (PortBaudRate < 1)
                        {
                            TextColor.ErrorColor("Неверное число, повторите ввод (число от 1 до 9) или введите вручную (10 и больше): ", false);
                        }
                    }
                    catch (ArgumentException)
                    {
                        TextColor.ErrorColor("Неверный формат, введите число: ", false);
                    }
                    catch (FormatException)
                    {
                        TextColor.ErrorColor("Неверный формат, введите число: ", false);
                    }
                } while (PortBaudRate < 1);
                if (PortBaudRate < 10)
                {
                    COM.SetPortBaud(COM.GetBaud(PortBaudRate));
                }
                else
                {
                    COM.SetPortBaud(PortBaudRate);
                }
                GetStateConfigurate(1);

                //3 Advanced settings
                TextColor.DarkCyanColor("Включить режим расширенной настройки (если пропустите, настройки будут заданы по умолчанию).\n[Y/y: Yes| AnyKey: No]: ", false);
                int AdvanSetting = Console.Read();
                Console.ReadLine();
                if (AdvanSetting == 89 || AdvanSetting == 121)
                {
                    GetStateConfigurate(1);
                    //3.1(2) SetParity
                    TextColor.GreenColor("\nВыберите контроль четности битов (если не знаете что это, выберите 1).", true);
                    for (int i = 1; i <= 5; i++)
                    {
                        Console.WriteLine(i + ".  =>  " + COM.GetParityName(i));
                    }
                    TextColor.GrayColor("Введите номер нужного вам контроля четности битов (от 1 до 5): ", false);
                    do
                    {
                        try
                        {
                            PortParity = Convert.ToInt32(Console.ReadLine());
                            if (PortParity < 1 || PortParity > 5)
                            {
                                TextColor.ErrorColor("Неверное число, повторите ввод (число от 1 до 5): ", false);
                            }
                        }
                        catch (ArgumentException)
                        {
                            TextColor.ErrorColor("Неверный формат, введите число (от 1 до 5): ", false);
                        }
                        catch (FormatException)
                        {
                            TextColor.ErrorColor("Неверный формат, введите число (от 1 до 5): ", false);
                        }
                    } while (PortParity < 1 || PortParity > 5);
                    COM.SetPortParity(PortParity);
                    GetStateConfigurate(2);

                    //3.2(3) SetDataBits
                    TextColor.GreenColor("\nУстановите чило битов данных в байте (если не знаете что это, введите 8).", true);
                    TextColor.GrayColor("Введите кол-во битов, рекомендуется 8 (от 5 до 8): ", false);
                    do
                    {
                        try
                        {
                            PortDataBits = Convert.ToInt32(Console.ReadLine());
                            if (PortDataBits < 5 || PortDataBits > 8)
                            {
                                TextColor.ErrorColor("Неверное число, повторите ввод (число от 5 до 8): ", false);
                            }
                        }
                        catch (ArgumentException)
                        {
                            TextColor.ErrorColor("Неверный формат, введите число (от 5 до 8): ", false);
                        }
                        catch (FormatException)
                        {
                            TextColor.ErrorColor("Неверный формат, введите число (от 5 до 8): ", false);
                        }
                    } while (PortDataBits < 5 || PortDataBits > 8);
                    COM.SetPortDataBits(PortDataBits);
                    GetStateConfigurate(3);

                    //3.3(4) SetStopBits
                    TextColor.GreenColor("\nУстановите число стопБитов (если не знаете что это, выберите 1).", true);
                    for (int i = 1; i <= 4; i++)
                    {
                        Console.WriteLine((i - 1) + ".  =>  " + COM.GetStopBitsName(i));
                    }
                    TextColor.GrayColor("Введите номер нужного вам кол-ва стопБитов (от 1 до 3): ", false);
                    do
                    {
                        try
                        {
                            PortStopBits = Convert.ToInt32(Console.ReadLine()) + 1;
                            if (PortStopBits < 2 || PortStopBits > 4)
                            {
                                TextColor.ErrorColor("Неверное число, повторите ввод (число от 1 до 3): ", false);
                            }
                        }
                        catch (ArgumentException)
                        {
                            TextColor.ErrorColor("Неверный формат, введите число (от 1 до 3): ", false);
                        }
                        catch (FormatException)
                        {
                            TextColor.ErrorColor("Неверный формат, введите число (от 1 до 3): ", false);
                        }
                    } while (PortStopBits < 2 || PortStopBits > 4);
                    COM.SetPortStopBits(PortStopBits);
                    GetStateConfigurate(4);

                    //3.4(5) SetHandshake
                    TextColor.GreenColor("\nУстановите протокол управления (если не знаете что это, выберите 1).", true);
                    for (int i = 1; i <= 4; i++)
                    {
                        Console.WriteLine((i) + ".  =>  " + COM.GetHandshakeName(i));
                    }
                    TextColor.GrayColor("Введите номер нужного вам протокола (от 1 до 4): ", false);
                    do
                    {
                        try
                        {
                            PortHandshake = Convert.ToInt32(Console.ReadLine());
                            if (PortHandshake < 1 || PortHandshake > 4)
                            {
                                TextColor.ErrorColor("Неверное число, повторите ввод (число от 1 до 4): ", false);
                            }
                        }
                        catch (ArgumentException)
                        {
                            TextColor.ErrorColor("Неверный формат, введите число (от 1 до 4): ", false);
                        }
                        catch (FormatException)
                        {
                            TextColor.ErrorColor("Неверный формат, введите число (от 1 до 4): ", false);
                        }
                    } while (PortHandshake < 1 || PortHandshake > 4);
                    COM.SetPortHandshake(PortHandshake);
                    GetStateConfigurate(5);
                }

                //4(6) Set ReadType/WriteType function
                TextColor.GreenColor("\nВыберите тип отправляемых данных [Any key - String; I/i - Int]: ", false);
                int WriteTypeBuf = Console.Read();
                Console.ReadLine();
                if (WriteTypeBuf == 'I' || WriteTypeBuf == 'i')
                {
                    TextColor.DarkCyanColor("Тип отправляемых данных: ", false);
                    TextColor.WhiteColor("Число (от 0 до 127)", true);
                    WriteType = true;
                }
                else
                {
                    TextColor.DarkCyanColor("Тип отправляемых данных: ", false);
                    TextColor.WhiteColor("Строка", true);
                    WriteType = false;
                }
                TextColor.GreenColor("\nВыберите тип принимаемых данных [Any key - String; I/i - Int]: ", false);
                int ReadTypeBuf = Console.Read();
                Console.ReadLine();
                if (ReadTypeBuf == 'I' || ReadTypeBuf == 'i')
                {
                    TextColor.DarkCyanColor("Тип принимаемых данных: ", false);
                    TextColor.WhiteColor("Число (от 0 до 127)", true);
                    ReadType = true;
                }
                else
                {
                    TextColor.DarkCyanColor("Тип принимаемых данных: ", false);
                    TextColor.WhiteColor("Строка", true);
                    ReadType = false;
                }

                //5 Connect
                Console.WriteLine("Открытие соединения...");
                while (!COM.OpenCOMport())
                {
                    TextColor.ErrorColor("Ошибка соединения, повторная попытка", true);
                    Thread.Sleep(1000);
                }
                GetStateConfigurate(6);
            }
            ReadThread = new Thread(ReadCOMport);
            ReadThread.Start();
            while (true)
            {
                if (!WriteType)
                {
                    COM.WriteCOMport(Console.ReadLine(), true);
                }
                else
                {
                    try
                    {
                        COM.WriteCOMport(Convert.ToInt32(Console.ReadLine()), false);
                    }
                    catch (FormatException)
                    {
                        TextColor.ErrorColor("Неверный формат, ожидалось число", true);
                    }
                    catch (OverflowException)
                    {
                        TextColor.ErrorColor("Неверный формат, ожидалось число", true);
                    }

                }
            }
        }
    }
}
