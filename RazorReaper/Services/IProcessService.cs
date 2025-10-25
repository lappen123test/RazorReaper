using System.Diagnostics;

namespace RazorReaper.Services;

/// <summary>
/// Service for process management operations.
/// </summary>
public interface IProcessService
{
    /// <summary>
    /// Gets all processes with the specified name.
    /// </summary>
    /// <param name="processName">The process name to search for (without .exe extension).</param>
    /// <returns>An array of Process objects matching the name.</returns>
    Process[] GetProcessesByName(string processName);

    /// <summary>
    /// Checks if a process with the specified name is currently running.
    /// </summary>
    /// <param name="processName">The process name to check (without .exe extension).</param>
    /// <returns>True if at least one process with that name is running; otherwise, false.</returns>
    bool IsProcessRunning(string processName);

    /// <summary>
    /// Starts a new process with the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the executable file.</param>
    /// <returns>The started Process object.</returns>
    Process Start(string filePath);

    /// <summary>
    /// Kills a process.
    /// </summary>
    /// <param name="process">The process to kill.</param>
    void Kill(Process process);
}
