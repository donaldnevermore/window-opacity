using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace WindowOpacity {
    public static class Util {
        public static Config GetConfig() {
            var path = AppContext.BaseDirectory + "config.json";
            var json = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<Config>(json);
            if (config is null) {
                throw new Exception("Config cannot be null. Please check config.json file.");
            }

            return config;
        }

        /// <summary>
        ///     Display running processes
        /// </summary>
        public static void DisplayProcesses() {
            Console.WriteLine("===Process names begin===");
            foreach (var p in Process.GetProcesses()) {
                if (p.MainWindowHandle != IntPtr.Zero) {
                    Console.WriteLine($"  {p.ProcessName}");
                }
            }

            Console.WriteLine("===Process names end===");
        }
    }
}
