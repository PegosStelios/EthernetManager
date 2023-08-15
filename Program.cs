using System;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;

namespace EthernetManager
{
    /// <summary>
    /// Provides functionality to enable, disable, or abort changes to the Ethernet adapter.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void Main(string[] args)
        {
            // Check if the application was restarted with elevated privileges.
            bool isElevatedInstance = args.Length > 0 && args[0] == "elevated";

            // If not an elevated instance and not running as administrator, restart with elevated privileges.
            if (!isElevatedInstance && !IsRunningAsAdministrator())
            {
                RestartWithElevatedPrivileges();
                return;
            }

            Console.WriteLine("Do you want to enable, disable the Ethernet adapter, or abort? (Enter 'e' for enable, 'd' for disable, or 'a' for abort)");
            var userInput = Console.ReadKey().KeyChar;

            Console.WriteLine(); // For a new line after user input

            switch (userInput)
            {
                case 'e':
                    AdjustEthernetAdapter(true);
                    break;
                case 'd':
                    AdjustEthernetAdapter(false);
                    break;
                case 'a':
                    Console.WriteLine("Operation aborted.");
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter 'e' for enable, 'd' for disable, or 'a' for abort.");
                    break;
            }
        }

        /// <summary>
        /// Checks if the current application instance is running with administrator rights.
        /// </summary>
        /// <returns>True if running as administrator, otherwise false.</returns>
        private static bool IsRunningAsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Restarts the current application instance with elevated (administrator) privileges.
        /// </summary>
        private static void RestartWithElevatedPrivileges()
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = System.Reflection.Assembly.GetExecutingAssembly().Location,
                Verb = "runas",
                Arguments = "elevated" // Pass an argument to indicate elevated instance
            };

            try
            {
                Process.Start(startInfo);
                Environment.Exit(0); // Close the current non-elevated instance
            }
            catch
            {
                Console.WriteLine("Failed to start process with administrator rights.");
            }
        }

        /// <summary>
        /// Adjusts the state of the Ethernet adapter based on the user's choice.
        /// </summary>
        /// <param name="enable">True to enable the adapter, false to disable it.</param>
        private static void AdjustEthernetAdapter(bool enable)
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId = 'Ethernet'");
            foreach (var item in searcher.Get())
            {
                var networkAdapter = (ManagementObject)item;
                var adapterName = networkAdapter["NetConnectionId"].ToString();

                if (adapterName == "Ethernet")
                {
                    if (!enable)
                    {
                        Console.WriteLine($"Disabling {adapterName}...");
                        networkAdapter.InvokeMethod("Disable", null);
                        Console.WriteLine($"{adapterName} has been disabled.");
                    }
                    else
                    {
                        Console.WriteLine($"Enabling {adapterName}...");
                        networkAdapter.InvokeMethod("Enable", null);
                        Console.WriteLine($"{adapterName} has been enabled.");
                    }
                }
            }
        }
    }
}
