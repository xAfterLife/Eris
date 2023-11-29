namespace ErisLib.Models;

[Serializable]
public readonly struct HttpFile
{
    public long Size { get; init; }
    public string Uri { get; init; }
    public string FileName { get; init; }
    public Guid VersionGuid { get; init; }
    public DateTime WriteDateUtc { get; init; }
    public DateTime CreationDateUtc { get; init; }

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