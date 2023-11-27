namespace WindowOpacity;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public static class Native {
    private const int GwlExStyle = -20;
    private const int WsExLayered = 0x80000;
    private const int LwaAlpha = 2;

    /// <summary>
    /// Find processes matching given names.
    /// </summary>
    /// <param name="processNames"></param>
    /// <returns></returns>
    public static Process[] FindProcesses(string[] processNames) {
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
        });

        return processes.ToArray();
    }

    /// <summary>
    /// Make window transparent.
    /// </summary>
    /// <param name="windowHandleList"></param>
    /// <param name="opacity"></param>
    public static void SetWindowsOpacity(Process[] windowHandleList, byte opacity) {
        if (windowHandleList.Length == 0) {
            Console.WriteLine("No processes found.");
            return;
        }

        if (opacity < 40 || opacity > 255) {
            Console.WriteLine("Opacity should be 40-255.");
            return;
        }

        foreach (var process in windowHandleList) {
            var hwnd = process.MainWindowHandle;
            var windowLong = GetWindowLong(hwnd, GwlExStyle);
            _ = SetWindowLong(hwnd, GwlExStyle, windowLong | WsExLayered);

            var ok = SetLayeredWindowAttributes(hwnd, 0, opacity, LwaAlpha);
            if (ok) {
                Console.WriteLine($"SetLayeredWindowAttributes {process.ProcessName} succeeded.");
            }
            else {
                Console.WriteLine($"SetLayeredWindowAttributes {process.ProcessName} failed.");
            }
        }
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
}
