namespace ErisLib.Structs;

/// <summary>
///     Represents a file to be downloaded from a remote location via HTTP.
/// </summary>
public readonly struct HttpFile
{
    public readonly long Size;
    public readonly string Uri;
    public readonly string FileName;
    public readonly Guid VersionGuid;
    public readonly DateTime WriteDateUtc;
    public readonly DateTime CreationDateUtc;

    public HttpFile(string fileName, string uri, DateTime writeDateUtc, DateTime creationDateUtc, long size, Guid? versionGuid = null)
    {
        FileName = fileName;
        Uri = uri;
        WriteDateUtc = writeDateUtc;
        CreationDateUtc = creationDateUtc;
        Size = size;
        VersionGuid = versionGuid ?? Guid.NewGuid();
    }
}
