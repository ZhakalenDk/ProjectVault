using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oiski.School.ProjectVault.App
{
    public static class ConsoleHelper
    {
        public static string GetStringInput(string displayText, bool hideInput = false)
        {
            Console.ResetColor();
            Console.Write($"{displayText}: ");
            Console.ForegroundColor = ConsoleColor.Cyan;

            string value = ((hideInput) ? (string.Empty) : (Console.ReadLine()));

            while (hideInput)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }

                value += key.KeyChar;
            }

            Console.ResetColor();

            return value;
        }

        public static void BuildBaseLayout(string username = null)
        {
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine($"Welcome to Oiski's En-Decrypter ({username ?? string.Empty})\n");   //  Title
        }
    }
}
