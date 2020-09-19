using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Unity_Server.Logging
{
    public static class Logger
    {
        public enum Type { Success, Error, Warn, Important }
        public static void log1(string msg, params object[] args)
        {
            Console.WriteLine(msg, args);
        }
        public static void log2(Type typ, string msg, params object[] args)
        {
            var color = ConsoleColor.White;
            switch (typ)
            {
                case Type.Success:
                    {
                        color = ConsoleColor.Green;
                        break;
                    }
                case Type.Warn:
                    {
                        color = ConsoleColor.Yellow;
                        break;
                    }
                case Type.Important:
                    {
                        color = ConsoleColor.Blue;
                        break;
                    }
                case Type.Error:
                    {
                        color = ConsoleColor.Red;
                        break;
                    }

            }
            log3(color, msg, args);
        }

        public static void logException(Exception ex,[CallerFilePath] string filePath ="", [CallerLineNumber] int rowNumber = 0)
        {
            string filename = filePath.Length > 0 ? filePath.Split('/')[filePath.Split('/').Length-1] : "-";
            log2(Type.Error, $"{ex.GetType()} in {filename}: {rowNumber} -- {ex.Message}");
        }
        
        public static void log3(ConsoleColor fontColor, string msg, params object[] args)
        {
            ConsoleColor preColor = Console.ForegroundColor;
            Console.ForegroundColor = fontColor;
            log1(msg, args);
            Console.ForegroundColor = preColor;
        }
    }
}
