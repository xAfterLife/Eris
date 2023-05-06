namespace ErisLib.Models;

[Serializable]
public class Patchnotes
{
	public Uri BannerUri { get; set; }
	public Uri NavigationUri { get; set; }
	public string Title { get; set; }
	public string Content { get; set; }

	public Patchnotes(Uri bannerUri, Uri navigationUri, string title, string content)
	{
		BannerUri = bannerUri;
		NavigationUri = navigationUri;
		Title = title;
		Content = content;
	}
}
