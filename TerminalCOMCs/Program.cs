using System;
using System.IO.Ports;

namespace TerminalCOMCs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Получение информации об активных COM портах...");
            if (!COM.PortsNamesUpdate())
            {
                Console.WriteLine("Ошибка получения имен портов.");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            int PortCount;
            if ((PortCount = COM.GetPortsCount()) == 0)
            {
                Console.WriteLine("Активные порты не найдены.");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            for (int i = 0; i < PortCount; i++)
            {
                Console.WriteLine(i + ".  =>  " + COM.GetPortsName(i));
            }
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
