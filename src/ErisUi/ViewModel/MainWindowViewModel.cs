using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErisLib;
using ErisLib.Enums;
using ErisLib.Structs;
using ErisUi.Core;

namespace ErisUi.ViewModel;

internal class MainWindowViewModel : ObservableObject
{
    private static CancellationTokenSource _tokenSource = null!;
    private static DownloadService _downloadService = null!;
    private static List<HttpFile> _downloadList = null!;

    private int _currentProgress;

    private string? _fileName;

    private int _maxProgress = 100;

    private string? _progressText;

    public string? ProgressText
    {
        get => _progressText;
        set => Update(ref _progressText, value);
    }

    public string? FileName
    {
        get => _fileName;
        set => Update(ref _fileName, value);
    }

    public int CurrentProgress
    {
        get => _currentProgress;
        set => Update(ref _currentProgress, value);
    }

    public int MaxProgress
    {
        get => _maxProgress;
        set => Update(ref _maxProgress, value);
    }

    public MainWindowViewModel()
    {
        _tokenSource = new CancellationTokenSource();
        _downloadService = new DownloadService(DownloadSpeedType.MBs);
        _downloadService.ProgressChanged += _downloadService_ProgressChanged;
    }

    private void _downloadService_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        FileName = e.FileName;
        MaxProgress = 100;
        CurrentProgress = (int)(100 / e.FileSize * e.CurrentSize);
        ProgressText = $"{e.CurrentSize}/{e.FileSize} {CurrentProgress}% - {e.DownloadSpeed}";
    }

    private async Task GetDownloadManifest(string uri)
    {
        _downloadList = (await _downloadService.GetHttpFile(uri, _tokenSource.Token))!;

        if ( _downloadList == null )
        {
            //TODO: Handle no files to download
        }
    }

    private async Task UpdateFiles(List<HttpFile> files)
    {
        //TODO: Implement Download-Folder
        await _downloadService.Download(files, "folder");
    }
}
