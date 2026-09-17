using Motofushin.App.Application.Features.Gpx;

namespace Motofushin.App.Infrastructure.Services;

/// <summary>
/// GPX track store reading track files (*.gpx) from a configured assets directory.
/// </summary>
public sealed class FileSystemGpxTrackStore(string assetsPath = "C:\\Users\\Viktor\\source\\Pet\\motofushin\\motofushin-backend\\src\\Motofushin.App.Api\\Assets") : IGpxTrackStore
{
	public async Task<IReadOnlyList<string>?> GetNamesAsync(CancellationToken cancellationToken = default)
	{
		if (!Directory.Exists(assetsPath))
		{
			return null;
		}

		var names = Directory
			.EnumerateFiles(assetsPath, "*.gpx", new EnumerationOptions { MatchType = MatchType.Simple })
			.Select(Path.GetFileName)
			.ToList();

		return names;
	}

	public async Task<string?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
	{
		if (ContainsPathTraversal(name))
		{
			return null;
		}

		var filePath = Path.Combine(assetsPath, name);

		if (!File.Exists(filePath))
		{
			return null;
		}

		return await File.ReadAllTextAsync(filePath, cancellationToken);
	}

	private static bool ContainsPathTraversal(string name) =>
		name.Contains('/') || name.Contains('\\') || name.Contains("..");
}