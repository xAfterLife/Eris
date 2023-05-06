using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using ErisLib;
using ErisUi.Services;
using ErisUi.ViewModels;
using ErisUi.Views.Pages;
using ErisUi.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using static ErisUi.Properties.Resources;

namespace ErisUi;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
	// The.NET Generic Host provides dependency injection, configuration, logging, and other services.
	// https://docs.microsoft.com/dotnet/core/extensions/generic-host
	// https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
	// https://docs.microsoft.com/dotnet/core/extensions/configuration
	// https://docs.microsoft.com/dotnet/core/extensions/logging
	private static readonly IHost Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
												  .ConfigureAppConfiguration(c =>
												  {
													  c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location) ?? throw new InvalidOperationException());
													  c.AddJsonStream(new MemoryStream(appconfig));
												  })
												  .ConfigureServices((_, services) =>
												  {
													  // App Host
													  services.AddHostedService<ApplicationHostService>();

													  // Page resolver service
													  services.AddSingleton<IPageService, PageService>();

													  // Theme manipulation
													  services.AddSingleton<IThemeService, ThemeService>();

													  // TaskBar manipulation
													  services.AddSingleton<ITaskBarService, TaskBarService>();

													  // Service containing navigation, same as INavigationWindow... but without window
													  services.AddSingleton<INavigationService, NavigationService>();

													  // Main window with navigation
													  services.AddScoped<INavigationWindow, MainWindow>();
													  services.AddScoped<MainWindowViewModel>();

													  // Views and ViewModels
													  services.AddScoped<LauncherPage>();
													  services.AddScoped<LauncherViewModel>();

													  // Service for Downloads
													  services.AddScoped<DownloadService>();
												  })
												  .Build();

	/// <summary>
	///     Gets registered service.
	/// </summary>
	/// <typeparam name="T">Type of the service to get.</typeparam>
	/// <returns>Instance of the service or <see langword="null" />.</returns>
	public static T GetService<T>() where T : class
	{
		return (Host.Services.GetService(typeof(T)) as T)!;
	}

	/// <summary>
	///     Occurs when the application is loading.
	/// </summary>
	private async void OnStartup(object sender, StartupEventArgs e)
	{
		await Host.StartAsync();
	}

	/// <summary>
	///     Occurs when the application is closing.
	/// </summary>
	private async void OnExit(object sender, ExitEventArgs e)
	{
		await Host.StopAsync();

		Host.Dispose();
	}

	/// <summary>
	///     Occurs when an exception is thrown by an application but not handled.
	/// </summary>
	private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		var st = new StackTrace(e.Exception, true);
		var frame = st.GetFrame(0);

		if ( frame != null )
		{
			var fileName = frame.GetFileName();
			var methodName = frame.GetMethod()!.Name;
			var line = frame.GetFileLineNumber();
			var col = frame.GetFileColumnNumber();

			MessageBox.Show(MainWindow!, $"An Error occured {fileName}.{methodName}->L{line} C{col}\n{e.Exception}\n\nThe Program will be terminated", "Error Occured", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		else
		{
			MessageBox.Show(MainWindow!, "An Error occured but the StackTrace can't be read\n\nThe Program will be terminated", "Error Occured", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		Environment.Exit(0);
	}
}
