using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ErisLib;
using ErisLib.Models;
using ErisUi.Models;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Common.Interfaces;

namespace ErisUi.ViewModels;

public sealed partial class LauncherViewModel : ObservableObject, INavigationAware, IAsyncDisposable
{
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

	private DownloadService? _downloadService;
	private bool _isInitialized;

	[ObservableProperty]
	private bool _startButtonEnabled;

	public ObservableCollection<Patchnotes> Patchnotes { get; set; } = new() { new Patchnotes(new Uri("http://wiki.nostale.it/images/9/94/BannerEstivo.jpg"), new Uri("http://wiki.nostale.it/images/9/94/BannerEstivo.jpg"), "Test", "Test"), new Patchnotes(new Uri("https://abload.de/img/suchbanneru8kux.png"), new Uri("https://abload.de/img/suchbanneru8kux.png"), "Suchevent", "Nachdem nun die Kissen aufgeschüttelt und die Decken zurecht gelegt wurden, sitzt Ihr im Kreis und erzählt euch im schwachen Kerzenlicht gruselige Geschichten. Plötzlich erscheinen rot leuchtende Augen in der Dunkelheit.\r\n\r\n\r\n\r\nDie Kerzen erlischen, es wird Stockfinster und ihr spürt wie euch eure weichen Kissen und Decken aus den Händen gerissen werden. Mimi zündet in letzter Sekunde eine neue Kerze an, so dass ihr noch den ein oder anderen kurzen Blick auf einige Gestalten werfen könnt, die mit eurer Bettwäsche in alle Himmelsrichtungen davon rennen.\r\n\r\n\r\n\r\nIhr müsst versuchen die gestohlenen Dinge wieder zurück zu bekommen! Leider konntet Ihr die Monster, die mitsamt dem Diebesgut in Richtung ihrer Heimat geflohen sind, nicht mehr gut genug erkennen.\r\n\r\nZusammen erstellt ihr eine Liste mit den entsprechenden Beobachtungen die ihr noch machen konntet:"), new Patchnotes(new Uri("https://abload.de/img/suchbanneru8kux.png"), new Uri("https://abload.de/img/suchbanneru8kux.png"), "Suchevent", "Nachdem nun die Kissen aufgeschüttelt und die Decken zurecht gelegt wurden, sitzt Ihr im Kreis und erzählt euch im schwachen Kerzenlicht gruselige Geschichten. Plötzlich erscheinen rot leuchtende Augen in der Dunkelheit.\r\n\r\n\r\n\r\nDie Kerzen erlischen, es wird Stockfinster und ihr spürt wie euch eure weichen Kissen und Decken aus den Händen gerissen werden. Mimi zündet in letzter Sekunde eine neue Kerze an, so dass ihr noch den ein oder anderen kurzen Blick auf einige Gestalten werfen könnt, die mit eurer Bettwäsche in alle Himmelsrichtungen davon rennen.\r\n\r\n\r\n\r\nIhr müsst versuchen die gestohlenen Dinge wieder zurück zu bekommen! Leider konntet Ihr die Monster, die mitsamt dem Diebesgut in Richtung ihrer Heimat geflohen sind, nicht mehr gut genug erkennen.\r\n\r\nZusammen erstellt ihr eine Liste mit den entsprechenden Beobachtungen die ihr noch machen konntet:") };

	public LauncherViewModel(IServiceProvider serviceProvider)
	{
		if ( !_isInitialized )
			InitializeViewModel(serviceProvider);
	}

	public ValueTask DisposeAsync()
	{
		if ( _isInitialized && _downloadService != null )
			_downloadService.ProgressChanged -= _downloadService_ProgressChanged;
		return default;
	}

	public void OnNavigatedTo() {}

	public void OnNavigatedFrom() {}

	private void InitializeViewModel(IServiceProvider serviceProvider)
	{
		_downloadService = serviceProvider.GetRequiredService<DownloadService>();
		_downloadService.ProgressChanged += _downloadService_ProgressChanged;

		_isInitialized = true;
	}

	private void _downloadService_ProgressChanged(object? sender, ProgressChangedEventArgs e)
	{
		_absoluteProgressValue = e.CurrentFile;
		_absoluteProgressMax = e.MaxFile;
		_absoluteDownloadProgress = $"{e.FileName} ({e.CurrentFile}/{e.MaxFile} - {Math.Round(100d / e.MaxFile * e.CurrentFile, 2)}%)";

		_currentProgressValue = (int)(100 / e.FileSize * e.CurrentSize);
		_currentProgressMax = 100;
		_currentFileProgress = $"{e.FileSize}/{e.FileSize} {Math.Round(100d / e.FileSize * e.CurrentSize, 2)} - {e.DownloadSpeed}";
	}
}
