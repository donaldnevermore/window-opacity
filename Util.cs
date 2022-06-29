namespace WindowOpacity;

using System.Diagnostics;

public static class Util {
    /// <summary>
    /// Get config from input.
    /// </summary>
    /// <returns></returns>
    public static Config GetConfig() {
        Console.WriteLine("Please enter process names, separated by commas.");
        var processesInput = Console.ReadLine();
        while (string.IsNullOrEmpty(processesInput)) {
            Console.WriteLine("Process names cannot be null.");
            processesInput = Console.ReadLine();
        }

        var defaultOpacityText = "(40-255, default: 240)";
        Console.WriteLine($"Please enter an opacity number {defaultOpacityText}.");
        var opacityInput = Console.ReadLine();
        byte opacity = 240;
        if (!string.IsNullOrEmpty(opacityInput)) {
            while (!byte.TryParse(opacityInput, out opacity) || opacity < 40 || opacity > 255) {
                Console.WriteLine($"Opacity must be a valid number {defaultOpacityText}.");
                opacityInput = Console.ReadLine();
            }
        }

        return new Config { ProcessNames = processesInput.Split(','), Opacity = opacity };
    }

    /// <summary>
    /// Display current processes.
    /// </summary>
    public static void DisplayProcesses() {
        Console.WriteLine("=== Process list ===");
        foreach (var p in Process.GetProcesses()) {
            if (p.MainWindowHandle != IntPtr.Zero) {
                Console.WriteLine($"    {p.ProcessName}");
            }
        }
    }
}
