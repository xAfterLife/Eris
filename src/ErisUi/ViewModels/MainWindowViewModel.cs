using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ErisUi.Views.Pages;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace ErisUi.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
	[ObservableProperty]
	private string _applicationTitle = string.Empty;

	private bool _isInitialized;

	[ObservableProperty]
	private ObservableCollection<INavigationControl> _navigationFooter = new();

	[ObservableProperty]
	private ObservableCollection<INavigationControl> _navigationItems = new();

	[ObservableProperty]
	private ObservableCollection<MenuItem> _trayMenuItems = new();

	public MainWindowViewModel(INavigationService navigationService)
	{
		if ( !_isInitialized )
			InitializeViewModel();
	}

	[RelayCommand]
	public static void OpenConfig()
	{
		var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "NtConfig.exe");
		if ( File.Exists(path) )
			Process.Start(path);
	}

	private void InitializeViewModel()
	{
		ApplicationTitle = "WPF UI - ErisUi";

		NavigationItems = new ObservableCollection<INavigationControl> { new NavigationItem { Content = "Launcher", PageTag = "launcher", Icon = SymbolRegular.DataHistogram24, PageType = typeof(LauncherPage) }, new NavigationItem { Content = "Launcher", PageTag = "launcher", Icon = SymbolRegular.WebAsset24, PageType = null } };

		NavigationFooter = new ObservableCollection<INavigationControl> { new NavigationItem { Content = "Settings", PageTag = "settings", Icon = SymbolRegular.Settings24, Command = OpenConfigCommand } };

		TrayMenuItems = new ObservableCollection<MenuItem> { new() { Header = "Home", Tag = "tray_home" } };

		_isInitialized = true;
	}
}
