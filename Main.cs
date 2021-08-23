using System;
using static WindowOpacity.Util;
using static WindowOpacity.Native;

DisplayProcesses();
var config = GetConfig();

// hwnd is window handle
var hwndList = FindProcesses(config.ProcessNames);
SetWindowsOpacity(hwndList, config.Opacity);

Console.WriteLine("Press any key to exit...");
Console.ReadKey(true);
