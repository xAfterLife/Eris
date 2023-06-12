using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ErisAdminPanel.Models;
using ErisLib;
using ErisLib.Models;
using Microsoft.Extensions.Configuration;
using Ookii.Dialogs.Wpf;
using Wpf.Ui.Common.Interfaces;

namespace ErisAdminPanel.ViewModels;

public partial class DashboardViewModel : ObservableObject, INavigationAware
{
	private readonly Config _configuration;

	[ObservableProperty]
	private string _newsFilePath;

	[ObservableProperty]
	private string _writeFileBackgroundColor = "#393939";

	public ObservableCollection<Patchnotes>? Patchnotes { get; set; }

	public DashboardViewModel(IConfiguration configuration)
	{
		_configuration = configuration.Get<Config>() ?? new Config { PatchnotesUri = @"C:\" };
		NewsFilePath = _configuration.PatchnotesUri!;

		if ( File.Exists(NewsFilePath) )
			Patchnotes = new ObservableCollection<Patchnotes>(PatchnotesService.ToPatchnotesList(File.ReadAllBytes(NewsFilePath))!);
		else
			Patchnotes = new ObservableCollection<Patchnotes>(new[] { new Patchnotes(default!, default!, "", "") });
	}

	public void OnNavigatedTo() {}

	public void OnNavigatedFrom() {}

	[RelayCommand]
	public void OpenInExplorer()
	{
		Process.Start("explorer.exe", $"{Path.GetDirectoryName(NewsFilePath)}");
	}

	[RelayCommand]
	public void SelectDirectory()
	{
		var dialog = new VistaSaveFileDialog { Title = @"Select the Folder the File will be Set to", FileName = "PatchNotes.json", OverwritePrompt = false };

		if ( dialog.ShowDialog() != true )
			return;

		NewsFilePath = dialog.FileName;
		_configuration.PatchnotesUri = NewsFilePath;

		File.WriteAllText("appconfig.json", JsonSerializer.Serialize(_configuration));
	}

	[RelayCommand]
	public void WriteFile()
	{
		File.WriteAllText(NewsFilePath, Patchnotes?.ToJson());
		WriteFileBackgroundColor = "#107020";
		_ = Task.Run(async () =>
		{
			await Task.Delay(1000);
			WriteFileBackgroundColor = "#393939";
		});
	}

	[RelayCommand]
	public void AddNews()
	{
		Patchnotes?.Add(new Patchnotes(default!, default!, string.Empty, string.Empty));
	}
}
