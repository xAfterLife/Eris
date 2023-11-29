using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ErisLib;
using ErisLib.Models;
using ErisUi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Common.Interfaces;

namespace ErisUi.ViewModels;

public sealed partial class LauncherViewModel : ObservableObject, INavigationAware, IAsyncDisposable
{
    [ObservableProperty]
    private Downloader _downloader = null!;

    private bool _isInitialized;

    [ObservableProperty]
    private bool _startButtonEnabled;

    public ObservableCollection<Patchnotes>? Patchnotes { get; set; } = new() { new Patchnotes(new Uri(@"https://dailynous.com/wp-content/uploads/2015/09/caution-banner.jpg"), new Uri(@"https://github.com/xAfterLife/Eris"), "404 - Patchnotes not found", "The PatchnotesUri is not set to an correct Value") };

    public LauncherViewModel(IServiceProvider serviceProvider)
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        if ( Uri.TryCreate(config.GetValue<string>("PatchnotesUri"), UriKind.Absolute, out var patchNotesUri) )
            _ = SetPatchnotes(patchNotesUri);

        if ( !_isInitialized )
            InitializeViewModel(serviceProvider);
    }

    public async ValueTask DisposeAsync()
    {
        if ( _isInitialized )
            await Downloader.DisposeAsync();
    }

    public void OnNavigatedTo() {}

    public void OnNavigatedFrom() {}

    private void InitializeViewModel(IServiceProvider serviceProvider)
    {
        Downloader = new Downloader(serviceProvider);
        _isInitialized = true;
    }

    private async Task SetPatchnotes(Uri uri)
    {
        Patchnotes = new ObservableCollection<Patchnotes>((await PatchnotesService.GetPatchNotes(uri))!);
    }
}