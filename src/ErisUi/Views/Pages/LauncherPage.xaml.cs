using ErisUi.ViewModels;
using Wpf.Ui.Common.Interfaces;

namespace ErisUi.Views.Pages;

/// <summary>
///     Interaction logic for LauncherPage.xaml
/// </summary>
public partial class LauncherPage : INavigableView<LauncherViewModel>
{
	public LauncherPage(LauncherViewModel viewModel)
	{
		ViewModel = viewModel;

		InitializeComponent();
	}

	public LauncherViewModel ViewModel { get; }
}
