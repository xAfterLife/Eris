using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ErisUi.Models;
using Wpf.Ui.Common.Interfaces;

namespace ErisUi.ViewModels;

public sealed partial class LauncherViewModel : ObservableObject, INavigationAware, IAsyncDisposable
{
	[ObservableProperty]
	private Downloader _downloader = null!;

	private bool _isInitialized;

	[ObservableProperty]
	private bool _startButtonEnabled;

	public ObservableCollection<Patchnotes> Patchnotes { get; set; } = new() { new Patchnotes(new Uri("http://wiki.nostale.it/images/9/94/BannerEstivo.jpg"), new Uri("http://wiki.nostale.it/images/9/94/BannerEstivo.jpg"), "Test", "Test"), new Patchnotes(new Uri("https://abload.de/img/suchbanneru8kux.png"), new Uri("https://abload.de/img/suchbanneru8kux.png"), "Suchevent", "Nachdem nun die Kissen aufgeschüttelt und die Decken zurecht gelegt wurden, sitzt Ihr im Kreis und erzählt euch im schwachen Kerzenlicht gruselige Geschichten. Plötzlich erscheinen rot leuchtende Augen in der Dunkelheit.\r\n\r\n\r\n\r\nDie Kerzen erlischen, es wird Stockfinster und ihr spürt wie euch eure weichen Kissen und Decken aus den Händen gerissen werden. Mimi zündet in letzter Sekunde eine neue Kerze an, so dass ihr noch den ein oder anderen kurzen Blick auf einige Gestalten werfen könnt, die mit eurer Bettwäsche in alle Himmelsrichtungen davon rennen.\r\n\r\n\r\n\r\nIhr müsst versuchen die gestohlenen Dinge wieder zurück zu bekommen! Leider konntet Ihr die Monster, die mitsamt dem Diebesgut in Richtung ihrer Heimat geflohen sind, nicht mehr gut genug erkennen.\r\n\r\nZusammen erstellt ihr eine Liste mit den entsprechenden Beobachtungen die ihr noch machen konntet:"), new Patchnotes(new Uri("https://abload.de/img/suchbanneru8kux.png"), new Uri("https://abload.de/img/suchbanneru8kux.png"), "Suchevent", "Nachdem nun die Kissen aufgeschüttelt und die Decken zurecht gelegt wurden, sitzt Ihr im Kreis und erzählt euch im schwachen Kerzenlicht gruselige Geschichten. Plötzlich erscheinen rot leuchtende Augen in der Dunkelheit.\r\n\r\n\r\n\r\nDie Kerzen erlischen, es wird Stockfinster und ihr spürt wie euch eure weichen Kissen und Decken aus den Händen gerissen werden. Mimi zündet in letzter Sekunde eine neue Kerze an, so dass ihr noch den ein oder anderen kurzen Blick auf einige Gestalten werfen könnt, die mit eurer Bettwäsche in alle Himmelsrichtungen davon rennen.\r\n\r\n\r\n\r\nIhr müsst versuchen die gestohlenen Dinge wieder zurück zu bekommen! Leider konntet Ihr die Monster, die mitsamt dem Diebesgut in Richtung ihrer Heimat geflohen sind, nicht mehr gut genug erkennen.\r\n\r\nZusammen erstellt ihr eine Liste mit den entsprechenden Beobachtungen die ihr noch machen konntet:") };

	public LauncherViewModel(IServiceProvider serviceProvider)
	{
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
}
