using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ErisLib;
using ErisLib.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ErisUi.Models;

public sealed partial class Downloader : ObservableObject, IAsyncDisposable
{
    private readonly DownloadService? _downloadService;

    [ObservableProperty]
    private string _absoluteDownloadProgress = "";

    [ObservableProperty]
    private int _absoluteProgressMax = 100;

    [ObservableProperty]
    private int _absoluteProgressValue;

    [ObservableProperty]
    private string _currentFileProgress = "";

    [ObservableProperty]
    private int _currentProgressMax = 100;

    [ObservableProperty]
    private int _currentProgressValue;

    public Downloader(IServiceProvider serviceProvider)
    {
        _downloadService = serviceProvider.GetRequiredService<DownloadService>();
        _downloadService.ProgressChanged += DownloadService_ProgressChanged;
    }

    public ValueTask DisposeAsync()
    {
        if ( _downloadService != null )
            _downloadService.ProgressChanged -= DownloadService_ProgressChanged;
        return ValueTask.CompletedTask;
    }

    private void DownloadService_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        AbsoluteProgressValue = e.CurrentFile;
        AbsoluteProgressMax = e.MaxFile;
        AbsoluteDownloadProgress = $"{e.FileName} ({e.CurrentFile}/{e.MaxFile} - {Math.Round(100d / e.MaxFile * e.CurrentFile, 2)}%)";

        CurrentProgressValue = (int)(100 / e.FileSize * e.CurrentSize);
        CurrentProgressMax = 100;
        CurrentFileProgress = $"{e.FileSize}/{e.FileSize} {Math.Round(100d / e.FileSize * e.CurrentSize, 2)} - {e.DownloadSpeed}";
    }
}