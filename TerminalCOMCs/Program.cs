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
        private static int PortParity = -1;

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
                    case 2:
                        {
                            TextColor.DarkCyanColor("Выбран контроль четности битов ", false);
                            TextColor.WhiteColor("" + COM.GetParityName(COM.GetCOMportParity()), false);
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
            //1. GetNames
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
            while (!COM.InitCOMport())
            {
                TextColor.ErrorColor("Ошибка инициализации, повторная попытка", true);
                Thread.Sleep(1000);
            }
            GetStateConfigurate(0);
            TextColor.GreenColor("Порт инициализирован", true);
            //2. SetBaudrate
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
            TextColor.DarkCyanColor("Включить режим расширенной настройки (Y/any key): ", false);
            int AdvanSetting = Console.Read();
            if (AdvanSetting == 89 || AdvanSetting == 121)
            {
                //3. SetParity
                TextColor.GreenColor("\nУстановите контроль четности битов (если не знаете что это, выберите 1).", true);
                for (int i = 1; i < 5; i++)
                {
                    Console.WriteLine(i + ".  =>  " + COM.GetParityName(i));
                }
                TextColor.GrayColor("Введите номер нужного вам контроля четности битов (от 1 до 5): ", false);
                do
                {
                    try
                    {
                        PortParity = Convert.ToInt32(Console.ReadLine());
                        if (PortParity < 1)
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
                } while (PortParity < 1);
                COM.SetPortParity(PortParity);
            }
            




            GetStateConfigurate(2);
            Console.WriteLine("Открытие соединения...");
            while (!COM.OpenCOMport())
            {
                TextColor.ErrorColor("Ошибка соединения, повторная попытка", true);
                Thread.Sleep(1000);
            }
            GetStateConfigurate(2);
            ReadThread = new Thread(ReadCOMport);
            ReadThread.Start();
            while (true)
            {
                COM.WriteCOMport(Console.ReadLine(), true);
            }
        }
    }
}
