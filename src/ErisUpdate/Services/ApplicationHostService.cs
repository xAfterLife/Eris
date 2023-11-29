using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ErisUpdate.Views.Windows;
using Microsoft.Extensions.Hosting;
using Wpf.Ui.Mvvm.Contracts;

namespace ErisUpdate.Services;

/// <summary>
///     Managed host of the application.
/// </summary>
public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private INavigationWindow _navigationWindow;

    public ApplicationHostService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    /// <summary>
    ///     Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Creates main window during activation.
    /// </summary>
    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if ( !Application.Current.Windows.OfType<MainWindow>().Any() )
        {
            var mainWindow = _serviceProvider.GetService(typeof(MainWindow)) as MainWindow;
            mainWindow?.Show();
        }

        await Task.CompletedTask;
    }
}