using ErisAdminPanel.ViewModels;
using Wpf.Ui.Common.Interfaces;

namespace ErisAdminPanel.Views.Pages;

/// <summary>
///     Interaction logic for DashboardPage.xaml
/// </summary>
public partial class DashboardPage : INavigableView<DashboardViewModel>
{
	public DashboardPage(DashboardViewModel viewModel)
	{
		ViewModel = viewModel;

		InitializeComponent();
	}

	public DashboardViewModel ViewModel { get; }
}
