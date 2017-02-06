using System;

namespace DDoSDetector
{
    public class Logger
    {
        public void Log(string message)
        {
            Log(message, LogEntryType.Information);
        }

        public void Log(string message, LogEntryType entryType)
        {
            Log(message, entryType, null);
        }

        private void Log(string message, LogEntryType entryType, Guid? requestId)
        {
            switch (entryType)
            {
                case LogEntryType.Information:
                    Console.ForegroundColor=ConsoleColor.White;
                    Console.BackgroundColor=ConsoleColor.Black;
                    break;
                case LogEntryType.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogEntryType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogEntryType.Attack:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case LogEntryType.BlockedRequest:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogEntryType.ForwardedRequest:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogEntryType.CompletedRequest:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogEntryType.BrokenRequest:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entryType), entryType, null);
            }
            Console.WriteLine($"{DateTime.Now:u}\t{entryType}\t{requestId}\t{message}");
        }

        public void Log(string message, LogEntryType entryType, Guid requestId)
        {
            
        }
           
    }
}