namespace RazorReaper.Services;

/// <summary>
/// Service for file system operations with abstraction for testability.
/// </summary>
public interface IFileSystemService
{
    /// <summary>
    /// Reads all text from a file.
    /// </summary>
    /// <param name="path">The file path to read.</param>
    /// <returns>The file contents as a string.</returns>
    Task<string> ReadAllTextAsync(string path);

    /// <summary>
    /// Writes text to a file, creating or overwriting it.
    /// </summary>
    /// <param name="path">The file path to write.</param>
    /// <param name="content">The content to write.</param>
    Task WriteAllTextAsync(string path, string content);

    /// <summary>
    /// Checks if a file exists.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    bool FileExists(string path);

    /// <summary>
    /// Checks if a directory exists.
    /// </summary>
    /// <param name="path">The directory path to check.</param>
    /// <returns>True if the directory exists; otherwise, false.</returns>
    bool DirectoryExists(string path);

    /// <summary>
    /// Opens a file using the default application.
    /// </summary>
    /// <param name="path">The file path to open.</param>
    void OpenFile(string path);

    /// <summary>
    /// Gets the directory path from a full file path.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>The directory path.</returns>
    string? GetDirectoryName(string path);

    /// <summary>
    /// Combines path segments into a full path.
    /// </summary>
    /// <param name="paths">The path segments to combine.</param>
    /// <returns>The combined path.</returns>
    string Combine(params string[] paths);
}
