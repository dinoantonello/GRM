using System;
using System.Collections.Generic;

namespace GRM.ApplicationInfrastructure
{
    public class ConsoleWrite
    {
        public static void Success(String message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Error(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception);
            Console.ResetColor();
            #if DEBUG
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            #endif
        }

        public static void Error(String message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            #if DEBUG
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            #endif
        }

        public static void Error(List<string> messages)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
            
            Console.ResetColor();
#if DEBUG
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
#endif
        }

        public static void Info(String message)
        {
            Console.WriteLine(message);
        }
    }
}