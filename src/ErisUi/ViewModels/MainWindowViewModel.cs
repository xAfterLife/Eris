using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ErisUi.Views.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace ErisUi.ViewModels;

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
    public static void OpenConfig()
    {
        var path = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory)!, "NtConfig.exe");
        if ( File.Exists(path) )
            Process.Start(path);
    }

    [RelayCommand]
    public static void CloseProgram()
    {
        Environment.Exit(0);
    }

    private void InitializeViewModel()
    {
        ApplicationTitle = _serviceProvider.GetRequiredService<IConfiguration>().GetValue<string>("LauncherTitle") ?? "Launcher-Title";

        NavigationItems = new ObservableCollection<INavigationControl> { new NavigationItem { Content = "Launcher", PageTag = "launcher", Icon = SymbolRegular.DataHistogram24, PageType = typeof(LauncherPage) }, new NavigationItem { Content = "Discord", PageTag = "discord", Image = new BitmapImage(new Uri("pack://application:,,,/Assets/discord24.png", UriKind.Absolute)), PageType = null } };

        NavigationFooter = new ObservableCollection<INavigationControl>
        {
            new NavigationItem { Content = "Settings", PageTag = "settings", Icon = SymbolRegular.Settings24, Command = OpenConfigCommand },
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