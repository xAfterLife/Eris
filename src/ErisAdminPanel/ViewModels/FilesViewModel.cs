using System;
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

public partial class FilesViewModel : ObservableObject, INavigationAware
{
	private const string DefaultButtonBackgroundColor = "#393939";
	private const string SuccessButtonBackgroundColor = "#107020";

	private readonly Config _configuration;

	[ObservableProperty]
	private string _updateManifestPath;

	[ObservableProperty]
	private string _sourceFolderPath;

	[ObservableProperty]
	private string _writeFileBackgroundColor = DefaultButtonBackgroundColor;

	public ObservableCollection<HttpFile>? Files { get; set; }

	public FilesViewModel(IConfiguration configuration)
	{
		_configuration = configuration.Get<Config>() ?? new Config { UpdateManifestUri = @"C:\", SourceFolderPath = @"C:\"};
		UpdateManifestPath = _configuration.UpdateManifestUri!;
		SourceFolderPath = _configuration.SourceFolderPath!;

		if ( File.Exists(UpdateManifestPath) )
		{
			FillFiles(SourceFolderPath);
		}
	}

	private void FillFiles(string path)
	{
		var len = path.Length;
		Files = new ObservableCollection<HttpFile>();
		foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
		{
			var fileInfo = new FileInfo(file);
			Files.Add(new HttpFile(file[len..], "tbd.", fileInfo.LastWriteTimeUtc, fileInfo.CreationTimeUtc, fileInfo.Length, Guid.NewGuid() ));
		}
	}

	public void OnNavigatedTo() { }

	public void OnNavigatedFrom() { }

	[RelayCommand]
	public void OpenInExplorer()
	{
		Process.Start("explorer.exe", $"{Path.GetDirectoryName(UpdateManifestPath)}");
	}

	[RelayCommand]
	public void SelectDirectory()
	{
		var dialog = new VistaSaveFileDialog { Title = @"Select the Folder the File will be Set to", FileName = "UpdateManifest.json", OverwritePrompt = false };

		if (dialog.ShowDialog() != true)
			return;

		UpdateManifestPath = dialog.FileName;
		_configuration.UpdateManifestUri = UpdateManifestPath;

		File.WriteAllText("appconfig.json", JsonSerializer.Serialize(_configuration));
	}

	[RelayCommand]
	public void SelectSourceDirectory()
	{
		var dialog = new VistaFolderBrowserDialog() { Description = @"Select the Folder the File will be Set to", UseDescriptionForTitle = true };

		if (dialog.ShowDialog() != true)
			return;

		SourceFolderPath = dialog.SelectedPath;
		_configuration.SourceFolderPath = SourceFolderPath;

		File.WriteAllText("appconfig.json", JsonSerializer.Serialize(_configuration));
	}

	[RelayCommand]
	public void WriteFile()
	{
		//TODO Write Json

		File.WriteAllText(UpdateManifestPath, Files?.ToJson());
		WriteFileBackgroundColor = SuccessButtonBackgroundColor;
		_ = Task.Run(async () =>
		{
			await Task.Delay(1000);
			WriteFileBackgroundColor = DefaultButtonBackgroundColor;
		});
	}
}
