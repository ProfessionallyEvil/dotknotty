using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

[Serializable]
public class VulnerableClass
{
    public string Command { get; set; }

    // This method is called during deserialization
    public string ExecuteCommand()
    {
        if (!string.IsNullOrEmpty(Command))
        {
            try
            {
                // Determine the OS platform
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Execute command in Windows
                    return ExecuteWindowsCommand(Command);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    // Execute command in Linux/OSX
                    return ExecuteUnixCommand(Command);
                }
                else
                {
                    return "Unsupported operating system.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Command execution failed: {ex.Message}");
                return $"Error executing command: {ex.Message}";
            }
        }
        return "No command executed.";
    }

    private string ExecuteWindowsCommand(string command)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(psi))
        {
            using (var reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine($"Executed command output (Windows): {result}");
                return result;
            }
        }
    }

    private string ExecuteUnixCommand(string command)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "/bin/sh",
            Arguments = $"-c \"{command}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(psi))
        {
            using (var reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine($"Executed command output (Unix): {result}");
                return result;
            }
        }
    }
}
