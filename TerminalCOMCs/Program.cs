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
        private static int PortCount;
        private static int PortNum = -1;
        private static int PortBaudRate = -1;

        public static void ReadCOMport()
        {
            while (true)
            {
                try
                {
                    int ReadBuf = COM.ReadCOMport();
                    if (ReadBuf >= 0)
                    {
                        TextColor.GreenColor(Convert.ToChar(ReadBuf), false);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e);
                }
            }
        }

        public static bool GetStateConfigurate(int Stage)
        {
            for (int i = 0; i <= Stage; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            TextColor.DarkCyanColor("Выбран порт ", false);
                            TextColor.WhiteColor("" + COM.GetCOMportName(), true);
                            break;
                        }
                    case 1:
                        {
                            
                            TextColor.DarkCyanColor("Выбрана скорость ", false);
                            TextColor.WhiteColor("" + COM.GetCOMportBaud(), false);
                            TextColor.DarkCyanColor(" Бод", true);
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
            do
            {
                TextColor.GreenColor("Получение информации об активных COM портах...", true);
                if (!COM.PortsNamesUpdate())
                {
                    TextColor.ErrorColor("Ошибка получения имен портов.", true);
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
                if ((PortCount = COM.GetPortsCount()) == 0)
                {
                    TextColor.ErrorColor("Активные порты не найдены. Проверьте наличие активных портов в диспетчере устройств.", true);
                    Console.WriteLine("Нажмите любую клавишу для продолжения...\n");
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
                    TextColor.ErrorColor("Неверный формат, введите число: ", false);
                }
                catch (FormatException)
                {
                    TextColor.ErrorColor("Неверный формат, введите число: ", false);
                }
            } while (PortNum < 0 || PortNum > PortCount - 1);
            COM.SetPortName(PortNum);
            Console.Clear();
            while (!COM.InitCOMport())
            {
                TextColor.ErrorColor("Ошибка инициализации, повторная попытка", true);
                Thread.Sleep(1000);
            }
            TextColor.GreenColor("Порт инициализирован", true);
            GetStateConfigurate(0);
            TextColor.GreenColor("/nУстановите скорость приема/передачи COM порта в Бодах.", true);
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
            Console.Clear();
            GetStateConfigurate(1);


            Console.Clear();
            if (!COM.SetConfCOMport())
            {
                TextColor.ErrorColor("Ошибка установки настроек", true);
                Console.ReadKey();
            }
            else
            {
                TextColor.GreenColor("Настройки установлены", true);
            }
            while (!COM.OpenCOMport())
            {
                TextColor.ErrorColor("Ошибка соединения, повторная попытка", true);
                Thread.Sleep(1000);
            }
            GetStateConfigurate(1);
            ReadThread = new Thread(ReadCOMport);
            ReadThread.Start();
            while (true)
            {
                COM.WriteCOMport(Console.ReadLine(), true);
            }
        }
    }
}
