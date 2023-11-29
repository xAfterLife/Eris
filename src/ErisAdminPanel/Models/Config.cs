using System;

namespace ErisAdminPanel.Models;

[Serializable]
public class Config
{
    public string? PatchnotesUri { get; set; }
    public string? UpdateManifestUri { get; set; }
    public string? SourceFolderPath { get; set; }
}