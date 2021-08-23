using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace WindowOpacity {
    public static class Util {
        public static Config GetConfig() {
            Console.WriteLine("Please enter process names, separated by commas.");
            var processesText = Console.ReadLine();
            while (string.IsNullOrEmpty(processesText)) {
                Console.WriteLine("Process names cannot be null. Try again.");
                processesText = Console.ReadLine();
            }

            Console.WriteLine("Please enter an opacity number.");
            var opacityText = Console.ReadLine();
            byte opacity;
            while (!byte.TryParse(opacityText, out opacity)) {
                Console.WriteLine("Opacity must be a number. Try again.");
                opacityText = Console.ReadLine();
            }

            return new Config { ProcessNames = processesText.Split(','), Opacity = opacity };
        }

        /// <summary>
        ///     Display current processes.
        /// </summary>
        public static void DisplayProcesses() {
            Console.WriteLine("===Process list===");
            foreach (var p in Process.GetProcesses()) {
                if (p.MainWindowHandle != IntPtr.Zero) {
                    Console.WriteLine($"    {p.ProcessName}");
                }
            }
        }
    }
}
