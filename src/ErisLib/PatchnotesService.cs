using System.Text;
using System.Text.Json;
using ErisLib.Models;

namespace ErisLib;

public static class PatchnotesService
{
	public static async Task<List<Patchnotes>?> GetPatchNotes(Uri uri)
	{
		var client = new HttpClient();
		var result = await client.GetStringAsync(uri);
		var bytes = Encoding.UTF8.GetBytes(result);

		return ToPatchnotesList(bytes);
	}

	public static List<Patchnotes>? ToPatchnotesList(this ReadOnlySpan<byte> json)
	{
		if ( json == default )
			throw new ArgumentNullException(nameof(json));

		try
		{
			return JsonSerializer.Deserialize<List<Patchnotes>>(json);
		}
		catch ( Exception e )
		{
			Console.WriteLine(e);
			throw;
		}
	}

	public static string ToJson(this IEnumerable<Patchnotes> notes)
	{
		if ( notes == default )
			throw new ArgumentNullException(nameof(notes));

		try
		{
			return JsonSerializer.Serialize(notes);
		}
		catch ( Exception e )
		{
			Console.WriteLine(e);
			throw;
		}
	}
}
