using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text.Json;
using WindowOpacity;


#region Main logic

DisplayProcesses();
var config = GetConfig();
if (config.ProcessNames is not null) {
    var hwndList = FindProcesses(config.ProcessNames);
    SetWindowsOpacity(hwndList, config.Opacity);
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

#endregion

const int GwlExstyle = -20;
const int WsExLayered = 0x80000;
const int LwaAlpha = 2;

/// Display running processes
static void DisplayProcesses() {
    Console.WriteLine("===Process names begin===");
    foreach (var p in Process.GetProcesses()) {
        if (p.MainWindowHandle != IntPtr.Zero) {
            Console.WriteLine($"  {p.ProcessName}");
        }
    }

    Console.WriteLine("===Process names end===");
}

/// Find processes matching given names
static Process[] FindProcesses(string[] processNames) {
    var processes = Process.GetProcesses().Where(p => {
        if (p.MainWindowHandle == IntPtr.Zero) {
            return false;
        }

        foreach (var name in processNames) {
            if (Regex.IsMatch(p.ProcessName, name)) {
                return true;
            }
        }

        return false;
    }).ToArray();

    return processes;
}

/// hwnd is window handle
void SetWindowsOpacity(Process[] windowHandleList, byte opacity) {
    if (windowHandleList.Length == 0) {
        Console.WriteLine("No processes were found.");
        return;
    }

    if (opacity < 40 || opacity > 255) {
        Console.WriteLine("Opacity should be 40 through 255");
        return;
    }

    foreach (var process in windowHandleList) {
        var hwnd = process.MainWindowHandle;
        var windowLong = GetWindowLong(hwnd, GwlExstyle);
        _ = SetWindowLong(hwnd, GwlExstyle, windowLong | WsExLayered);

        var ok = SetLayeredWindowAttributes(hwnd, 0, opacity, LwaAlpha);
        if (ok) {
            Console.WriteLine($"SetLayeredWindowAttributes {process.ProcessName} ok.");
        }
        else {
            Console.WriteLine($"SetLayeredWindowAttributes {process.ProcessName} failed.");
        }
    }
}

static Config GetConfig() {
    var json = File.ReadAllText(AppContext.BaseDirectory + "config.json");
    var config = JsonSerializer.Deserialize<Config>(json);
    if (config is null) {
        throw new Exception("Config cannot be null. Please check config.json file.");
    }

    return config;
}

[DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
static extern int GetWindowLong(IntPtr hwnd, int nIndex);

[DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

[DllImport("user32.dll")]
static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
