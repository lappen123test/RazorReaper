using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace RazorReaper.Services.Implementations;

/// <summary>
/// Implementation of IProcessService for process management.
/// </summary>
public class ProcessService : IProcessService
{
    private readonly ILogger<ProcessService> _logger;

    public ProcessService(ILogger<ProcessService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public Process[] GetProcessesByName(string processName)
    {
        try
        {
            _logger.LogDebug("Getting processes by name: {ProcessName}", processName);
            var processes = Process.GetProcessesByName(processName);
            _logger.LogDebug("Found {Count} process(es) with name: {ProcessName}", processes.Length, processName);
            return processes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting processes by name: {ProcessName}", processName);
            return Array.Empty<Process>();
        }
    }

    /// <inheritdoc/>
    public bool IsProcessRunning(string processName)
    {
        try
        {
            var processes = GetProcessesByName(processName);
            bool isRunning = processes.Length > 0;
            _logger.LogDebug("Process {ProcessName} running: {IsRunning}", processName, isRunning);
            return isRunning;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if process is running: {ProcessName}", processName);
            return false;
        }
    }

    /// <inheritdoc/>
    public Process Start(string filePath)
    {
        try
        {
            _logger.LogInformation("Starting process: {FilePath}", filePath);

            var startInfo = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true // Required for steam:// and other URI protocols
            };

            var process = Process.Start(startInfo);
            if (process == null)
            {
                _logger.LogWarning("Process.Start returned null for: {FilePath}", filePath);
                // For URI protocols (steam://), Process.Start may return null but still work
                // Return a dummy process to indicate success
                return Process.GetCurrentProcess();
            }
            return process;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting process: {FilePath}", filePath);
            throw;
        }
    }

    /// <inheritdoc/>
    public void Kill(Process process)
    {
        try
        {
            _logger.LogInformation("Killing process: {ProcessName} (ID: {ProcessId})",
                process.ProcessName, process.Id);
            process.Kill();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error killing process: {ProcessName} (ID: {ProcessId})",
                process.ProcessName, process.Id);
            throw;
        }
    }
}
