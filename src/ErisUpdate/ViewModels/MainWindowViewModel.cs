using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ErisUpdate.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle;

    [ObservableProperty]
    private float _windowOpacity = 0f;

    private bool _isInitialized;

    public MainWindowViewModel()
    {
        _applicationTitle = "ErisUpdate";

        if (!_isInitialized)
            InitializeViewModel();

        Simulate();
    }

    private async void Simulate()
    {
        await FadeIn(1250);
        await Task.Delay(1500);
        await FadeOut(1250);
    }

    private void InitializeViewModel()
    {
        _isInitialized = true;
    }

    public async Task FadeIn(int duration)
    {
        WindowOpacity = 0f;
        double steps = Convert.ToInt32(100 * (duration / 1000f));

        for (var i = 1; i <= steps; i++)
        {
            WindowOpacity = (float)(i / steps);
            await Task.Delay(10);
        }

        WindowOpacity = 1f;
    }

    public async Task FadeOut(int duration)
    {
        WindowOpacity = 1f;
        double steps = Convert.ToInt32(100 * (duration / 1000f));

        for (var i = Convert.ToInt32(steps); i >= 1; i--)
        {
            WindowOpacity = (float)(i / steps);
            await Task.Delay(10);
        }

        WindowOpacity = 0f;
    }
}