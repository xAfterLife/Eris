using ErisAdminPanel.ViewModels;
using Wpf.Ui.Common.Interfaces;

namespace ErisAdminPanel.Views.Pages;

/// <summary>
///     Interaction logic for FilesPage.xaml
/// </summary>
public partial class FilesPage : INavigableView<FilesViewModel>
{
	public FilesPage(FilesViewModel viewModel)
	{
		ViewModel = viewModel;

		InitializeComponent();
	}

	public FilesViewModel ViewModel { get; }
}
