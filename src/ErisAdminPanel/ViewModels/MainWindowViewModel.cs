using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ErisAdminPanel.Views.Pages;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace ErisAdminPanel.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
	private readonly IServiceProvider _serviceProvider;

	[ObservableProperty]
	private string _applicationTitle = string.Empty;

	private bool _isInitialized;

	[ObservableProperty]
	private ObservableCollection<INavigationControl> _navigationFooter = new();

	[ObservableProperty]
	private ObservableCollection<INavigationControl> _navigationItems = new();

	[ObservableProperty]
	private ObservableCollection<MenuItem> _trayMenuItems = new();

	public MainWindowViewModel(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		if ( !_isInitialized )
			InitializeViewModel();
	}

	[RelayCommand]
	public static void CloseProgram()
	{
		Environment.Exit(0);
	}

	private void InitializeViewModel()
	{
		ApplicationTitle = "WPF UI - ErisPatchNotes";

		NavigationItems = new ObservableCollection<INavigationControl> { new NavigationItem { Content = "Home", PageTag = "dashboard", Icon = SymbolRegular.Home24, PageType = typeof(DashboardPage) } };

		NavigationFooter = new ObservableCollection<INavigationControl>
		{
			new NavigationItem
			{
				Content = "Exit",
				PageTag = "exit",
				Icon = SymbolRegular.Door28,
				PageType = null,
				Command = CloseProgramCommand
			}
		};

		TrayMenuItems = new ObservableCollection<MenuItem> { new() { Header = "Home", Tag = "tray_home" } };

		_isInitialized = true;
	}
}
