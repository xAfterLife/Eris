using System.Text.Json;
using ErisLib.Enums;
using ErisLib.Models;

namespace ErisLib;

public sealed class DownloadService
{
    private static string? _fileName;
    private static int _curFile;
    private static int _maxFile;
    private readonly HttpClient _client;

    /// <summary>
    ///     <see cref="DownloadSpeedType" />
    /// </summary>
    public DownloadSpeedType SpeedType { get; }

    /// <summary>
    ///     CTOR of the <see cref="DownloadService" /> Class
    /// </summary>
    /// <param name="speedType"></param>
    public DownloadService(DownloadSpeedType speedType = DownloadSpeedType.MBs)
    {
        SpeedType = speedType;
        _client = new HttpClient();
    }

    /// <summary>
    ///     Reports updates of the <see cref="Download" /> Function
    /// </summary>
    public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;

    /// <summary>
    ///     Download as <see cref="HttpFile" />
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<List<HttpFile>?> GetHttpFile(string url, CancellationToken cancellationToken)
    {
        if ( !Uri.TryCreate(url, UriKind.Absolute, out var uri) )
            return default;

        var response = await _client.GetAsync(uri, cancellationToken);
        if ( !response.IsSuccessStatusCode )
            return default;

        return await JsonSerializer.DeserializeAsync<List<HttpFile>>(await response.Content.ReadAsStreamAsync(cancellationToken), cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Downloads the files written in the List of <see cref="HttpFile" />
    /// </summary>
    /// <param name="files"></param>
    /// <param name="downloadFolder"></param>
    /// <returns></returns>
    public async Task Download(List<HttpFile> files, string downloadFolder)
    {
        _curFile = 0;
        _maxFile = files.Count;

        foreach ( var file in files )
        {
            _curFile++;
            _fileName = file.FileName;
            await using var fileStream = File.OpenWrite(Path.Combine(downloadFolder, file.FileName));
            await DownloadFile(file.Uri, fileStream);
        }
    }

    /// <summary>
    ///     Loads a file into the Stream with Reporting of Progress and Download-Speed
    /// </summary>
    /// <param name="url"></param>
    /// <param name="output"></param>
    /// <returns></returns>
    private async Task DownloadFile(string url, Stream output)
    {
        if ( !Uri.TryCreate(url, UriKind.Absolute, out var uri) )
            return;

        long bytesRecieved = 0;
        var buffer = new Memory<byte>(new byte[8192]);

        using var response = await _client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
        var totalBytes = response.Content.Headers.ContentLength;
        await using var stream = await response.Content.ReadAsStreamAsync();

        int bytesRead;
        var startTime = DateTime.Now;
        var lastDeltaTime = startTime;

        do
        {
            bytesRecieved += bytesRead = await stream.ReadAsync(buffer);
            var deltaTime = DateTime.Now;
            await output.WriteAsync(buffer[..bytesRead]);

            if ( deltaTime.Subtract(lastDeltaTime).TotalMilliseconds <= 250 )
                continue;

            lastDeltaTime = deltaTime;
            OnProgressChanged(new ProgressChangedEventArgs(_fileName, bytesRecieved, totalBytes, $"{GetDownloadSpeed(bytesRecieved, DateTime.Now.Subtract(startTime))} {Enum.GetName(typeof(DownloadSpeedType), SpeedType)}", _curFile, _maxFile));
        } while ( bytesRead != 0 || totalBytes > bytesRecieved );

        OnProgressChanged(new ProgressChangedEventArgs(_fileName, bytesRecieved, totalBytes, $"{GetDownloadSpeed(bytesRecieved, DateTime.Now.Subtract(startTime))} {Enum.GetName(typeof(DownloadSpeedType), SpeedType)}", _curFile, _maxFile));
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
    private float GetDownloadSpeed(long bytes, TimeSpan time)
    {
        if ( bytes < 0 || time.Milliseconds == 0 )
            return 0;

        return (float)Math.Round((decimal)(bytes / time.TotalMilliseconds * 1000 / Math.Pow(1024.0f, (double)SpeedType)), 2);
    }
}