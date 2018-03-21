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
        public static Thread ReadThread;

        public static void ReadCOMport()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine(COM.ReadCOMport());
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e);
                }
            }
        }

        static void Main(string[] args)
        {
            int PortCount;
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
            int PortNum = -1;
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
            TextColor.DarkCyanColor("Выбран порт ", false);
            TextColor.WhiteColor(COM.GetPortsName(PortNum) + "\n", true);

            TextColor.GreenColor("Установите скорость приема/передачи COM порта в Бодах.", true);
            int PortBaudRate = -1;
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
            COM.SetPortName(PortNum);
            Console.Clear();
            TextColor.DarkCyanColor("Выбран порт ", false);
            TextColor.WhiteColor("" + COM.GetPortsName(PortNum), true);
            if (PortBaudRate < 10)
            {
                COM.SetPortBaud(COM.GetBaud(PortBaudRate));
                TextColor.DarkCyanColor("Выбрана скорость ", false);
                TextColor.WhiteColor("" + COM.GetBaud(PortBaudRate), false);
                TextColor.DarkCyanColor(" Бод", true);
            }
            else
            {
                COM.SetPortBaud(PortBaudRate);
                TextColor.DarkCyanColor("Выбрана скорость ", false);
                TextColor.WhiteColor("" + PortBaudRate, false);
                TextColor.DarkCyanColor(" Бод", true);
            }
            while (!COM.InitCOMport())
            {
                TextColor.ErrorColor("Ошибка инициализации, повторная попытка", true);
                Thread.Sleep(1000);
            }
            TextColor.GreenColor("Порт инициализирован", true);
            while (!COM.OpenCOMport())
            {
                TextColor.ErrorColor("Ошибка соединения, повторная попытка", true);
                Thread.Sleep(1000);
            }
            TextColor.GreenColor("Соединение открыто", true);
            if (COM.IsOpenCOMport())
            {
                TextColor.GreenColor("Соединение открыто", true);
            }
            else
            {
                TextColor.ErrorColor("ERROR", true);
            }

            ReadThread = new Thread(ReadCOMport);
            ReadThread.Start();
            while (true)
            {
                COM.WriteCOMport(Console.ReadLine(), true);
            }
        }
    }
}
