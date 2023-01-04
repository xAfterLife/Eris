using ErisLib.Enums;
using ErisLib.Structs;

namespace ErisLib;

/// <summary>
///     Provides a service for downloading one or more files from a remote location.
///     The progress and download speed of the file(s) can be monitored through events.
/// </summary>
internal sealed class DownloadService
{
    private const int MaxQueueSize = 100;
    private static string? _fileName;
    private readonly HttpClient _client;
    private readonly Queue<(DateTime, long)> _downloadQueue;
    private readonly IEnumerable<HttpFile> _files;
    public DownloadSpeedType SpeedType { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DownloadService" /> class.
    /// </summary>
    /// <param name="files">The collection of files to download.</param>
    /// <param name="speedType">The type of download speed to report.</param>
    public DownloadService(IEnumerable<HttpFile> files, DownloadSpeedType speedType)
    {
        _files = files;
        _downloadQueue = new Queue<(DateTime, long)>();
        SpeedType = speedType;
        _client = new HttpClient();
    }

    /// <summary>
    ///     Occurs when the progress of a download has changed.
    /// </summary>
    public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;

    /// <summary>
    ///     Begins the download of a collection of files.
    /// </summary>
    /// <param name="downloadDirectory">The directory to save the downloaded files to.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task StartDownload(string downloadDirectory)
    {
        foreach ( var file in _files )
        {
            _fileName = file.FileName;
            var stream = File.OpenWrite(Path.Combine(downloadDirectory, file.FileName));

            await Download(file.Uri, stream);
        }
    }

    /// <summary>
    ///     Downloads a file from the specified URL and saves it to the specified output stream.
    /// </summary>
    /// <param name="url">The URL of the file to download.</param>
    /// <param name="output">The stream to save the downloaded file to.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    private async Task Download(string url, Stream output)
    {
        long bytesRecieved = 0;
        var buffer = new byte[8192];

        if ( !Uri.IsWellFormedUriString(url, UriKind.Absolute) )
            return;

        using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        await using var stream = await response.Content.ReadAsStreamAsync();
        var totalBytes = response.Content.Headers.ContentLength;

        long bytesRead;

        do
        {
            bytesRecieved += bytesRead = await stream.ReadAsync(buffer);
            await output.WriteAsync(buffer);

            _downloadQueue.Enqueue((DateTime.Now, bytesRecieved));
            if ( _downloadQueue.Count < MaxQueueSize )
                _ = _downloadQueue.Dequeue();

            OnDownloadSpeedChanged(new DownloadSpeedChangedEventArgs($"{GetDownloadSpeed()} {Enum.GetName(typeof(DownloadSpeedType), SpeedType)}"));
            OnProgressChanged(new ProgressChangedEventArgs(bytesRecieved, totalBytes, (float)Math.Round((decimal)(bytesRead / (totalBytes ?? 0) * 100), 2)));
        } while ( bytesRead != 0 || totalBytes >= bytesRecieved );
    }

    /// <summary>
    ///     Raises the <see cref="ProgressChanged" /> event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    private void OnProgressChanged(ProgressChangedEventArgs e)
    {
        ProgressChanged?.Invoke(this, e);
    }

    /// <summary>
    ///     Calculates the download speed of a file.
    /// </summary>
    /// <returns>The download speed, in selected SpeedType per second.</returns>
    private float GetDownloadSpeed()
    {
        if ( _downloadQueue.Count < 2 )
            return 0f;

        var (firstDate, firstBytes) = _downloadQueue.LastOrDefault();
        var (lastDate, lastBytes) = _downloadQueue.FirstOrDefault();

        var bytes = lastBytes - firstBytes;
        var time = lastDate - firstDate;

        return (float)Math.Round((decimal)(bytes / time.Milliseconds * 1000 / Math.Pow(1024.0f, (double)SpeedType)), 2);
    }

    /// <summary>
    ///     Raises the <see cref="DownloadSpeedChanged" /> event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    internal void OnDownloadSpeedChanged(DownloadSpeedChangedEventArgs e)
    {
        DownloadSpeedChanged?.Invoke(this, e);
    }

    /// <summary>
    ///     Occurs when the download speed of a file has changed.
    /// </summary>
    public event EventHandler<DownloadSpeedChangedEventArgs>? DownloadSpeedChanged;

    /// <summary>
    ///     Provides data for the <see cref="DownloadService.ProgressChanged" /> event.
    /// </summary>
    public class ProgressChangedEventArgs : EventArgs
    {
        public readonly long CurrentSize;
        public readonly string FileName;
        public readonly long FileSize;
        public readonly float ProgressPercent;

        public ProgressChangedEventArgs(long? progressMin, long? progressMax, float? progressPercent)
        {
            FileName = _fileName ?? string.Empty;
            CurrentSize = progressMin ?? 0;
            FileSize = progressMax ?? 0;
            ProgressPercent = progressPercent ?? 0;
        }
    }

    /// <summary>
    ///     Provides data for the <see cref="DownloadService.DownloadSpeedChanged" /> event.
    /// </summary>
    public sealed class DownloadSpeedChangedEventArgs : EventArgs
    {
        public readonly string DownloadSpeed;

        public DownloadSpeedChangedEventArgs(string? downloadSpeed)
        {
            DownloadSpeed = downloadSpeed ?? "NaN";
        }
    }
}
