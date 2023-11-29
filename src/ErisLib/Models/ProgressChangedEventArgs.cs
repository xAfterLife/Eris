namespace ErisLib.Models;

/// <summary>
///     Provides data for the <see cref="DownloadService.ProgressChanged" /> event.
/// </summary>
public class ProgressChangedEventArgs : EventArgs
{
    public readonly int CurrentFile;
    public readonly long CurrentSize;
    public readonly string DownloadSpeed;
    public readonly string FileName;
    public readonly long FileSize;
    public readonly int MaxFile;

    /// <summary>
    ///     CTOR of <see cref="ProgressChangedEventArgs" /> class.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="progressMin"></param>
    /// <param name="progressMax"></param>
    /// <param name="downloadSpeed"></param>
    public ProgressChangedEventArgs(string? fileName, long? progressMin, long? progressMax, string? downloadSpeed, int currentFile, int maxFile)
    {
        FileName = fileName ?? string.Empty;
        CurrentSize = progressMin ?? 0;
        FileSize = progressMax ?? 0;
        DownloadSpeed = downloadSpeed ?? string.Empty;
        CurrentFile = currentFile;
        MaxFile = maxFile;
    }
}